using Nasa.MarsRoboticRover.Application.Interfaces;
using System.IO;

namespace Nasa.MarsRoboticRover
{
    /// <summary>Reads the whole mission from a text file.</summary>
    public class FileMissionInputProvider : IMissionInputProvider
    {
        private readonly string _path;

        public FileMissionInputProvider(string path)
        {
            _path = path;
        }

        public string GetInput()
        {
            return File.ReadAllText(_path);
        }
    }
}
