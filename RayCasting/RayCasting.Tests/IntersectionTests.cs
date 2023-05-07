using RayCasting.Cameras;
using RayCasting.Casters;
using RayCasting.Figures;
using RayCasting.Lighting;
using RayCasting.Objects;
using RayCasting.Scenes;

namespace RayCasting.Tests;

public class IntersectionTests
{
    [Test, Category("SphereTests")]
    public void RayInsideSphere_GetIntersectionPoint_ReturnsNotNull()
    {
        // Arrange
        Point3D coordOrigin = new(0, 0, 0);

        Sphere sphere = new(coordOrigin, 1f);

        Ray3D ray = new(coordOrigin, new(0, 0, -1));

        // Act
        Point3D? intersectionPoint = sphere.GetIntersectionPoint(ray);

        // Assert
        Assert.That(intersectionPoint, Is.Not.Null);
    }

    [Test, Category("SphereTests")]
    public void RayDirOppositeToSpherePos_GetIntersectionPoint_ReturnsNull()
    {
        // Arrange
        Point3D coordOrigin = new(0, 0, 0);

        Sphere sphere = new(new(0, 0, -10), 1f);

        Ray3D ray = new(coordOrigin, new(0, 0, 1));

        // Act
        Point3D? intersectionPoint = sphere.GetIntersectionPoint(ray);

        // Assert
        Assert.That(intersectionPoint, Is.Null);
    }

    [Test, Category("PlaneTests")]
    public void PlaneParallelToRay_ReturnsNull()
    {
        // Arrange
        Point3D coordOrigin = new(0, 0, 0);

        Plane plane = new(coordOrigin, new(0, 1, 0));

        Ray3D ray = new(coordOrigin, new(0, 0, -1));

        // Act
        Point3D? intersectionPoint = plane.GetIntersectionPoint(ray);

        // Assert
        Assert.That(intersectionPoint, Is.Null);
    }

    [Test, Category("PlaneTests")]
    public void PlaneOnRayDir_ReturnsNotNull()
    {
        // Arrange
        Point3D coordOrigin = new(0, 0, 0);

        Plane plane = new(coordOrigin, new(0, 1, 1));

        Ray3D ray = new(coordOrigin, new(0, 0, -1));

        // Act
        Point3D? intersectionPoint = plane.GetIntersectionPoint(ray);

        // Assert
        Assert.That(intersectionPoint, Is.Not.Null);
    }

    [Test, Category("PlaneTests")]
    public void PlaneNotOnRayDir_ReturnsNull()
    {
        // Arrange
        Point3D coordOrigin = new(0, 0, 0);

        Plane plane = new(coordOrigin, new(0, 1, 1));

        Ray3D ray = new(new(1, 1, 1), new(2, 2, 2));

        // Act
        Point3D? intersectionPoint = plane.GetIntersectionPoint(ray);

        // Assert
        Assert.That(intersectionPoint, Is.Null);
    }

    [Test, Category("DiskTests")]
    public void DiskOnRayDirInsideRadius_ReturnsNotNull()
    {
        // Arrange
        Point3D coordOrigin = new(0, 0, 0);

        Disk disk = new(coordOrigin, 1, new(0, 1, 1));

        Ray3D ray = new(coordOrigin, new(0, 0, 1));

        // Act
        Point3D? intersectionPoint = disk.GetIntersectionPoint(ray);

        // Assert
        Assert.That(intersectionPoint, Is.Not.Null);
    }
}