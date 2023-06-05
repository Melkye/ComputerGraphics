using RayCasting.Cameras;
using RayCasting.Figures;
using RayCasting.Lighting;

namespace RayCasting.Scenes;

public class SceneCreator
{
    public Scene Create9Spheres()
    {
        string name = "";
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

        Scene scene = new(name, camera, new ILighting[] { lighting }, figures);

        return scene;
    }

    public Scene TriangleHell(string name, Camera camera, ILighting[] lightings)
    {
        //Camera camera = new(new(0, 0, 0), new(0, 0, -1), new(1, 0, 0), 90);
        //ILightSource downsideLight = new DirectedLightSource(new(0, 1, 0), new(0, -1, 0));

        float coordZ = -10;

        Scene triangleHell = new(name, camera, lightings,
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
        });

        return triangleHell;
    }
}
