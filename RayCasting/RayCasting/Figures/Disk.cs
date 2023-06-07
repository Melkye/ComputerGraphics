using RayCasting.Objects;
using RayCasting.Transformations;
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

    public void Transform(TransformationMatrix4x4 transformation)
    {
        // TODO: implement
        throw new NotImplementedException();
    }

    public IIntersectable[]? GetFiguresInside()
    {
        // TODO: if needed, change self-returning logic to smth else
        return new IIntersectable[] { this };
    }

    public Point3D GetCentralPoint()
    {
        return Center;
    }

    public List<Point3D> GetDiscretePoints()
    {
        return new List<Point3D> { Center };
    }
}
