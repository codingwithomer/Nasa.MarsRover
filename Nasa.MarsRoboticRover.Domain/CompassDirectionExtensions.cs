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
    }
}
