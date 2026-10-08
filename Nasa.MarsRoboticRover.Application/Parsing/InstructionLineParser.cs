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
            return !line.IsFirst && !LineValueParser.LooksLikeCoordinates(line);
        }

        public IReadOnlyList<ICommand> Parse(InputLine line)
        {
            List<ICommand> commands = new List<ICommand>();

            foreach (char letter in line.Text)
            {
                if (!_instructionSet.TryGetCommand(letter, out ICommand command))
                {
                    throw new InvalidMissionException(Describe(letter, line));
                }

                commands.Add(command);
            }

            return commands;
        }

        private string Describe(char letter, InputLine line)
        {
            if (char.IsWhiteSpace(letter))
            {
                return $"Spaces are not allowed inside instructions on line {line.Number} (write 'MM', not 'M M').";
            }

            if (_instructionSet.TryGetCommand(char.ToUpperInvariant(letter), out _))
            {
                return $"Letters must be upper case: use '{char.ToUpperInvariant(letter)}' instead of '{letter}' on line {line.Number}.";
            }

            return $"Unknown instruction '{letter}' on line {line.Number}. Allowed instructions: {string.Join(", ", _instructionSet.Letters)}.";
        }
    }
}
