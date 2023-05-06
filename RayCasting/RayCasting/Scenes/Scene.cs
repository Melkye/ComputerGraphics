using RayCasting.Figures;
using RayCasting.Lighting;
using RayCasting.Cameras;

namespace RayCasting.Scenes;
internal class Scene
{
    public Scene(ICamera camera, ILightSource lightSource, IScreen screen, IIntersectable[] figures)
    {
        Camera = camera;
        LightSource = lightSource;
        Screen = screen;
        Figures = figures;
    }
    public ICamera Camera { get; }

    public ILightSource LightSource { get; } // TODO: make several sources

    public IIntersectable[] Figures { get; }

    public IScreen Screen { get; }
}
