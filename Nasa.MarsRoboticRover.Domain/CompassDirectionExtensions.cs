namespace Nasa.MarsRoboticRover.Domain
{
    public static class CompassDirectionExtensions
    {
        public static CompassDirection TurnLeft(this CompassDirection compassDirection)
        {
            return compassDirection switch
            {
                CompassDirection.North => CompassDirection.West,
                CompassDirection.West => CompassDirection.South,
                CompassDirection.South => CompassDirection.East,
                CompassDirection.East => CompassDirection.North,
                _ => throw new System.ArgumentOutOfRangeException(nameof(compassDirection), compassDirection, null)
            };
        }

        public static CompassDirection TurnRight(this CompassDirection compassDirection)
        {
            return compassDirection switch
            {
                CompassDirection.North => CompassDirection.East,
                CompassDirection.East => CompassDirection.South,
                CompassDirection.South => CompassDirection.West,
                CompassDirection.West => CompassDirection.North,
                _ => throw new System.ArgumentOutOfRangeException(nameof(compassDirection), compassDirection, null)
            };
        }

        public static char ToLetter(this CompassDirection compassDirection)
        {
            return compassDirection switch
            {
                CompassDirection.North => 'N',
                CompassDirection.East => 'E',
                CompassDirection.South => 'S',
                CompassDirection.West => 'W',
                _ => throw new System.ArgumentOutOfRangeException(nameof(compassDirection), compassDirection, null)
            };
        }

        public static bool TryParse(char letter, out CompassDirection compassDirection)
        {
            switch (letter)
            {
                case 'N':
                    compassDirection = CompassDirection.North;
                    return true;
                case 'E':
                    compassDirection = CompassDirection.East;
                    return true;
                case 'S':
                    compassDirection = CompassDirection.South;
                    return true;
                case 'W':
                    compassDirection = CompassDirection.West;
                    return true;
                default:
                    compassDirection = default;
                    return false;
            }
        }
    }
}
