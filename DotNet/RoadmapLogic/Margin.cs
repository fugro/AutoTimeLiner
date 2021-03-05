namespace RoadmapLogic
{
    public class Margin
    {
        public Margin(int all) : this(all, all, all, all) { }

        public Margin(int rightLeft, int topBottom) : this(rightLeft, topBottom, rightLeft, topBottom) { }

        public Margin(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public int Left { get; }
        public int Top { get; }
        public int Right { get; }
        public int Bottom { get; }
    }
}
