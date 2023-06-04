using ImageConverter;
using RayCasting.Objects;

namespace RayCasting.Lighting;
public class DirectionalLighting : ILighting
{
    public DirectionalLighting(Pixel color, float intensity, Vector3D direction)
    {
        Color = color;
        Intensity = intensity;

        Direction = direction.Normalized();
    }

    public Pixel Color { get; }
    public float Intensity { get; }

    private Vector3D Direction { get; }

    public Vector3D GetDirection(Point3D targetPoint) => Direction;
}
