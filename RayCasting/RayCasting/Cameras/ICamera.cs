using RayCasting.Objects;

namespace RayCasting.Cameras;
public interface ICamera
{
    Point3D Position { get; }

    Vector3D Direction { get; }

    float FieldOfView { get; }
}
