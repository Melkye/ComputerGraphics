using RayCasting.Objects;
using RayCasting.Transformations;

namespace RayCasting.Cameras;
public class Camera : ICamera
{
    public Camera(Point3D position, Vector3D forwardDirection, Vector3D upDirection, float fieldOfView)
    {
        Position = position;
        ForwardDirection = forwardDirection.Normalized();
        FieldOfView = fieldOfView;
        UpDirection = upDirection.Normalized();
    }

    public Point3D Position { get; private set; }

    public Vector3D ForwardDirection { get; private set; }

    public Vector3D UpDirection { get; private set; }

    public Vector3D RightDirection => ForwardDirection.Cross(UpDirection);

    public void Rotate(TransformationMatrix4x4 rotation)
    {
        ForwardDirection = rotation.Multiply(ForwardDirection);
        UpDirection = rotation.Multiply(UpDirection);
    }

    public void Rotate(Axes axis, float angleInDegrees)
    {
        TransformationMatrixBuilder transformationBuilder = new();
        TransformationMatrix4x4 rotation = transformationBuilder.Rotate(axis, angleInDegrees);

        ForwardDirection = rotation.Multiply(ForwardDirection);
        UpDirection = rotation.Multiply(UpDirection);
    }

    public void Translate(TransformationMatrix4x4 translation)
    {

        Position = translation.Multiply(Position);

    }

    public void Translate(Axes axis, float shift)
    {
        TransformationMatrixBuilder transformationBuilder = new();
        TransformationMatrix4x4 translation = transformationBuilder.Translate(axis, shift);

        Position = translation.Multiply(Position);
    }

    public float FieldOfView { get; }
}
