using RayCasting.Objects;

namespace RayCasting.Lighting;

// UNDONE: make it not one directional etc
public interface ILightSource
{
    public Point3D Origin { get; }

    public Vector3D Direction { get; }
}
