using System.Collections.Generic;

namespace Nasa.MarsRoboticRover.Application.Parsing
{
    /// <summary>
    /// A non-blank, trimmed input line together with its 1-based line number in the original text.
    /// <see cref="IsFirst"/> marks the first non-blank line, which is the plateau whatever it contains.
    /// </summary>
    public readonly record struct InputLine(int Number, string Text, bool IsFirst = false)
    {
        private const char ByteOrderMark = '\uFEFF';

        public string[] Parts => Text.Split((char[])null, System.StringSplitOptions.RemoveEmptyEntries | System.StringSplitOptions.TrimEntries);

        public static List<InputLine> Split(string input)
        {
            // A UTF-8 byte order mark can survive in text read from standard input; it is not part of the first line.
            string[] rawLines = input.TrimStart(ByteOrderMark).Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');

            List<InputLine> lines = new List<InputLine>();

            for (int index = 0; index < rawLines.Length; index++)
            {
                string text = rawLines[index].Trim();

                if (text.Length > 0)
                {
                    lines.Add(new InputLine(index + 1, text, lines.Count == 0));
                }
            }

            return lines;
        }
    }
}
