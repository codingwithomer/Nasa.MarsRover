using Nasa.MarsRoboticRover.Application.Interfaces;
using System.Collections.Generic;

namespace Nasa.MarsRoboticRover.Application.Parsing
{
    /// <summary>The single-letter instructions a rover understands and the command each one stands for.</summary>
    public interface IInstructionSet
    {
        IReadOnlyCollection<char> Letters { get; }

        bool TryGetCommand(char letter, out ICommand command);
    }
}
