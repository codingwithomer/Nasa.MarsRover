using Nasa.MarsRoboticRover.Application.Commands;
using Nasa.MarsRoboticRover.Application.Interfaces;
using Nasa.MarsRoboticRover.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nasa.MarsRoboticRover.Application.Parsing
{
    /// <summary>
    /// Immutable letter -> command table. Commands are stateless, so one instance per letter is shared by every
    /// occurrence of that letter.
    /// </summary>
    public class InstructionSet : IInstructionSet
    {
        private readonly IReadOnlyDictionary<char, ICommand> _commands;

        public InstructionSet(IEnumerable<KeyValuePair<char, ICommand>> commands)
        {
            ArgumentNullException.ThrowIfNull(commands);

            Dictionary<char, ICommand> table = new Dictionary<char, ICommand>();

            foreach (KeyValuePair<char, ICommand> pair in commands)
            {
                if (!table.TryAdd(pair.Key, pair.Value))
                {
                    throw new ArgumentException($"letter '{pair.Key}' is already defined", nameof(commands));
                }
            }

            _commands = table;
        }

        public IReadOnlyCollection<char> Letters => _commands.Keys.OrderBy(letter => letter).ToList();

        public static InstructionSet CreateDefault()
        {
            return new InstructionSet(new Dictionary<char, ICommand>
            {
                ['L'] = new RotateRoverCommand(Rotation.Left),
                ['R'] = new RotateRoverCommand(Rotation.Right),
                ['M'] = new MoveRoverCommand()
            });
        }

        /// <summary>Returns a new set that also understands <paramref name="letter"/>.</summary>
        public InstructionSet With(char letter, ICommand command)
        {
            return new InstructionSet(_commands.Append(new KeyValuePair<char, ICommand>(letter, command)));
        }

        public bool TryGetCommand(char letter, out ICommand command)
        {
            return _commands.TryGetValue(letter, out command);
        }
    }
}
