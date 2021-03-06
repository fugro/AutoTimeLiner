using SixLabors.ImageSharp;

namespace RoadmapLogic
{
    public sealed class PlacardLocation
    {
        public PointF[] Points { get; }

        public PlacardLocation(float xPos, int index, Settings settings, Position position)
        {
            Points = new PointF[3];

            if (position == Position.Top)
            {
                Points[0] = new PointF(xPos + settings.DogLegWidth + settings.PlacardXSpacing, settings.MidPoint - (settings.ChevronHeight / 2) - settings.TopOffsets[index] - settings.PlacardYSpacing / 2 - 1);
            }
            else
            {
                Points[0] = new PointF(xPos + settings.DogLegWidth + settings.PlacardXSpacing, settings.MidPoint + (settings.ChevronHeight / 2) + settings.BottomOffsets[index] - settings.PlacardYSpacing / 2 + 1);
            }
            Points[1] = new PointF(Points[0].X, Points[0].Y + settings.PlacardYSpacing);
            Points[2] = new PointF(Points[0].X, Points[1].Y + settings.PlacardYSpacing);
        }
    }
}
