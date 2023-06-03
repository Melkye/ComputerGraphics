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

    //public void Transform(TransformationMatrix4x4 transformation)
    //{

    //}
    public void Rotate(TransformationMatrix4x4 rotation)
    {
        ForwardDirection = rotation.Multiply(ForwardDirection);
        UpDirection = rotation.Multiply(UpDirection);
    }

    public void Rotate(Axes axis, float angleInDegrees)
    {
        TransformationMatrixBuilder transformationBuilder = new();
        // TODO decide how to use this and consider forward/upward direction
        TransformationMatrix4x4 rotation = transformationBuilder.Rotate(axis, angleInDegrees);

        // TODO guess don't need this
        switch (axis)
        {
            case Axes.X:
                // change both forward and up
                break;
            case Axes.Y:
                // change forward
                break;
            case Axes.Z:
                // change up
                break;
        }

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
        // TODO decide how to use this and consider forward/upward direction
        TransformationMatrix4x4 translation = transformationBuilder.Translate(axis, shift);

        // TODO guess don't need this
        switch (axis)
        {
            case Axes.X:
                // possibly need to change change both forward and up
                break;
            case Axes.Y:
                // possibly need to change change both forward and up
                break;
            case Axes.Z:
                // possibly need to change change both forward and up
                break;
        }

        //ForwardDirection = translation.Multiply(ForwardDirection);
        //UpDirection = translation.Multiply(UpDirection);

        Position = translation.Multiply(Position);
    }

    public float FieldOfView { get; }
}
