using RayCasting.Cameras;

namespace RayCasting.Scenes;

public interface IScreen
{
    ICamera Camera { get; }

    int Height { get; }
    //Point3D[,] Image { get; }// TODO: need this?
    int Width { get; }
}