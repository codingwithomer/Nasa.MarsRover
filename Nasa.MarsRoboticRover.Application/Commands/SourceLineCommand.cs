using Nasa.MarsRoboticRover.Application.Interfaces;

namespace Nasa.MarsRoboticRover.Application.Commands
{
    /// <summary>A command together with the 1-based input line it came from, so execution errors can name the line.</summary>
    internal sealed class SourceLineCommand : ICommand
    {
        public SourceLineCommand(ICommand command, int line)
        {
            Command = command;
            Line = line;
        }

        public ICommand Command { get; }

        public int Line { get; }

        public void Execute(MissionContext context)
        {
            Command.Execute(context);
        }
    }
}
