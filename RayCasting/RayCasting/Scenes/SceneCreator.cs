using RayCasting.Cameras;
using RayCasting.Figures;
using RayCasting.Lighting;
using RayCasting.Transformations;

namespace RayCasting.Scenes;

public class SceneCreator
{
    public Scene Create9Spheres()
    {
        Camera camera = new(new(0, 0, 0), new(0, 0, -1), new(1, 0, 0), 60);
        ILighting lighting = new DirectionalLighting(new(255, 255, 255), 1, new(0, -1, 0));
        IIntersectable[] figures = new IIntersectable[]
        {
            new Sphere(new(-1.5f, 1.5f, -5), 0.5f),
            new Sphere(new(0, 1.5f, -5), 0.5f),
            new Sphere(new(1.5f, 1.5f, -5), 0.5f),
            new Sphere(new(-1.5f, 0, -5), 0.5f),
            new Sphere(new(0, 0, -5), 0.5f),
            new Sphere(new(1.5f, 0, -5), 0.5f),
            new Sphere(new(-1.5f, -1.5f, -5), 0.5f),
            new Sphere(new(0, -1.5f, -5), 0.5f),
            new Sphere(new(1.5f, -1.5f, -5), 0.5f),
        };

        //Scene scene = new(name, camera, new ILighting[] { lighting }, figures);
        Scene scene = new("9spheres", camera, new ILighting[] { lighting }, figures);

        return scene;
    }

    public Scene TriangleHell(string name, Camera camera, ILighting[] lightings)
    {
        //Camera camera = new(new(0, 0, 0), new(0, 0, -1), new(1, 0, 0), 90);
        //ILightSource downsideLight = new DirectedLightSource(new(0, 1, 0), new(0, -1, 0));

        float coordZ = -10;
        Scene triangleHell = new(
            "triangleHell",
            camera,
            lightings,
            new IIntersectable[]
            {
                new Triangle(new(-5f, 4f, coordZ), new(-4f, 6f, coordZ), new(-3f,4f, coordZ)),
                new Triangle(new(-3f, 4f, coordZ), new(-2f, 6f, coordZ), new(-1f, 4f, coordZ)),
                new Triangle(new(-1f, 4f, coordZ), new(0f, 6f, coordZ), new(1f, 4f, coordZ)),
                new Triangle(new(1f, 4f, coordZ), new(2f, 6f, coordZ), new(3f, 4f, coordZ)),
                new Triangle(new(3f, 4f, coordZ), new(4f, 6f, coordZ), new(5f, 4f, coordZ)),

                new Triangle(new(-5f, 2f, coordZ), new(-4f, 4f, coordZ), new(-3f, 2f, coordZ)),
                new Triangle(new(-3f, 2f, coordZ), new(-2f, 4f, coordZ), new(-1f, 2f, coordZ)),
                new Triangle(new(-1f, 2f, coordZ), new(0f, 4f, coordZ), new(1f, 2f, coordZ)),
                new Triangle(new(1f, 2f, coordZ), new(2f, 4f, coordZ), new(3f, 2f, coordZ)),
                new Triangle(new(3f, 2f, coordZ), new(4f, 4f, coordZ), new(5f, 2f, coordZ)),

                new Triangle(new(-5f, 0f, coordZ), new(-4f, 2f, coordZ), new(-3f, 0f, coordZ)),
                new Triangle(new(-3f, 0f, coordZ), new(-2f, 2f, coordZ), new(-1f, 0f, coordZ)),
                new Triangle(new(-1f, 0f, coordZ), new(0f, 2f, coordZ), new(1f, 0f, coordZ)),
                new Triangle(new(1f, 0f, coordZ), new(2f, 2f, coordZ), new(3f, 0f, coordZ)),
                new Triangle(new(3f, 0f, coordZ), new(4f, 2f, coordZ), new(5f, 0f, coordZ)),

                new Triangle(new(-5f, -2f, coordZ), new(-4f, 0f, coordZ), new(-3f, -2f, coordZ)),
                new Triangle(new(-3f, -2f, coordZ), new(-2f, 0f, coordZ), new(-1f, -2f, coordZ)),
                new Triangle(new(-1f, -2f, coordZ), new(0f, 0f, coordZ), new(1f, -2f, coordZ)),
                new Triangle(new(1f, -2f, coordZ), new(2f, 0f, coordZ), new(3f, -2f, coordZ)),
                new Triangle(new(3f, -2f, coordZ), new(4f, 0f, coordZ), new(5f, -2f, coordZ)),

                new Triangle(new(-5f, -4f, coordZ), new(-4f, -2f, coordZ), new(-3f, -4f, coordZ)),
                new Triangle(new(-3f, -4f, coordZ), new(-2f, -2f, coordZ), new(-1f, -4f, coordZ)),
                new Triangle(new(-1f, -4f, coordZ), new(0f, -2f, coordZ), new(1f, -4f, coordZ)),
                new Triangle(new(1f, -4f, coordZ), new(2f, -2f, coordZ), new(3f, -4f, coordZ)),
                new Triangle(new(3f, -4f, coordZ), new(4f, -2f, coordZ), new(5f, -4f, coordZ)),
            }
            );

        return triangleHell;
    }

    public Scene CowsOnPlane(string cowPath, string f16Path)
    {
        Camera cam1 = new(new(0, 0, 0), new(0, 0, -1), new(0, 1, 0), 90);

        PointLighting pinkLightingOneFiveNegSeven = new(new(255, 105, 180), 1f, new(6, 4, -7));
        PointLighting cyanLightingMinusOneFiveNegSeven = new(new(0, 100, 100), 1f, new(-6, 4, -7));

        var lightings = new ILighting[]
        {
            pinkLightingOneFiveNegSeven,
            cyanLightingMinusOneFiveNegSeven,
        };

        var transformationsBuilder = new TransformationMatrixBuilder();

        // cows
        var cowOnNoseTriangles = new ObjReader().ReadTriangles(cowPath);
        var cowOnRightWingTriangles = new ObjReader().ReadTriangles(cowPath);
        var cowOnLeftWingTriangles = new ObjReader().ReadTriangles(cowPath);

        var cowOnNoseTransform = transformationsBuilder
            .Scale(2f)
            .ThenRotate(Axes.X, -90)
            .ThenRotate(Axes.Y, -180)
            .ThenTranslate(Axes.X, -4.2f)
            .ThenTranslate(Axes.Y, 1.75f)
            .ThenTranslate(Axes.Z, -7.9f)
            .ThenRotate(Axes.Z, 14);

        var cowOnRightWingTransform = transformationsBuilder
            .Scale(2f)
            .ThenRotate(Axes.X, -90)
            .ThenRotate(Axes.Y, -180)
            .ThenTranslate(Axes.X, 1)
            .ThenTranslate(Axes.Y, 0.4f)
            .ThenTranslate(Axes.Z, -9.5f);

        var cowOnLeftWingTransform = transformationsBuilder
            .Scale(2f)
            .ThenRotate(Axes.X, -90)
            .ThenRotate(Axes.Y, -180)
            .ThenTranslate(Axes.X, 1)
            .ThenTranslate(Axes.Y, 0.4f)
            .ThenTranslate(Axes.Z, -6f);

        for (int i = 0; i < cowOnNoseTriangles.Length; i++)
        {
            cowOnNoseTriangles[i].Transform(cowOnNoseTransform);
            cowOnRightWingTriangles[i].Transform(cowOnRightWingTransform);
            cowOnLeftWingTriangles[i].Transform(cowOnLeftWingTransform);
        }

        // f-16
        var f16Triangles = new ObjReader().ReadTriangles(f16Path);

        var f16Transform = transformationsBuilder
            .Rotate(Axes.Y, -90)
            .ThenTranslate(Axes.Z, -6)
            .ThenTranslate(Axes.Y, -4);

        foreach (var triangle in f16Triangles)
        {
            triangle.Transform(f16Transform);
        }

        // concat figures

        var cowsOnPlaneTriangles = f16Triangles
            .Concat(cowOnNoseTriangles)
            .Concat(cowOnLeftWingTriangles)
            .Concat(cowOnRightWingTriangles)
            .ToArray();


        // cam transforms
        cam1.Rotate(Axes.X, -20);
        cam1.Rotate(Axes.Y, -60);

        cam1.Translate(Axes.X, -7f);
        cam1.Translate(Axes.Y, 2f);
        cam1.Translate(Axes.Z, -5f);

        Scene cowsOnPlaneSceneKd = new("cowsOnPlane", cam1, lightings, cowsOnPlaneTriangles);

        return cowsOnPlaneSceneKd;
    }
}
