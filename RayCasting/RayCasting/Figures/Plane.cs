using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayCasting.Objects;

namespace RayCasting.Figures;
public class Plane : IIntersectable
{
    public Plane(Point3D point, Vector3D normalVector)
    {
        Point = point;
        NormalVector = normalVector.Normalized();
    }

    public Plane(Point3D pointA, Point3D pointB, Point3D pointC)
    {
        Point = pointA;

        Vector3D vecAB = new(pointA, pointB);
        Vector3D vecBC = new(pointB, pointC);
        Vector3D normalVec = vecAB.Cross(vecBC).Normalized();

        NormalVector = normalVec;
    }

    // NOTE: consider switching to Ray instead of Point and Vector pair here and everywhere

    public Point3D Point { get; }

    public Vector3D NormalVector { get; }

    public Point3D? GetIntersectionPoint(Ray3D ray)
    {
        float rayNormalVecDotProduct = ray.Direction.Dot(NormalVector);
        if (Math.Abs(rayNormalVecDotProduct) < 10e-6)
        {
            return null;
        }

        // (plainNormalPointRadiusVec - plainAnyPointRadiusVec)
        // = vector that lays in plane and its starting point is intersection point

        //Vector3D rayRadiusVec = new Vector3D(ray.Position.X, ray.Position.Y, ray.Position.Z);
        Vector3D planePointRadiusVec = new(Point.X, Point.Y, Point.Z);
        Vector3D rayPointRadiusVec = new(ray.Origin.X, ray.Origin.Y, ray.Origin.Z);

        float t = (planePointRadiusVec - rayPointRadiusVec).Dot(NormalVector) / rayNormalVecDotProduct;

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
