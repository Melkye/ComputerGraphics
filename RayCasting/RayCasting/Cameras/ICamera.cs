using RayCasting.Objects;
using RayCasting.Transformations;

namespace RayCasting.Cameras;
public interface ICamera
{
    Point3D Position { get; }

    Vector3D ForwardDirection { get; }

    Vector3D UpDirection { get; }

    Vector3D RightDirection { get; }

    // TODO: decide if need this
    //void Transform(TransformationMatrix4x4 transformation);
    void Rotate(TransformationMatrix4x4 rotation);

    void Rotate(Axes axis, float angleInDegrees);

    void Translate(TransformationMatrix4x4 move);

    void Translate(Axes axis, float shift);

    float FieldOfView { get; }
}
