namespace Nasa.MarsRoboticRover.Entities
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public static Position Origin
        {
            get
            {
                return new Position(0, 0);
            }
        }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool IsWithin(Position min, Position max)
        {
            return ((X >= min.X) && (X <= max.X)) && 
                   ((Y >= min.Y) && (Y <= max.Y));
        }
    }
}
