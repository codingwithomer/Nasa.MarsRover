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


## Architecture

Dependencies point inwards only (enforced by `DependencyRuleTests`):

```
Nasa.MarsRoboticRover              console app and composition root (DI wiring, report formatting)
  -> Nasa.MarsRoboticRover.Application   use cases: parsing, commands, mission execution
       -> Nasa.MarsRoboticRover.Domain   Plateau, MarsRover, Position, compass/rotation rules
```

- **Domain** knows nothing about input, commands or output. `Plateau` is created with its size, enforces bounds and
  occupancy, and is the only way to deploy a `MarsRover`. Rovers depend on the narrow `ITerrain` interface.
- **Application** turns text into commands (`CommandParser` + one `ILineParser` per line kind) and runs them against a
  per-run `MissionContext` (`CommandCenter`). All services are stateless.
- **Console** wires everything in `AddMarsRover()` and prints the report.

Extending the system:

| To add...                    | Do this                                                                 |
|------------------------------|-------------------------------------------------------------------------|
| a new instruction letter     | add a `letter -> command` entry to the `InstructionLineParser` registry |
| a new kind of input line     | implement `ILineParser` and register it                                 |
| a different input source     | implement `IMissionInputProvider` (file, stdin, ...) and register it    |
| a new rover/mission behavior | add an `ICommand`; it works on `MissionContext`                         |

Patterns used, each where the problem asks for it: Command (`ICommand`), Strategy (`ILineParser`),
Factory/registry (instruction letters), Dependency Injection (constructor injection, composition root).
