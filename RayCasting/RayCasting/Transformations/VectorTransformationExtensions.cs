using RayCasting.Objects;

namespace RayCasting.Transformations;

public class VectorTransformationExtensions
{
    public Vector3D Rotate(this Vector3D vector, Axes axis, float angle)
    {
        var matrix = TransformationMatrix.GetRotationMatrix();

        Vector3D rotatedVector = new(vector.X * scale)
        return rotatedVector;
    }
}
