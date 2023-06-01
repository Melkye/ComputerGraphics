using RayCasting.Objects;

namespace RayCasting.Transformations;

public class TransformationMatrixBuilder
{
    public TransformationMatrix4x4 GetRotationMatrix4x4(Axes axis, float angle)
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

        TransformationMatrix4x4 matrix = GetRotationMatrix4x4(axisVector, angle);

        return matrix;
    }

    //For column vectors, each of these basic vector rotations appears
    //counterclockwise when the axis about which they occur points toward
    //the observer, the coordinate system is right-handed, and the
    //angle θ is positive.Rz, for instance, would rotate toward the y-axis
    //a vector aligned with the x-axis, as can easily be checked by operating
    //with Rz on the vector(1,0,0) : *pic*
    public TransformationMatrix4x4 GetRotationMatrix4x4(Vector3D axisVector, float angle)
    {
        Vector3D axisUnitVector = axisVector.Normalized();

        var x = axisUnitVector.X;
        var y = axisUnitVector.Y;
        var z = axisUnitVector.Z;

        var cos = (float)Math.Cos(angle);
        var sin = (float)Math.Sin(angle);

        //https://en.wikipedia.org/wiki/Rotation_matrix
        TransformationMatrix4x4 matrix = new(
        new float [,]
        {
            { cos + x*x*(1-cos)  , x*y*(1-cos)-z*sin  , x*z*(1-cos)+y+sin, 0 },
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

    public TransformationMatrix4x4 GetTranslationMatrix4x4(float shift)
    {
        return GetTranslationMatrix4x4(shift, shift, shift);
    }

    public TransformationMatrix4x4 GetTranslationMatrix4x4(Axes axis, float shift)
    {
        float shiftX = 0;
        float shiftY = 0;
        float shiftZ = 0;

        switch (axis)
        {
            case Axes.X:
                shiftX = shift;
                break;
            case Axes.Y:
                shiftY = shift;
                break;
            case Axes.Z:
                shiftZ = shift;
                break;
        }

        return GetTranslationMatrix4x4(shiftX, shiftY, shiftZ);
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

    public TransformationMatrix4x4 GetScaleMatrix4x4(float scale)
    {
        return TranslationMatrix4x4(scale, scale, scale);
    }

    public TransformationMatrix4x4 GetScaleMatrix4x4(Axes axis, float scale)
    {
        float scaleX = 0;
        float scaleY = 0;
        float scaleZ = 0;

        switch (axis)
        {
            case Axes.X:
                scaleX = scale;
                break;
            case Axes.Y:
                scaleY = scale;
                break;
            case Axes.Z:
                scaleZ = scale;
                break;
        }

        return TranslationMatrix4x4(scaleX, scaleY, scaleZ);
    }
}
