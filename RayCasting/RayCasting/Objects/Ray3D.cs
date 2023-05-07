namespace RayCasting.Objects;
public readonly struct Ray3D
{
    public Ray3D(Point3D origin, Vector3D direction)
    {
        Origin = origin;
        Direction = direction.Normalized();
    }

    public readonly Point3D Origin { get; }

    public readonly Vector3D Direction { get; }
}
