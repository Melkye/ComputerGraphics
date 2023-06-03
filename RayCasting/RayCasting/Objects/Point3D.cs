using System.Diagnostics.CodeAnalysis;

namespace RayCasting.Objects;
public readonly struct Point3D
{
    public Point3D(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public readonly float X { get; }

    public readonly float Y { get; }

    public readonly float Z { get; }

    public float GetDistance(Point3D that)
    {
        float diffX = X - that.X;
        float diffY = Y - that.Y;
        float diffZ = Z - that.Z;

        float distance = (float)Math.Sqrt((diffX * diffX) + (diffY * diffY) + (diffZ * diffZ));

        return distance;
    }

    public static Point3D operator +(Point3D point, Vector3D vector)
    {
        Point3D movedPoint = new(
            point.X + vector.X,
            point.Y + vector.Y,
            point.Z + vector.Z);

        return movedPoint;
    }

    public static Point3D operator -(Point3D point, Vector3D vector)
    {
        Point3D movedPoint = point + (-vector);

        return movedPoint;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Point3D that)
            return false;

        float precision = 10e-6f;

        var diffX = Math.Abs(X - that.X);
        var diffY = Math.Abs(Y - that.Y);
        var diffZ = Math.Abs(Z - that.Z);

        return diffX < precision
            && diffY < precision
            && diffZ < precision;
    }
}
