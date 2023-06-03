using System.Runtime.InteropServices;
using RayCasting.Objects;
using RayCasting.Scenes;

namespace RayCasting.Casters;

public class LightAndShadowConsideringCaster : ICaster
{
    public byte Cast(Scene scene, (float alpha, float beta, float gamma) pixelAngles)
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
            if (distanceToFigure >= distance)
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
        if (brightnessMinusOneToOne <= 0f)
            return 0;


        // shadows

        // TODO fix hack
        Point3D lightOriginForCurrentRay = (Point3D)intersectionPoint - ray.Direction * 100;

        // not sure if it works, need to check and change then
        //Vector3D vectorFromClosestInterPointToLight = -scene.LightSource.Direction;
        //Vector3D vectorFromClosestInterPointToLight = new((Point3D)intersectionPoint, scene.LightSource.Origin);
        Vector3D vectorFromClosestInterPointToLight = new((Point3D)intersectionPoint, lightOriginForCurrentRay);

        Ray3D rayFromClosestInterPointToLight = new((Point3D)intersectionPoint, vectorFromClosestInterPointToLight);
        //for (int i = 0; i < scene.Figures.Length; i++)
        //{
        //    Point3D? figureIntersectionPoint = scene.Figures[i].GetIntersectionPoint(rayFromClosestInterPointToLight);
        //    if (figureIntersectionPoint is not null
        //        && !figureIntersectionPoint.Equals(intersectionPoint))
        //    {
        //        brightnessMinusOneToOne *= 0.5f;
        //        break;

        //    }
        //}

        Point3D? figureIntersectionPointForShadow = null;
        float distanceToPointForShadow = float.MaxValue;


        for (int i = 0; i < scene.Figures.Length; i++)
        {
            Point3D? figureIntersectionPoint = scene.Figures[i].GetIntersectionPoint(rayFromClosestInterPointToLight);
            if (figureIntersectionPoint == null)
                continue;

            if (figureIntersectionPoint.Equals(intersectionPoint))
                continue;


            float distanceToPointFromLightOrigin = lightOriginForCurrentRay.GetDistance((Point3D)figureIntersectionPoint);
            if (distanceToPointFromLightOrigin >= distanceToPointForShadow)
                continue;

            figureIntersectionPointForShadow = figureIntersectionPoint;
            distanceToPointForShadow = distanceToPointFromLightOrigin;
        }

        if (figureIntersectionPointForShadow is not null)
            brightnessMinusOneToOne *= 0.5f;

        byte brightnessZeroTo255 = (byte)(brightnessMinusOneToOne * 255);

        return brightnessZeroTo255;
    }
}
