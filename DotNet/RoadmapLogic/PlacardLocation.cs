using SixLabors.ImageSharp;

namespace RoadmapLogic
{
    public sealed class PlacardLocation
    {
        public PointF[] Points { get; }

        public PlacardLocation(float xPos, int index, Settings settings, Position position, bool useProvidedXPosition = false)
        {
            Points = new PointF[3];

            var xPosition = useProvidedXPosition ? xPos - settings.PlacardXSpacing : xPos + settings.DogLegWidth + settings.PlacardXSpacing;

            if (position == Position.Top)
            {
                Points[0] = new PointF(xPosition, settings.MidPoint - (settings.ChevronHeight / 2) - settings.TopOffsets[index] - settings.PlacardYSpacing / 2 - 1);
            }
            else
            {
                Points[0] = new PointF(xPosition, settings.MidPoint + (settings.ChevronHeight / 2) + settings.BottomOffsets[index] - settings.PlacardYSpacing / 2 + 1);
            }

            Points[1] = new PointF(Points[0].X, Points[0].Y + settings.PlacardYSpacing);
            Points[2] = new PointF(Points[0].X, Points[1].Y + settings.PlacardYSpacing);
        }
    }
}
