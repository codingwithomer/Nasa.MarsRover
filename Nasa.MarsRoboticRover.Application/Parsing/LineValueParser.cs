using Nasa.MarsRoboticRover.Domain;
using System.Globalization;
using System.Linq;

namespace Nasa.MarsRoboticRover.Application.Parsing
{
    internal static class LineValueParser
    {
        /// <summary>Starts like a coordinate line: a digit, or a sign followed by a digit ("-1 2 N").</summary>
        public static bool LooksLikeCoordinates(InputLine line)
        {
            string text = line.Text;

            return char.IsAsciiDigit(text[0]) ||
                   ((text[0] == '-' || text[0] == '+') && text.Length > 1 && char.IsAsciiDigit(text[1]));
        }

        public static Position ParsePlateau(InputLine line)
        {
            string[] parts = line.Parts;

            if (parts.Length != 2 || !parts.All(IsWholeNumber))
            {
                throw new InvalidMissionException(
                    $"The plateau on line {line.Number} must be two non-negative whole numbers (X Y), found '{line.Text}'.");
            }

            return new Position(ParseWholeNumber(line, parts[0]), ParseWholeNumber(line, parts[1]));
        }

        public static Position ParseRoverPosition(InputLine line, string[] parts)
        {
            if (parts.Length != 3)
            {
                throw new InvalidMissionException(
                    $"A rover position on line {line.Number} needs X Y and a heading (for example 1 2 N), found '{line.Text}'.");
            }

            string badCoordinate = parts.Take(2).FirstOrDefault(part => !IsWholeNumber(part));

            if (badCoordinate != null)
            {
                throw new InvalidMissionException(
                    $"The rover coordinates on line {line.Number} must be non-negative whole numbers, found '{badCoordinate}'.");
            }

            return new Position(ParseWholeNumber(line, parts[0]), ParseWholeNumber(line, parts[1]));
        }

        public static CompassDirection ParseCompassDirection(InputLine line, string part)
        {
            if (part.Length == 1 && CompassDirectionLetters.TryParse(part[0], out CompassDirection compassDirection))
            {
                return compassDirection;
            }

            if (part.Length == 1 && CompassDirectionLetters.TryParse(char.ToUpperInvariant(part[0]), out _))
            {
                throw new InvalidMissionException(
                    $"Letters must be upper case: use '{char.ToUpperInvariant(part[0])}' instead of '{part}' as the heading on line {line.Number}.");
            }

            throw new InvalidMissionException($"The heading on line {line.Number} must be N, E, S or W, found '{part}'.");
        }

        private static bool IsWholeNumber(string part)
        {
            return part.All(char.IsAsciiDigit);
        }

        private static int ParseWholeNumber(InputLine line, string part)
        {
            if (!int.TryParse(part, NumberStyles.None, CultureInfo.InvariantCulture, out int value))
            {
                throw new InvalidMissionException($"The number '{part}' on line {line.Number} is too large (the maximum is {int.MaxValue}).");
            }

            return value;
        }
    }
}
