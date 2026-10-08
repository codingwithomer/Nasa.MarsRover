using Nasa.MarsRoboticRover.Application.Interfaces;
using System.IO;

namespace Nasa.MarsRoboticRover
{
    /// <summary>Reads the whole mission from a reader such as standard input.</summary>
    public class TextReaderMissionInputProvider : IMissionInputProvider
    {
        private readonly TextReader _reader;

        public TextReaderMissionInputProvider(TextReader reader)
        {
            _reader = reader;
        }

        public string GetInput()
        {
            return _reader.ReadToEnd();
        }
    }
}
