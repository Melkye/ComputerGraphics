using RayCasting.Objects;

namespace RayCasting.Cameras;
public class Camera : ICamera
{
    public Camera(Point3D position, Vector3D direction, Vector3D upDirection, float fieldOfView)
    {
        Position = position;
        Direction = direction.Normalized();
        FieldOfView = fieldOfView;
        UpDirection = upDirection.Normalized();
    }

    public Point3D Position { get; }

    public Vector3D Direction { get; }

    public Vector3D UpDirection { get; }

    public Vector3D RightDirection => Direction.Cross(UpDirection);

    public float FieldOfView { get; }
}
