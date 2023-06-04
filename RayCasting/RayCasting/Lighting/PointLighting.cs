using ImageConverter;
using RayCasting.Objects;

namespace RayCasting.Lighting;
public class PointLighting : ILighting
{
    public PointLighting(Pixel color, float intensity, Point3D position)
    {
        Color = color;
        Intensity = intensity;

        Position = position;
    }

    public Pixel Color { get; }
    public float Intensity { get; }

    private Point3D Position { get; }

    public Vector3D GetDirection(Point3D targetPoint) => new Vector3D(Position, targetPoint).Normalized();
}
