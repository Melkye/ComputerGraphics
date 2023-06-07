using ImageConverter;
using RayCasting.Objects;

namespace RayCasting.Lighting;

public interface ILighting
{
    // TODO: rename Pixel to Color(?)
    public Pixel Color { get; }

    // has values in interval [0; 1]
    public float Intensity { get; }

    public Vector3D[] GetDirections(Point3D targetPoint);
}
