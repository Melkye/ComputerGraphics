using RayCasting.Objects;
using RayCasting.Transformations;

namespace RayCasting.Figures;
public interface IIntersectable
{
    Point3D? GetIntersectionPoint(Ray3D ray);

    Vector3D GetNormalVector(Point3D point);

    void Transform(TransformationMatrix4x4 transformation);
}
