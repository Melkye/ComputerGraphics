using ImageConverter;
using RayCasting.Objects;

namespace RayCasting.Lighting;

public interface ILighting
{
    // TODO: rename Pixel to Color(?)
    public Pixel Color { get; }

    // has values in interval [0; 1]
    public float Intensity { get; }

    public Vector3D GetDirection(Point3D targetPoint);

    //// TODO consider if need this or GetDirection works fine
    //// [0; 1]
    //public Pixel GetColorAtPoint(Point3D targetPoint);
}
