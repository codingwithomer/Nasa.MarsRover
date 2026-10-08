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
                throw new ArgumentException($"Empty input.", "input");
            }

            List<InputLine> commandLines = GetCommandLines(commandInput);

            int currentRoverIndex = -1;

            for (int i = 0; i < commandLines.Count; i++)
            {
                string commandLine = commandLines[i].Text;
                int lineNumber = commandLines[i].Number;

                if (commandLine.Length == 0)
                {
                    throw new ArgumentException($"Empty line not allowed on line {lineNumber}.", "input");
                }

                string[] commandLineParts = commandLine.Split();

                if (char.IsDigit(commandLine[0]))
                {
                    if (commandLineParts.Count() < 2 || commandLineParts.Count() > 3)
                    {
                        throw new ArgumentException($"Line starting with digit must have either two or three parts on line {lineNumber}.", "input");
                    }

                    if (!int.TryParse(commandLineParts[0], out int x) || x < 0)
                    {
                        throw new ArgumentException($"Cannot parse positive integer from {commandLineParts[0]} on line {lineNumber}.", "input");
                    }

                    if (!int.TryParse(commandLineParts[1], out int y) || y < 0)
                    {
                        throw new ArgumentException($"Cannot parse positive integer from {commandLineParts[1]} on line {lineNumber}.", "input");
                    }

                    if (commandLineParts.Count() == 2)
                    {
                        if (i != 0)
                        {
                            throw new ArgumentException($"Plateau initialization should be on the first line on line {lineNumber}.", "input");
                        }

                        Position position = new Position(x, y);

                        ICommand locationInitializeCommand = new LocationInitializeCommand(position);

                        commands.Add(locationInitializeCommand);
                    }

                    else
                    {
                        if (commandLineParts[2].Length != 1 || !CompassDirectionExtensions.TryParse(commandLineParts[2][0], out CompassDirection compassDirection))
                        {
                            throw new ArgumentException($"Rover initialization line should have either N, E, S, or W on the last part on line {lineNumber}.", "input");
                        }

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
                    foreach (char rotationString in commandLine)
                    {
                        SetRoverCommand(commands, lineNumber, rotationString);
                    }
                }
            }

            if (currentRoverIndex != -1)
            {
                commands.Add(new PrintPositionAndCompassDirectionCommand());
            }

            return commands;
        }

        private void SetRoverCommand(List<ICommand> commands, int lineNumber, char rotationCharacter)
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
                    throw new ArgumentException($"Rover rotation/move line should have either L, R, or M characters on line {lineNumber}.", "input");
            }
        }

        private List<InputLine> GetCommandLines(string commandInput)
        {
            string[] rawLines = commandInput.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');

            return rawLines.Select((line, index) => new InputLine(index + 1, line.Trim()))
                           .Where(line => line.Text.Length > 0)
                           .ToList();
        }

        private readonly record struct InputLine(int Number, string Text);
    }
}
