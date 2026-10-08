# Mars Rover

A squad of robotic rovers are to be landed by NASA on a plateau on Mars. This plateau, which is
curiously rectangular, must be navigated by the rovers so that their on board cameras can get a
complete view of the surrounding terrain to send back to Earth.

A rover's position and location is represented by a combination of x and y co-ordinates and a letter
representing one of the four cardinal compass points. The plateau is divided up into a grid to
simplify navigation. An example position might be 0, 0, N, which means the rover is in the bottom
left corner and facing North.

In order to control a rover, NASA sends a simple string of letters. The possible letters are 'L', 'R' and
'M'. 'L' and 'R' makes the rover spin 90 degrees left or right respectively, without moving from its
current spot. 'M' means move forward one grid point, and maintain the same heading.
Assume that the square directly North from (x, y) is (x, y+1).

Input:

The first line of input is the upper-right coordinates of the plateau, the lower-left coordinates are
assumed to be 0,0.
The rest of the input is information pertaining to the rovers that have been deployed. Each rover
has two lines of input. The first line gives the rover's position, and the second line is a series of
instructions telling the rover how to explore the plateau.

The position is made up of two integers and a letter separated by spaces, corresponding to the x
and y co-ordinates and the rover's orientation.
Each rover will be finished sequentially, which means that the second rover won't start to move
until the first one has finished moving.

Output:

The output for each rover should be its final co-ordinates and heading.

Input and Output

Test Input:

5 5
<br>
1 2 N
<br>
LMLMLMLMM
<br>
3 3 E
<br>
MMRMMRMRRM

Expected Output:

1 3 N
<br>
5 1 E


## Running

```
dotnet run --project Nasa.MarsRoboticRover                  # built-in sample mission (the example above)
dotnet run --project Nasa.MarsRoboticRover -- mission.txt   # mission read from a file
cat mission.txt | dotnet run --project Nasa.MarsRoboticRover # mission read from standard input
dotnet run --project Nasa.MarsRoboticRover -- --help        # usage (also -h)
```

Output contract:

- The built-in sample is shown as an example: the input under `Test Input:` and the report under `Expected Output:`.
  It waits for a key press when run interactively.
- A mission from a file or from standard input prints **only the report** on stdout, one `X Y H` line per rover and
  nothing else, so scripts can consume it. It never waits for a key.
- Redirected standard input that is empty is an error (`Empty input.`), it does not fall back to the sample.
- Errors go to stderr, never to stdout. If a rover fails, the rovers that had already finished are still printed on
  stdout and the error names the failing rover and input line (`Rover 2 (line 4): ...`).

Exit codes: `0` success, `1` invalid mission or usage, `2` the input could not be read (missing file, a directory,
an empty file name, no permission).
`dotnet test` runs the whole suite.

## Rules

- The plateau is a grid of whole-number points from `(0, 0)` to the upper-right corner on the first line. The first
  non-blank line is always the plateau; blank lines are ignored; `\n`, `\r\n` and `\r` line endings and a UTF-8 byte
  order mark are accepted.
- Numbers are plain non-negative whole numbers up to 2147483647 (no sign, no decimals).
- A rover landing outside the plateau, or moving off it, is an error: it is never silently ignored.
- Rovers cannot share a square. A rover cannot land on, or move onto, a square an earlier rover currently occupies;
  a square an earlier rover has left is free again.
- Rovers run sequentially: the second rover starts only when the first has finished all its instructions.
- When a rover fails the mission stops. The rovers that finished before it are still reported, the error names the
  failing rover and line, and the exit code is `1`.
- Headings (`N`, `E`, `S`, `W`) and instructions (`L`, `R`, `M`) must be upper case; lowercase is rejected with a
  message saying so. Instructions are written without spaces (`MM`, not `M M`).
- Every rover needs an instruction line right after its position; a rover followed by another rover or by the end
  of the input is rejected.
- A mission that only defines the plateau is valid and has an empty report.

## Architecture

Dependencies point inwards only (enforced by `DependencyRuleTests`, on the project files and on the compiled assemblies):

```
Nasa.MarsRoboticRover              console host: composition root, input sources, report headers, exit codes
  -> Nasa.MarsRoboticRover.Application   use cases: parsing, commands, MissionContext, MissionExecutor
       -> Nasa.MarsRoboticRover.Domain   Plateau, MarsRover, Position, compass rules
```

- **Domain** knows nothing about input, commands or output. `Plateau` is created with its size, enforces bounds and
  occupancy, and is the only way to deploy a `MarsRover`. Rovers depend on the narrow `ITerrain` interface.
- **Application** turns text into commands (`CommandParser` coordinating one `ILineParser` per line kind) and runs them
  against a per-run `MissionContext` (`MissionExecutor`). The report is the final state of each rover on the plateau.
  All services are stateless.
- **Console** wires everything in `AddMarsRover()`, picks the input source and prints the report.

Errors: everything that is wrong with the user's mission is an `InvalidMissionException` (a parse error naming the
1-based input line, or a rover that cannot carry out its instructions, naming the rover and the line). Other exception
types are programming errors and are not reported as an invalid mission. In the domain, an argument that can never
be valid (outside the plateau, unknown heading) is an `ArgumentOutOfRangeException`; a call that the current state forbids
(occupied square, blocked move) is an `InvalidOperationException`.

### Extending

| To add...                  | Do this                                                                                          |
|----------------------------|--------------------------------------------------------------------------------------------------|
| a new instruction letter   | implement `ICommand` if needed, then change the `IInstructionSet` registration in `AddMarsRover()` to `InstructionSet.CreateDefault().With('X', command)` |
| a new rover behavior       | implement `ICommand` (it works on `MissionContext`)                                              |
| a different input source   | implement `IMissionInputProvider` and register it in the host (`AddMarsRover()` registers none)  |
| a new kind of input line   | implement `ILineParser`, add a `LineKind` value, register it, and teach `CommandParser` about it (ordering rules, and which parser claims a line, live there and in `InputLine.IsFirst`) |

Adding an instruction letter or an input source is additive (one registration in the host). A new *line kind* is not:
the ordering rules (plateau first, instructions after a rover, no rover without instructions) live in `CommandParser`,
so it has to learn about the new kind.

### Patterns

Command (`ICommand`), Strategy (`ILineParser`), a letter -> command registry (`IInstructionSet`) and constructor
Dependency Injection with a single composition root. Nothing else is used on purpose: this is a small problem.
