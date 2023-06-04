using RayCasting.Figures;
using RayCasting.Lighting;
using RayCasting.Cameras;

namespace RayCasting.Scenes;
public class Scene
{
    public Scene(ICamera camera, ILighting[] lightings, IIntersectable[] figures)
    {
        Camera = camera;
        Lightings = lightings;
        Figures = figures;
    }
    public ICamera Camera { get; }

    // TODO remove public setter 
    public ILighting[] Lightings { get; set; }

    public IIntersectable[] Figures { get; }
}
