using ImageConverter;
using RayCasting.Figures;
using RayCasting.Lighting;
using RayCasting.Objects;
using RayCasting.Scenes;
using RayCasting.Transformations;

namespace RayCasting.Casters;

public class ColorConsideringCaster : ICaster
{
    public Pixel Cast(Scene scene, Vector3D rayDirection)
    {
        Ray3D camRay = new(scene.Camera.Position, rayDirection);

        (Point3D? closestIntersectionPoint, Vector3D? normalVectorAtIntersectionPoint) = 
            GetClosestIntersectionPointAndNormalVector(camRay, scene.Figures);

        if (closestIntersectionPoint is null)
            return new Pixel(0, 0, 0);

        Pixel color = GetColorAtPoint(scene.Lightings, scene.Figures, (Point3D)closestIntersectionPoint, (Vector3D)normalVectorAtIntersectionPoint);

        return color;
    }

    private (Point3D? closestIntersectionPoint, Vector3D? normalVectorAtIntersectionPoint) 
        GetClosestIntersectionPointAndNormalVector(Ray3D camRay, IIntersectable[] figures)
    {
        Point3D? closestIntersectionPoint = null;
        float distance = float.MaxValue;
        int closestFigureIndex = -1;

        for (int i = 0; i < figures.Length; i++)
        {
            Point3D? figureIntersectionPoint = figures[i].GetIntersectionPoint(camRay);
            if (figureIntersectionPoint == null)
                continue;

            float distanceToFigure = camRay.Origin.GetDistance((Point3D)figureIntersectionPoint);
            if (distanceToFigure >= distance)
                continue;

            closestIntersectionPoint = figureIntersectionPoint;
            distance = distanceToFigure;
            closestFigureIndex = i;
        }

        Vector3D? normalVectorAtIntersectionPoint = null;

        if (closestIntersectionPoint is not null)
            normalVectorAtIntersectionPoint = figures[closestFigureIndex].GetNormalVector((Point3D)closestIntersectionPoint);

        return (closestIntersectionPoint, normalVectorAtIntersectionPoint);
    }

    private Pixel GetColorAtPoint(ILighting[] lightings, IIntersectable[] figures, Point3D point, Vector3D normalVector)
    {
        // TODO learn what to do when two lightings collide
        // currently they are added (value += (color * intensity) * brightness)
        Pixel color = new();

        foreach (ILighting lighting in lightings)
        {
            float brightnessByCurrentLighting = CalculateBrightness(lighting, figures, point, normalVector);
            if (brightnessByCurrentLighting == 0)
                continue;

            byte redByCurrentLighting = (byte)(lighting.Color.Red * lighting.Intensity * brightnessByCurrentLighting);
            byte greenByCurrentLighting = (byte)(lighting.Color.Green * lighting.Intensity * brightnessByCurrentLighting);
            byte blueByCurrentLighting = (byte)(lighting.Color.Blue * lighting.Intensity * brightnessByCurrentLighting);

            color = new(
                (byte)(color.Red + redByCurrentLighting),
                (byte)(color.Green + greenByCurrentLighting),
                (byte)(color.Blue + blueByCurrentLighting));
        }

        return color;
    }

    private bool IsPointShaded(IIntersectable[] figures, Point3D point, Vector3D vectorFromPointToLight)
    {
        Ray3D rayFromPointToLight = new(point, vectorFromPointToLight);

        foreach (IIntersectable figure in figures)
        {

            Point3D? figureIntersectionPoint = figure.GetIntersectionPoint(rayFromPointToLight);
            if (figureIntersectionPoint is null)
                continue;

            if (figureIntersectionPoint.Equals(point))
                continue;

            if (figureIntersectionPoint is not null)
                return true;
        }

        return false;
    }

    private float CalculateBrightness(ILighting lighting, IIntersectable[] figures, Point3D point, Vector3D normalVector)
    {
        float totalBrightnessByLighting = 0;
        var directions = lighting.GetDirections(point);

        foreach (var direction in directions)
        {
            float brightnessByLightingMinusOneToOne = -normalVector.Dot(direction);
            if (brightnessByLightingMinusOneToOne <= 0f)
                continue;

            float brightnessByLightingDir0To1 = brightnessByLightingMinusOneToOne;

            Vector3D vectorFromPointToLight = -direction;
            if (IsPointShaded(figures, point, vectorFromPointToLight))
                brightnessByLightingDir0To1 *= 0.3f;

            totalBrightnessByLighting += brightnessByLightingDir0To1;
        }

        var totalBrightnessByLightingNormalized = totalBrightnessByLighting / directions.Count();

        return totalBrightnessByLightingNormalized;
    }
}
