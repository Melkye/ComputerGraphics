using RayCasting.Objects;

namespace RayCasting.Figures;
internal class Sphere : IIntersectable
{
    public Sphere(Point3D center, float radius)
    {
        Center = center;
        Radius = radius;
    }
    public Point3D Center { get; }
    public float Radius { get; }
    public Point3D? GetIntersectionPoint(Ray3D ray) // TODO bool?
    {
        // ray.dir^2 * t^2 + 2*ray.dir*(ray.origin - center) * t + (ray.origin - center)^2 - r^2 = 0
        
        // a = ray.dir^2
        // b = 2*ray.dir*(ray.origin - center)
        // c = (ray.origin - center)^2 - r^2
        // centerToOrigin = ray.origin - center = vec(center, origin)

        Vector3D centerToOrigin = new(Center, ray.Origin);
        float a = ray.Direction.Dot(ray.Direction);
        float b = 2 * ray.Direction.Dot(centerToOrigin);
        float c = centerToOrigin.Dot(centerToOrigin) - (Radius * Radius);

        float D = b * b - 4 * a * c;

        if (D < 0)
        {
            return null;        
        }

        float closestT = (- b - (float)Math.Sqrt(D)) / (2 * a);

        return ray.Origin + (ray.Direction * closestT);
    }

    // NOTE: need to normalize?
    public Vector3D GetNormalVector(Point3D point)
    {
        Vector3D normalVector = new Vector3D(Center, point).Normalized();

        return normalVector;
    }
}
