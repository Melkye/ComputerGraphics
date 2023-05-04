using RayCasting.Objects;

namespace RayCasting.Figures;
internal interface IIntersectable
{
    Point3D? GetIntersectionPoint(Ray3D ray);
}
