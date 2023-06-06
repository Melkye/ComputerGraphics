using System;
using RayCasting.Objects;
using RayCasting.Transformations;

namespace RayCasting.Figures;


// used references: 
//  https://tavianator.com/2011/ray_box.html

public class Cuboid : IIntersectable
{
    public Cuboid(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
    {
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;
        this.minZ = minZ;
        this.maxZ = maxZ;
    }
    float minX { get; }
    float maxX { get; }
    float minY { get; }
    float maxY { get; }
    float minZ { get; }
    float maxZ { get; }

    public IIntersectable[]? figuresInside { get; set; }

    // return point (1, 1, 1) because 
    public Point3D? GetIntersectionPoint(Ray3D ray)
    {
        float? distance;
        if (GetIntersectionDistance(ray, out distance))
        {
            return ray.Origin + ray.Direction * distance;
        }
        return null;
    }

    public bool GetIntersectionDistance(Ray3D ray, out float? distance)
    {
        float tmin = float.NegativeInfinity;
        float tmax = float.PositiveInfinity;

        float rnx = ray.Direction.X;
        float rny = ray.Direction.Y;
        float rnz = ray.Direction.Z;

        if (rnx != 0)
        {
            float tx1 = (minX - ray.Origin.X) / rnx;
            float tx2 = (maxX - ray.Origin.X) / rnx;

            tmin = Math.Max(tmin, Math.Min(tx1, tx2));
            tmax = Math.Min(tmax, Math.Max(tx1, tx2));
        }
        if (rny != 0)
        {
            float ty1 = (minY - ray.Origin.Y) / rny;
            float ty2 = (maxY - ray.Origin.Y) / rny;

            tmin = Math.Max(tmin, Math.Min(ty1, ty2));
            tmax = Math.Min(tmax, Math.Max(ty1, ty2));
        }
        if (rnz != 0)
        {
            float tz1 = (minZ - ray.Origin.Z) / rnz;
            float tz2 = (maxZ - ray.Origin.Z) / rnz;

            tmin = Math.Max(tmin, Math.Min(tz1, tz2));
            tmax = Math.Min(tmax, Math.Max(tz1, tz2));
        }

        if (tmax >= tmin)
            distance = tmin;
        else
            distance = null;

        return tmax >= tmin;
    }

    public Vector3D GetNormalVector(Point3D point)
    {
        throw new NotImplementedException();
    }

    public void Transform(TransformationMatrix4x4 transformation)
    {
        throw new NotImplementedException();
    }

    public IIntersectable[]? GetFiguresInside()
    {
        return figuresInside;
    }

    public Point3D GetCentralPoint()
    {
        //TODO: return central point between eight points
        throw new NotImplementedException();
    }

    public List<Point3D> GetDiscretePoints()
    {
        //TODO: return eight points from every edge
        throw new NotImplementedException();
    }
}
