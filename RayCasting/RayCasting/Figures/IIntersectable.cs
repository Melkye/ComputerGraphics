using RayCasting.Objects;

namespace RayCasting.Figures;
public interface IIntersectable
{
    Point3D? GetIntersectionPoint(Ray3D ray);

    // UNDONE: this doesn't check if point lays in surface
    Vector3D GetNormalVector(Point3D point);
}
