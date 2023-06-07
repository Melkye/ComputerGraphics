using RayCasting.Objects;

namespace RayCasting.Transformations;

public static class TransformationExtensions
{
    public static TransformationMatrix4x4 Rotate(this TransformationMatrixBuilder builder, Vector3D axisVector, float angleInDegrees)
    {
        var rotation = builder.GetRotationMatrix4x4(axisVector, angleInDegrees);

        return rotation;
    }

    public static TransformationMatrix4x4 Rotate(this TransformationMatrixBuilder builder, Axes axis, float angleInDegrees)
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

        return Rotate(builder, axisVector, angleInDegrees);
    }


    public static TransformationMatrix4x4 Translate(this TransformationMatrixBuilder builder, float shiftX, float shiftY, float shiftZ)
    {
        var translation = builder.GetTranslationMatrix4x4(shiftX, shiftY, shiftZ);

        return translation;
    }

    public static TransformationMatrix4x4 Translate(this TransformationMatrixBuilder builder, Axes axis, float shift)
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
        return Translate(builder, shiftX, shiftY, shiftZ);
    }

    public static TransformationMatrix4x4 Translate(this TransformationMatrixBuilder builder, float shift)
    {
        return Translate(builder, shift, shift, shift);
    }

    public static TransformationMatrix4x4 Scale(this TransformationMatrixBuilder builder, float scaleX, float scaleY, float scaleZ)
    {
        var scaleMatrix = builder.GetScaleMatrix4x4(scaleX, scaleY, scaleZ);

        return scaleMatrix;
    }

    public static TransformationMatrix4x4 Scale(this TransformationMatrixBuilder builder, float scale)
    {
        return Scale(builder, scale, scale, scale);
    }

    public static TransformationMatrix4x4 Scale(this TransformationMatrixBuilder builder, Axes axis, float scale)
    {
        float scaleX = 1;
        float scaleY = 1;
        float scaleZ = 1;

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

        return Scale(builder, scaleX, scaleY, scaleZ);
    }

    public static TransformationMatrix4x4 ThenRotate(this TransformationMatrix4x4 transformation, Vector3D axisVector, float angleInDegrees)
    {
        var rotation = new TransformationMatrixBuilder().Rotate(axisVector, angleInDegrees);

        var rotated = rotation.Multiply(transformation);

        return rotated;
    }

    public static TransformationMatrix4x4 ThenRotate(this TransformationMatrix4x4 transformation, Axes axis, float angleInDegrees)
    {
        var rotation = new TransformationMatrixBuilder().Rotate(axis, angleInDegrees);

        var rotated = rotation.Multiply(transformation);

        return rotated;
    }

    public static TransformationMatrix4x4 ThenTranslate(this TransformationMatrix4x4 transformation, Axes axis, float shift)
    {
        var translation = new TransformationMatrixBuilder().Translate(axis, shift);

        var translated = translation.Multiply(transformation);

        return translated;
    }

    public static TransformationMatrix4x4 ThenTranslate(this TransformationMatrix4x4 transformation, float shiftX, float shiftY, float shiftZ)
    {
        var translation = new TransformationMatrixBuilder().Translate(shiftX, shiftY, shiftZ);

        var translated = translation.Multiply(transformation);

        return translated;
    }

    public static TransformationMatrix4x4 ThenTranslate(this TransformationMatrix4x4 transformation, float shift)
    {
        var translation = new TransformationMatrixBuilder().Translate(shift);

        var translated = translation.Multiply(transformation);

        return translated;
    }

    public static TransformationMatrix4x4 ThenScale(this TransformationMatrix4x4 transformation, float scaleX, float scaleY, float scaleZ)
    {
        var scaleMatrix = new TransformationMatrixBuilder().Scale(scaleX, scaleY, scaleZ);

        var scaled = scaleMatrix.Multiply(transformation);

        return scaled;
    }

    public static TransformationMatrix4x4 ThenScale(this TransformationMatrix4x4 transformation, Axes axis, float scale)
    {
        var scaleMatrix = new TransformationMatrixBuilder().Scale(axis, scale);

        var scaled = scaleMatrix.Multiply(transformation);

        return scaled;
    }

    public static TransformationMatrix4x4 ThenScale(this TransformationMatrix4x4 transformation, float scale)
    {
        var scaleMatrix = new TransformationMatrixBuilder().Scale(scale);

        var scaled = scaleMatrix.Multiply(transformation);

        return scaled;
    }

}
