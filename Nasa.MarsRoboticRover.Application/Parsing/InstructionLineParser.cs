using Nasa.MarsRoboticRover.Application.Commands;
using Nasa.MarsRoboticRover.Application.Interfaces;
using Nasa.MarsRoboticRover.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nasa.MarsRoboticRover.Application.Parsing
{
    /// <summary>A run of single-letter instructions such as "LMLMM". Letters map to commands through a registry.</summary>
    public class InstructionLineParser : ILineParser
    {
        public static IReadOnlyDictionary<char, Func<ICommand>> DefaultInstructions { get; } =
            new Dictionary<char, Func<ICommand>>
            {
                ['L'] = () => new RoverRotatorCommand(Rotation.Left),
                ['R'] = () => new RoverRotatorCommand(Rotation.Right),
                ['M'] = () => new MoveRoverCommand()
            };

        private readonly IReadOnlyDictionary<char, Func<ICommand>> _instructions;

        public InstructionLineParser() : this(DefaultInstructions)
        {
        }

        public InstructionLineParser(IReadOnlyDictionary<char, Func<ICommand>> instructions)
        {
            _instructions = instructions;
        }

        public LineKind Kind => LineKind.Instructions;

        public bool CanParse(InputLine line)
        {
            return !LineValueParser.StartsWithDigit(line);
        }

        public IEnumerable<ICommand> Parse(InputLine line)
        {
            List<ICommand> commands = new List<ICommand>();

            foreach (char letter in line.Text)
            {
                if (!_instructions.TryGetValue(letter, out Func<ICommand> createCommand))
                {
                    string allowed = string.Join(", ", _instructions.Keys);
                    throw new ArgumentException($"Rover rotation/move line should have only these characters: {allowed} on line {line.Number}.", "input");
                }

                commands.Add(createCommand());
            }

            return commands;
        }
    }
}
