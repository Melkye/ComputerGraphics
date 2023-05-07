using RayCasting.Objects;

namespace RayCasting.Cameras;
public class Camera : ICamera
{
    public Camera(Point3D position, Vector3D direction, float fieldOfView)
    {
        Position = position;
        Direction = direction.Normalized();
        FieldOfView = fieldOfView;
    }

    public Point3D Position { get; }

    public Vector3D Direction { get; }

    public float FieldOfView { get; }
}
