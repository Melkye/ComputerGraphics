using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayCasting.Objects;

namespace RayCasting.Figures;
internal class Plane : IIntersectable
{
    public Plane(Point3D point, Vector3D normalVector)
    {
        Point = point;
        NormalVector = normalVector;
    }

    // NOTE: consider switching to Ray instead of Point and Vector pair here and everywhere

    public Point3D Point { get; }

    public Vector3D NormalVector { get; }

    public Point3D? GetIntersectionPoint(Ray3D ray)
    {
        float rayNormalDotProduct = ray.Direction.Dot(NormalVector);
        if (Math.Abs(rayNormalDotProduct) < 10e-6)
        {
            return null;
        }

        // (plainNormalPointRadiusVec - plainAnyPointRadiusVec)
        // = vector that lays in plane and its starting point is intersection point

        //Vector3D rayRadiusVec = new Vector3D(ray.Position.X, ray.Position.Y, ray.Position.Z);
        Vector3D planePointRadiusVec = new Vector3D(Point.X, Point.Y, Point.Z);
        Vector3D rayPointRadiusVec = new Vector3D(ray.Origin.X, ray.Origin.Y, ray.Origin.Z);

        float t = (rayPointRadiusVec - planePointRadiusVec).Dot(NormalVector) / (ray.Direction.Dot(NormalVector));

        if (t < 0) // TODO: and == 0?
        {
            return null;
        }

        Point3D intersectPoint = ray.Origin + ray.Direction * t;
        return intersectPoint;
    }
    public Vector3D GetNormalVector(Point3D point)
    {
        // TODO: consider ray coming from one or the other side -- should NormalVector be the same?
        return NormalVector;
    }
}
