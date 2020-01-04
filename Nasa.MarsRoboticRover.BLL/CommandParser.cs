using Nasa.MarsRoboticRover.BLL.Interfaces;
using Nasa.MarsRoboticRover.Entities;
using Nasa.MarsRoboticRover.Entities.Commands;
using Nasa.MarsRoboticRover.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nasa.MarsRoboticRover.BLL
{
    public class CommandParser : IParser
    {
        public List<ICommand> Parse(string commandInput)
        {
            List<ICommand> commands = new List<ICommand>();

            if (string.IsNullOrEmpty(commandInput))
            {
                throw new ArgumentException("input", $"Empty input.");
            }

            string[] lines = GetCommandLines(commandInput);

            int currentRoverIndex = -1;

            for (int i = 0; i < lines.Count(); i++)
            {
                string line = lines[i];

                if (line.Length == 0)
                {
                    throw new ArgumentException("input", $"Empty line not allowed on line {i}.");
                }

                string[] lineParts = line.Split();

                if (char.IsDigit(line[0]))
                {
                    if (lineParts.Count() < 2 || lineParts.Count() > 3)
                    {
                        throw new ArgumentException("input", $"Line starting with digit must have either two or three parts on line {i}.");
                    }

                    if (!int.TryParse(lineParts[0], out int x) || x < 0)
                    {
                        throw new ArgumentException("input", $"Cannot parse positive integer from {lineParts[0]} on line {i}.");
                    }

                    if (!int.TryParse(lineParts[1], out int y) || y < 0)
                    {
                        throw new ArgumentException("input", $"Cannot parse positive integer from {lineParts[1]} on line {i}.");
                    }

                    if (lineParts.Count() == 2)
                    {
                        if (i != 0)
                        {
                            throw new ArgumentException("input", $"Plateau initialization should be on the first line on line {i}.");
                        }

                        Position position = new Position(x, y);

                        ICommand locationInitializeCommand = new LocationInitializeCommand(position);

                        commands.Add(locationInitializeCommand);
                    }

                    else
                    {
                        if (lineParts[2].Length != 1 || !"NESW".Contains(lineParts[2]))
                        {
                            throw new ArgumentException("input", $"Rover initialization line should have either N, E, S, or W on the last part on line {i}.");
                        }

                        CompassDirection compassDirection = GetCompassDirection(lineParts);

                        if (currentRoverIndex != -1)
                        {
                            commands.Add(new PrintPositionAndCompassDirectionCommand());
                        }

                        currentRoverIndex++;

                        Position roverPosition = new Position(x, y);

                        ICommand roverCreationCommand = new RoverCreationCommand(roverPosition, compassDirection);

                        commands.Add(roverCreationCommand);
                    }
                }
                else
                {
                    foreach (char rotationString in line)
                    {
                        SetRoverCommand(commands, i, rotationString);
                    }
                }
            }

            if (currentRoverIndex != -1)
            {
                commands.Add(new PrintPositionAndCompassDirectionCommand());
            }

            return commands;
        }

        private void SetRoverCommand(List<ICommand> commands, int index, char rotationCharacter)
        {
            switch (rotationCharacter)
            {
                case 'L':
                    {
                        ICommand rotatorCommand = new RoverRotatorCommand(Rotation.Left);
                        commands.Add(rotatorCommand);
                        break;
                    }
                case 'R':
                    {
                        ICommand rotatorCommand = new RoverRotatorCommand(Rotation.Right);
                        commands.Add(rotatorCommand);
                        break;
                    }
                case 'M':
                    {
                        ICommand moveRoverCommand = new MoveRoverCommand();
                        commands.Add(moveRoverCommand);
                        break;
                    }
                default:
                    throw new ArgumentException("input", $"Rover rotation/move line should have either L, R, or M characters on line {index}.");
            }
        }

        private CompassDirection GetCompassDirection(string[] lineParts)
        {
            CompassDirection compassDirection = CompassDirection.North;

            switch (lineParts[2][0])
            {
                case 'N':
                    compassDirection = CompassDirection.North;
                    break;
                case 'E':
                    compassDirection = CompassDirection.East;
                    break;
                case 'S':
                    compassDirection = CompassDirection.South;
                    break;
                case 'W':
                    compassDirection = CompassDirection.West;
                    break;
            }

            return compassDirection;
        }

        private string[] GetCommandLines(string commandInput)
        {
            string[] commandLines = commandInput.Split(new[] { '\r', '\n' })
                                                .Where(line => !string.IsNullOrEmpty(line))
                                                .Select(line => line.Trim()).ToArray();

            return commandLines;
        }
    }
}
