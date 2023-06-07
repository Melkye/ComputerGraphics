using ImageConverter;
using RayCasting.Objects;
using RayCasting.Scenes;

namespace RayCasting.Casters;
public class LightNeglectingCaster : ICaster
{
    public Pixel Cast(Scene scene, Vector3D rayDirection)
    {
        Ray3D ray = new(scene.Camera.Position, rayDirection);

        Point3D? intersectionPoint = null;

        foreach (var figure in scene.Figures)
        {
            intersectionPoint = figure.GetIntersectionPoint(ray);
            if (intersectionPoint != null)
                break;
        }

        if (intersectionPoint == null)
            return new Pixel(0, 0, 0);

        return new Pixel(255, 255, 255);
    }
}
