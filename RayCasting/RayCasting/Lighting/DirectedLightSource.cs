using RayCasting.Objects;

namespace RayCasting.Lighting;
public class DirectedLightSource : ILightSource
{
    public DirectedLightSource(Point3D origin, Vector3D direction)
    {
        Origin = origin;
        Direction = direction;
    }
    public Point3D Origin { get; }
    public Vector3D Direction { get; }
}
