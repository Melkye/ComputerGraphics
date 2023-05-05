using RayCasting.Objects;
using RayCasting.Scenes;

namespace RayCasting.Casters;

internal class LightConsideringCaster : ICaster
{
    public byte Cast(Scene scene, (float, float, float) pixelAngles)
    {
        Vector3D rayDirection = new(pixelAngles);

        Ray3D ray = new(scene.Camera.Position, rayDirection);

        Point3D? intersectionPoint = scene.Figure.GetIntersectionPoint(ray);
        if (intersectionPoint == null)
            return 0;

        //HACK remove cast to Point3D
        Vector3D normalVectorAtIntersectPoint = scene.Figure.GetNormalVector((Point3D)intersectionPoint);

        //HACK do smth with normalization
        Vector3D normalizedLightRay = scene.LightSource.Direction.Normalized();

        float brightnessMinusOneToOne = normalVectorAtIntersectPoint.Dot(normalizedLightRay);

        byte brightnessZeroTo255;

        if (brightnessMinusOneToOne <= 0)
        {
            brightnessZeroTo255 = 0;
        }
        else
        {
            brightnessZeroTo255 = (byte)(brightnessMinusOneToOne * 255);
        }

        return brightnessZeroTo255;
    }
}
