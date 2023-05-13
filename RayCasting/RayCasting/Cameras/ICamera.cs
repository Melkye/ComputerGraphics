using RayCasting.Objects;

namespace RayCasting.Cameras;
public interface ICamera
{
    Point3D Position { get; }

    Vector3D Direction { get; }

    Vector3D UpDirection { get; }

    Vector3D RightDirection { get; }

    float FieldOfView { get; }
}
