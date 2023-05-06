using System.Runtime.InteropServices;
using RayCasting.Objects;
using RayCasting.Scenes;

namespace RayCasting.Casters;

internal class LightConsideringCaster : ICaster
{
    public byte Cast(Scene scene, (float, float, float) pixelAngles)
    {
        Vector3D rayDirection = new(pixelAngles);

        Ray3D ray = new(scene.Camera.Position, rayDirection);


        Point3D? intersectionPoint = null;
        float distance = float.MaxValue;
        int closestFigureIndex = 0;

        for (int i = 0; i < scene.Figures.Length; i++)
        {
            Point3D? figureIntersectionPoint = scene.Figures[i].GetIntersectionPoint(ray);
            if (figureIntersectionPoint == null)
                continue;

            float distanceToFigure = scene.Camera.Position.GetDistance((Point3D)figureIntersectionPoint);
            if (distanceToFigure >= distance) // UNDONE: =? compare brighness?
                continue;

            intersectionPoint = figureIntersectionPoint;
            distance = distanceToFigure;
            closestFigureIndex = i;
        }

        if (intersectionPoint == null)
            return 0;

        //HACK remove cast to Point3D
        Vector3D normalVectorAtIntersectPoint = scene.Figures[closestFigureIndex].GetNormalVector((Point3D)intersectionPoint);

        //HACK do smth with normalization
        Vector3D normalizedLightRay = scene.LightSource.Direction.Normalized();

        // negating because need to calculate based on smaller angle between vecs
        // but due to spacical configuration it will use larger one

        float brightnessMinusOneToOne = -normalVectorAtIntersectPoint.Dot(normalizedLightRay);

        byte brightnessZeroTo255 = Math.Max((byte)0, (byte)(brightnessMinusOneToOne * 255));

        return brightnessZeroTo255;
    }
}
