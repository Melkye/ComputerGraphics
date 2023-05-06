namespace RayCasting.Objects;
internal readonly struct Vector3D
{
    public Vector3D(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3D(Point3D start, Point3D end)
    {
        X = end.X - start.X;
        Y = end.Y - start.Y;
        Z = end.Z - start.Z;
    }

    public Vector3D(Vector3D copyFrom)
    {
        X = copyFrom.X;
        Y = copyFrom.Y;
        Z = copyFrom.Z;
    }

    // TODO: refactor to use this constructor instead of (p.x, p.y, p.z)
    public Vector3D(Point3D pointFrom)
    {
        X = pointFrom.X;
        Y = pointFrom.Y;
        Z = pointFrom.Z;
    }

    // TODO: factor out angles to a separate class
    public Vector3D((float alpha, float beta, float gamma) angles)
    {
        X = (float)Math.Cos(angles.alpha);
        Y = (float)Math.Cos(angles.beta);
        Z = (float)Math.Cos(angles.gamma);
    }

    public readonly float X { get; } 

    public readonly float Y { get; }

    public readonly float Z { get; }

    public float Abs => (float)Math.Sqrt((X * X) +(Y * Y) + (Z * Z));

    public (float, float, float) GetAngles()
    {
        Vector3D normalized = this.Normalized();

        float alpha = (float)Math.Acos(normalized.X);
        float beta = (float)Math.Acos(normalized.Y);
        float gamma = (float)Math.Acos(normalized.Z);

        return (alpha, beta, gamma);
    }

    public float Dot(Vector3D that)
    {
        float product = (X * that.X) + (Y * that.Y) + (Z * that.Z);

        return product;
    }

    public Vector3D Cross(Vector3D that)
    {
        float x = (Y * that.Z) - (Z * that.Y);
        float y = (X * that.X) - (X * that.Z);
        float z = (X * that.Y) - (Y * that.X);

        Vector3D product = new(x, y, z);

        return product;
    }

    public Vector3D Normalized()
    {
        if (X == 0 && Y == 0 && Z == 0)
            return this;

        float x = X / Abs;
        float y = Y / Abs;
        float z = Z / Abs;

        Vector3D normalized = new(x, y, z);

        return normalized;
    }

    public static Vector3D operator -(Vector3D vector)
    {
        Vector3D oppositeVector = new(
            -vector.X,
            -vector.Y,
            -vector.Z);

        return oppositeVector;
    }

    public static Vector3D operator +(Vector3D left, Vector3D right)
    {
        Vector3D resultVector = new(
            left.X + right.X,
            left.Y + right.Y,
            left.Z + right.Z);

        return resultVector;
    }

    public static Vector3D operator -(Vector3D left, Vector3D right)
    {
        Vector3D resultVector = left + (-right);
        return resultVector;
    }

    public static Vector3D operator *(Vector3D vector, float scalar)
    {
        Vector3D resultVector = new(
            vector.X * scalar,
            vector.Y * scalar,
            vector.Z * scalar);

        return resultVector;
    }
}
