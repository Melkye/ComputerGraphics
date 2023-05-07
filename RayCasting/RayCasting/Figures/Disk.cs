using RayCasting.Objects;
using System.Numerics;

namespace RayCasting.Figures;
public class Disk : IIntersectable
{
    public Disk(Point3D center, float radius, Vector3D normalVector)
    {
        Center = center;
        Radius = radius;
        NormalVector = normalVector;
    }
    public Point3D Center { get; set; }
    public float Radius { get; set; }
    public Vector3D NormalVector { get; set; }

    public Point3D? GetIntersectionPoint(Ray3D ray)
    {
        Plane plane = new(Center, NormalVector);

        Point3D? intesectPoint = plane.GetIntersectionPoint(ray);

        if (intesectPoint == null)
            return null;

        float distanceFromCenter = Center.GetDistance((Point3D)intesectPoint);
        if (distanceFromCenter > Radius)
            return null;

        return intesectPoint;
    }

    public Vector3D GetNormalVector(Point3D point)
    {
        return NormalVector;
    }
}
