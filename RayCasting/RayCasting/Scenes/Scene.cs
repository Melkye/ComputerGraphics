using RayCasting.Figures;
using RayCasting.Lighting;
using RayCasting.Cameras;

namespace RayCasting.Scenes;
public class Scene
{
    public Scene(ICamera camera, ILightSource lightSource, IIntersectable[] figures)
    {
        Camera = camera;
        LightSource = lightSource;
        Figures = figures;
    }
    public ICamera Camera { get; }

    // TODO remove public setter 
    public ILightSource LightSource { get; set; }

    public IIntersectable[] Figures { get; }
}
