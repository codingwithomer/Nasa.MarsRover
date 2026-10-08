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
```

Exit codes: `0` success, `1` invalid mission or usage (message on stderr), `2` input could not be read.
`dotnet test` runs the whole suite.

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

Errors: parse errors are `ArgumentException`s that name the 1-based input line. In the domain, an argument that can never
be valid (outside the plateau, unknown heading) is an `ArgumentOutOfRangeException`; a call that the current state forbids
(occupied square, blocked move) is an `InvalidOperationException`.

### Extending

| To add...                  | Do this                                                                                          |
|----------------------------|--------------------------------------------------------------------------------------------------|
| a new instruction letter   | `InstructionSet.CreateDefault().With('X', command)`; nothing else changes                        |
| a new rover behavior       | implement `ICommand` (it works on `MissionContext`)                                              |
| a different input source   | implement `IMissionInputProvider` and register it                                                |
| a new kind of input line   | implement `ILineParser`, add a `LineKind` value, register it, and extend `CommandParser.ValidateOrder` if the new kind has ordering rules |

Adding an instruction letter or an input source is purely additive. A new *line kind* is not: the ordering rules
(plateau first, instructions after a rover) live in `CommandParser`, so it has to learn about the new kind.

### Patterns

Command (`ICommand`), Strategy (`ILineParser`), a letter -> command registry (`IInstructionSet`) and constructor
Dependency Injection with a single composition root. Nothing else is used on purpose: this is a small problem.
