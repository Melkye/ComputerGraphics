using ImageConverter;
using RayCasting.Objects;

namespace RayCasting.Lighting;

// TODO: finish AmbientLighting
public class AmbientLighting : ILighting
{
    public AmbientLighting(Pixel color, float intensity)
    {
        Color = color;
        Intensity = intensity;
    }

    public Pixel Color { get; }
    public float Intensity { get; }


    public Vector3D GetDirection(Point3D targetPoint) => new(targetPoint, targetPoint);
}
