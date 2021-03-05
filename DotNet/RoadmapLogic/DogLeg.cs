using SixLabors.ImageSharp;

namespace RoadmapLogic
{
    public class DogLeg
    {
        //ToDo: update to avoid hard coded offsets. Move to settings maybe!
        private static readonly int[] TopOffsets = { 217, 141, 65 };
        private static readonly int[] BottomOffsets = { 176, 100, 24 };

        public PointF[] Points { get; }

        public DogLeg(float xPos, int index, Settings settings, Position position)
        {
            Points = new PointF[3];
            
            if (position == Position.Top)
            {
                Points[0] = new PointF(xPos, settings.MidPoint - (settings.ChevronHeight / 2) - 1);
                Points[2] = new PointF(xPos + settings.DogLegWidth, Points[0].Y - TopOffsets[index]);
            }
            else
            {
                Points[0] = new PointF(xPos, settings.MidPoint + (settings.ChevronHeight / 2) + 1);
                Points[2] = new PointF(xPos + settings.DogLegWidth, Points[0].Y + BottomOffsets[index]);
            }
            Points[1] = new PointF(Points[0].X, Points[2].Y);
        }
    }
}
