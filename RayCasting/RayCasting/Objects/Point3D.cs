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
}
