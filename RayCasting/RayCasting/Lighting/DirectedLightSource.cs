using RayCasting.Objects;

namespace RayCasting.Lighting;
internal class DirectedLightSource : ILightSource// UNDONE: : Ray3D, ILightSource
{
    public DirectedLightSource(Point3D origin, Vector3D direction)
    {
        Origin = origin;
        Direction = direction;
    }
    public Point3D Origin { get; }
    public Vector3D Direction { get; }
}
