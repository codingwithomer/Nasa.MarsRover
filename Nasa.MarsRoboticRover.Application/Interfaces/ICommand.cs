namespace Nasa.MarsRoboticRover.Application.Interfaces
{
    public interface ICommand
    {
        void Execute(MissionContext context);
    }
}
