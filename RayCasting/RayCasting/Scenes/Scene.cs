using RayCasting.Figures;
using RayCasting.Lighting;
using RayCasting.Cameras;

namespace RayCasting.Scenes;
internal class Scene
{
    public Scene(ICamera camera, ILightSource lightSource, IScreen screen, IIntersectable figure)
    {
        Camera = camera;
        LightSource = lightSource;
        Screen = screen;
        Figure = figure;
    }
    public ICamera Camera { get; }

    public ILightSource LightSource { get; } // TODO: make several sources

    public IIntersectable Figure { get; } // TODO: make several figures

    public IScreen Screen { get; }
}
