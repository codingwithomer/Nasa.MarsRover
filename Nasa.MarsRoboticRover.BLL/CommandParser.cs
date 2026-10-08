using Nasa.MarsRoboticRover.BLL.Commands;
using Nasa.MarsRoboticRover.BLL.Interfaces;
using Nasa.MarsRoboticRover.BLL.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nasa.MarsRoboticRover.BLL
{
    /// <summary>
    /// Walks the input line by line, delegates each line to the <see cref="ILineParser"/> that understands it
    /// and enforces the ordering of the input: plateau first, then (rover, instructions) pairs.
    /// </summary>
    public class CommandParser : IParser
    {
        private readonly IReadOnlyList<ILineParser> _lineParsers;

        public CommandParser(IEnumerable<ILineParser> lineParsers)
        {
            _lineParsers = lineParsers.ToList();
        }

        public List<ICommand> Parse(string commandInput)
        {
            if (string.IsNullOrWhiteSpace(commandInput))
            {
                throw new ArgumentException("Empty input.", "input");
            }

            List<ICommand> commands = new List<ICommand>();
            bool isFirstLine = true;
            bool roverSeen = false;

            foreach (InputLine line in InputLine.Split(commandInput))
            {
                ILineParser lineParser = FindParser(line);

                ValidateOrder(lineParser.Kind, line, isFirstLine, roverSeen);

                if (lineParser.Kind == LineKind.Rover && roverSeen)
                {
                    commands.Add(new PrintPositionAndCompassDirectionCommand());
                }

                commands.AddRange(lineParser.Parse(line));

                roverSeen |= lineParser.Kind == LineKind.Rover;
                isFirstLine = false;
            }

            if (roverSeen)
            {
                commands.Add(new PrintPositionAndCompassDirectionCommand());
            }

            return commands;
        }

        private ILineParser FindParser(InputLine line)
        {
            ILineParser lineParser = _lineParsers.FirstOrDefault(parser => parser.CanParse(line));

            if (lineParser == null)
            {
                throw new ArgumentException($"Line {line.Number} is not a plateau (X Y), a rover position (X Y H) or a list of instructions.", "input");
            }

            return lineParser;
        }

        private static void ValidateOrder(LineKind kind, InputLine line, bool isFirstLine, bool roverSeen)
        {
            if (isFirstLine && kind != LineKind.Plateau)
            {
                throw new ArgumentException($"The first line must define the plateau, found line {line.Number}.", "input");
            }

            if (!isFirstLine && kind == LineKind.Plateau)
            {
                throw new ArgumentException($"Plateau initialization should be on the first line on line {line.Number}.", "input");
            }

            if (kind == LineKind.Instructions && !roverSeen)
            {
                throw new ArgumentException($"Instructions on line {line.Number} must follow a rover position.", "input");
            }
        }
    }
}
