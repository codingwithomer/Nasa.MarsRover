using Nasa.MarsRoboticRover.Domain;
using System;
using System.Globalization;

namespace Nasa.MarsRoboticRover.Application.Parsing
{
    internal static class LineValueParser
    {
        public static bool StartsWithDigit(InputLine line)
        {
            return char.IsAsciiDigit(line.Text[0]);
        }

        public static Position ParsePosition(InputLine line, string[] parts)
        {
            return new Position(ParseCoordinate(line, parts[0]), ParseCoordinate(line, parts[1]));
        }

        public static CompassDirection ParseCompassDirection(InputLine line, string part)
        {
            if (part.Length != 1 || !CompassDirectionLetters.TryParse(part[0], out CompassDirection compassDirection))
            {
                throw new InvalidMissionException($"Rover initialization line should have either N, E, S, or W on the last part on line {line.Number}.");
            }

            return compassDirection;
        }

        private static int ParseCoordinate(InputLine line, string part)
        {
            if (!int.TryParse(part, NumberStyles.None, CultureInfo.InvariantCulture, out int value))
            {
                throw new InvalidMissionException($"Cannot parse positive integer from {part} on line {line.Number}.");
            }

            return value;
        }
    }
}
