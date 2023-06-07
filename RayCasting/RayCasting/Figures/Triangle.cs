using RayCasting.Objects;
using RayCasting.Transformations;

namespace RayCasting.Figures;

public class Triangle : IIntersectable
{
    public Triangle(Point3D vertex0, Point3D vertex1, Point3D vertex2)
    {
        this.vertex0 = vertex0;
        this.vertex1 = vertex1;
        this.vertex2 = vertex2;
        centralPoint = new Point3D(
           (vertex0.X + vertex1.X + vertex2.X) / 3,
           (vertex0.Y + vertex1.Y + vertex2.Y) / 3,
           (vertex0.Z + vertex1.Z + vertex2.Z) / 3);
    }

    Point3D vertex0;
    Point3D vertex1;
    Point3D vertex2;
    public Point3D centralPoint { get; private set; }

    readonly float _EPSILON = 0.0000001f;


    public Point3D? GetIntersectionPoint(Ray3D ray)
    {
        Vector3D edge1, edge2, h, s, q;
        float a, f, u, v;
        edge1 = new Vector3D(vertex1) - new Vector3D(vertex0);
        edge2 = new Vector3D(vertex2) - new Vector3D(vertex0);
        h = ray.Direction.Cross(edge2);
        a = edge1.Dot(h);

        if (a > -_EPSILON && a < _EPSILON)
            return null;

        f = 1f / a;
        s = new Vector3D(ray.Origin) - new Vector3D(vertex0);
        u = s.Dot(h) * f;

        if (u < 0 || u > 1)
            return null;

        q = s.Cross(edge1);
        v = ray.Direction.Dot(q) * f;

        if (v < 0 || u + v > 1)
            return null;

        float t = edge2.Dot(q) * f;

        if (t > _EPSILON)
            return ray.Origin + ray.Direction * t;
        else
            return null;
    }

    public Vector3D GetNormalVector(Point3D point)
    {
        //TODO: check if normal is pointed towards light
        Vector3D vecAB = new(vertex0, vertex1);
        Vector3D vecBC = new(vertex1, vertex2);

        // if want to see triangle hell, negate result
        // i.e. use -vecAB.Cross(vecBC).Normalized();
        return vecAB.Cross(vecBC).Normalized();
    }

    public void Transform(TransformationMatrix4x4 transformation)
    {
        vertex0 = transformation.Multiply(vertex0);
        vertex1 = transformation.Multiply(vertex1);
        vertex2 = transformation.Multiply(vertex2);
        centralPoint = new Point3D(
            (vertex0.X + vertex1.X + vertex2.X) / 3,
            (vertex0.Y + vertex1.Y + vertex2.Y) / 3,
            (vertex0.Z + vertex1.Z + vertex2.Z) / 3);
    }

    public IIntersectable[]? GetFiguresInside()
    {
        // TODO: if needed, change self-returning logic to smth else
        return new IIntersectable[] { this };
    }

    public Point3D GetCentralPoint()
    {
        return centralPoint;
    }

    public List<Point3D> GetDiscretePoints()
    {
        return new List<Point3D> { vertex0, vertex1, vertex2 };
    }
}