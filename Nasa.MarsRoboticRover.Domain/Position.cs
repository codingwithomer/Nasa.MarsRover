namespace Nasa.MarsRoboticRover.Domain
{
    public readonly record struct Position(int X, int Y)
    {
        public static Position Origin => new Position(0, 0);

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public bool IsWithin(Position min, Position max)
        {
            return ((X >= min.X) && (X <= max.X)) &&
                   ((Y >= min.Y) && (Y <= max.Y));
        }
    }
}
