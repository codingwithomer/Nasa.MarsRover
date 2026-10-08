using Nasa.MarsRoboticRover.Application.Interfaces;
using System;
using System.Collections.Generic;

namespace Nasa.MarsRoboticRover.Application.Parsing
{
    /// <summary>A run of single-letter instructions such as "LMLMM", resolved through an <see cref="IInstructionSet"/>.</summary>
    public class InstructionLineParser : ILineParser
    {
        private readonly IInstructionSet _instructionSet;

        public InstructionLineParser(IInstructionSet instructionSet)
        {
            ArgumentNullException.ThrowIfNull(instructionSet);

            _instructionSet = instructionSet;
        }

        public LineKind Kind => LineKind.Instructions;

        public bool CanParse(InputLine line)
        {
            return !LineValueParser.StartsWithDigit(line);
        }

        public IReadOnlyList<ICommand> Parse(InputLine line)
        {
            List<ICommand> commands = new List<ICommand>();

            foreach (char letter in line.Text)
            {
                if (!_instructionSet.TryGetCommand(letter, out ICommand command))
                {
                    throw new ArgumentException(
                        $"Unknown instruction '{letter}' on line {line.Number}. Allowed instructions: {string.Join(", ", _instructionSet.Letters)}.",
                        "input");
                }

                commands.Add(command);
            }

            return commands;
        }
    }
}
