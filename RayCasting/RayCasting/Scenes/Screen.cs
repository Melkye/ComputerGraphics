using System.Drawing;
using RayCasting.Objects;
using RayCasting.Cameras;

namespace RayCasting.Scenes;
internal class Screen : IScreen
{
    public Screen(ICamera camera, int height, int width)
    {
        Camera = camera;
        Height = height;
        Width = width;
    }

    public ICamera Camera { get; }

    public int Height { get; }

    public int Width { get; }

    //public Point3D LeftBottomCorner
    //{
    //    get 
    //    {
    //        float halfFov = Camera.FieldOfView / 2;

    //        // TODO: add aspectRatio etc to differ width and height
    //        float halfWidthInSpace = (float)Math.Tan(Math.PI * halfFov / 180) * Camera.;


    //    }
    //}
}
