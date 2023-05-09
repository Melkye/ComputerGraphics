using RayCasting.Cameras;
using RayCasting.Casters;
using RayCasting.Figures;
using RayCasting.Lighting;
using RayCasting.Objects;
using RayCasting.Scenes;

namespace RayCasting.Tests;

public class CastingTests
{
    [Test]
    public void EmptyScene_Cast_Returns0()
    {
        // Arrange
        Point3D coordOrigin = new(0, 0, 0);

        Camera camera = new(coordOrigin, new(0, 0, -1), 60);
        DirectedLightSource lightSource = new(new(0, 1, 0), new(0, -1, 0));
        IIntersectable[] emptyFigureList = Array.Empty<IIntersectable>();

        Scene scene = new(camera, lightSource, emptyFigureList);

        LightConsideringCaster caster = new();

        (float, float, float) pixelAngles = camera.Direction.GetAngles();

        // Act
        byte brightness = caster.Cast(scene, pixelAngles);

        // Assert
        Assert.That(brightness, Is.EqualTo(0));
    }

    [Test]
    public void LightPerpendicularToSphere_Cast_Returns255()
    {
        // Arrange
        Point3D coordOrigin = new(0, 0, 0);

        Camera camera = new(coordOrigin, new(0, 0, -1), 60);
        DirectedLightSource lightSource = new(new(0, 0, 0), new(0, 0, -1));
        IIntersectable[] figures = new IIntersectable[]
        {
            new Sphere(new(0, 0, -3), 1)
        };

        Scene scene = new(camera, lightSource, figures);

        LightConsideringCaster caster = new();

        (float, float, float) pixelAngles = camera.Direction.GetAngles();

        // Act
        byte brightness = caster.Cast(scene, pixelAngles);

        // Assert
        Assert.That(brightness, Is.EqualTo(255));
    }
}
