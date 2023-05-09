using RayCasting.Cameras;
using RayCasting.Figures;
using RayCasting.Lighting;

namespace RayCasting.Scenes;

public class SceneCreator
{
    public Scene Create9Shperes()
    {
        Camera camera = new(new(0, 0, 0), new(0, 0, -1), 60);
        ILightSource lightSource = new DirectedLightSource(new(0, 1, 0), new(0, -1, 0));
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

        Scene scene = new(camera, lightSource, figures);

        return scene;
    }
}
