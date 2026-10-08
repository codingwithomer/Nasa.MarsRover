using System.Collections.Generic;
using System.Linq;

namespace Nasa.MarsRoboticRover.Application.Parsing
{
    /// <summary>A non-blank, trimmed input line together with its 1-based line number in the original text.</summary>
    public readonly record struct InputLine(int Number, string Text)
    {
        public string[] Parts => Text.Split(' ', System.StringSplitOptions.RemoveEmptyEntries | System.StringSplitOptions.TrimEntries);

        public static List<InputLine> Split(string input)
        {
            string[] rawLines = input.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');

            return rawLines.Select((line, index) => new InputLine(index + 1, line.Trim()))
                           .Where(line => line.Text.Length > 0)
                           .ToList();
        }
    }
}
