using SixLabors.ImageSharp;

namespace RoadmapLogic
{
    public class DogLeg
    {
        public PointF[] Points { get; }

        public DogLeg(float xPos, int index, Settings settings, Position position)
        {
            Points = new PointF[3];
            
            if (position == Position.Top)
            {
                Points[0] = new PointF(xPos, settings.MidPoint - (settings.ChevronHeight / 2) - 1);
                Points[2] = new PointF(xPos + settings.DogLegWidth, Points[0].Y - settings.TopOffsets[index]);
            }
            else
            {
                Points[0] = new PointF(xPos, settings.MidPoint + (settings.ChevronHeight / 2) + 1);
                Points[2] = new PointF(xPos + settings.DogLegWidth, Points[0].Y + settings.BottomOffsets[index]);
            }
            Points[1] = new PointF(Points[0].X, Points[2].Y);
        }
    }
}
