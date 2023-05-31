
using System.Numerics;
using RayCasting.Objects;

namespace RayCasting.Transformations;

public class TransformationMatrixBuilder
{
    public TransformationMatrix3D GetRotationMatrix(Axes axis, float angle)
    {
        Vector3D axisVector = new();

        switch (axis)
        {
            case Axes.X:
                axisVector = new Vector3D(1, 0, 0);
                break;
            case Axes.Y:
                axisVector = new Vector3D(0, 1, 0);
                break;
            case Axes.Z:
                axisVector = new Vector3D(0, 0, 1);
                break;
        }

        TransformationMatrix3D matrix = GetRotationMatrix(axisVector, angle);

        return matrix;
    }

    //For column vectors, each of these basic vector rotations appears
    //counterclockwise when the axis about which they occur points toward
    //the observer, the coordinate system is right-handed, and the
    //angle θ is positive.Rz, for instance, would rotate toward the y-axis
    //a vector aligned with the x-axis, as can easily be checked by operating
    //with Rz on the vector(1,0,0) : *pic*
    public TransformationMatrix3D GetRotationMatrix(Vector3D axisVector, float angle)
    {
        Vector3D axisUnitVector = axisVector.Normalized();

        TransformationMatrix3D matrix = new(
        new float [,]
        {
            // TODO add math
            {1, 0, 0, 0 },
            {0, 1, 0, 0 },
            {0, 0, 1, 0 },
            {0, 0, 0, 1 }
            //{1, 0, 0, 0 },
            //{0, 1, 0, 0 },
            //{0, 0, 1, 0 },
            //{0, 0, 0, 1 }
        });


        return matrix;
    }
    // TODO position
    // TODO rotation
    // TODO scale

}
