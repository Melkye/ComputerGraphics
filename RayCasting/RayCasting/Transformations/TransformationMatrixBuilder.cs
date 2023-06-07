using RayCasting.Objects;

namespace RayCasting.Transformations;

public class TransformationMatrixBuilder
{
    //For column vectors, each of these basic vector rotations appears
    //counterclockwise when the axis about which they occur points toward
    //the observer, the coordinate system is right-handed, and the
    //angle θ is positive.Rz, for instance, would rotate toward the y-axis
    //a vector aligned with the x-axis, as can easily be checked by operating
    //with Rz on the vector(1,0,0) : *pic*
    public TransformationMatrix4x4 GetRotationMatrix4x4(Vector3D axisVector, float angleInDegrees)
    {

        float angleInRadians = (float)(angleInDegrees * Math.PI / 180);

        Vector3D axisUnitVector = axisVector.Normalized();

        var x = axisUnitVector.X;
        var y = axisUnitVector.Y;
        var z = axisUnitVector.Z;

        var cos = (float)Math.Cos(angleInRadians);
        if (Math.Abs(cos) < 10e-6)
            cos = 0;

        var sin = (float)Math.Sin(angleInRadians);
        if (Math.Abs(sin) < 10e-6)
            sin = 0;

        //https://en.wikipedia.org/wiki/Rotation_matrix
        TransformationMatrix4x4 matrix = new(
        new float [,]
        {
            { cos + x*x*(1-cos)  , x*y*(1-cos)-z*sin  , x*z*(1-cos)+y*sin, 0 },
            { y*x*(1-cos) + z*sin, cos+y*y*(1-cos)    , y*z*(1-cos)-x*sin, 0 },
            { z*x*(1-cos) - y*sin, z*y*(1-cos) + x*sin, cos + z*z*(1-cos), 0 },
            {0, 0, 0, 1 }
        });

        return matrix;
    }

    public TransformationMatrix4x4 GetTranslationMatrix4x4(float shiftX, float shiftY, float shiftZ)
    {
        TransformationMatrix4x4 matrix = new(
        new float[,]
        {
            {1, 0, 0, shiftX },
            {0, 1, 0, shiftY },
            {0, 0, 1, shiftZ },
            {0, 0, 0, 1 }
        });

        return matrix;
    }

    public TransformationMatrix4x4 GetScaleMatrix4x4(float scaleX, float scaleY, float scaleZ)
    {
        TransformationMatrix4x4 matrix = new(
        new float[,]
        {
            {scaleX, 0, 0, 0 },
            {0, scaleY, 0, 0 },
            {0, 0, scaleZ, 0 },
            {0, 0, 0, 1 }
        });

        return matrix;
    }
}
