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

        //HACK remove cast to Point3D
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
            // TODO consider removing check for AmbientLighting
            if (lighting is AmbientLighting)
            {
                Pixel colorByAmbientLighting = GetAmbient(figures, lighting.Color, point, normalVector, lighting.Intensity);
                byte redByAmbientLighting = (byte)(colorByAmbientLighting.Red * lighting.Intensity);
                byte greenByAmbientLighting = (byte)(colorByAmbientLighting.Green * lighting.Intensity);
                byte blueByAmbientLighting = (byte)(colorByAmbientLighting.Blue * lighting.Intensity);

                color = new(
                    (byte)(color.Red + redByAmbientLighting),
                    (byte)(color.Green + greenByAmbientLighting),
                    (byte)(color.Blue + blueByAmbientLighting));
                continue;
            }

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
        float brightnessByLightingMinusOneToOne = -normalVector.Dot(lighting.GetDirection(point));
        if (brightnessByLightingMinusOneToOne <= 0f)
            return 0;

        float brightnessByLighting0To1 = brightnessByLightingMinusOneToOne;

        Vector3D vectorFromPointToLight = -lighting.GetDirection(point);
        if (IsPointShaded(figures, point, vectorFromPointToLight))
            brightnessByLighting0To1 *= 0.5f;

        return brightnessByLighting0To1;
    }
    private Pixel GetAmbient(IIntersectable[] figures, Pixel color, Point3D point, Vector3D normalVector, float intensity)
    {
        float red = 0;
        float green = 0;
        float blue = 0;

        Transformations.TransformationMatrixBuilder builder = new();

        Point3D pointAtDistance10FromIntersectionPoint = point + 10 * normalVector;

        // possibly need to divide by number of lightings
        float distributedIntensity = intensity;

        foreach (Axes axis in Enum.GetValues<Axes>())
        {
            for (float shift = -10; shift < 10; shift += 0.1f)
            {
                Point3D lightPosition = builder.Translate(axis, shift).Multiply(pointAtDistance10FromIntersectionPoint);

                PointLighting directional = new(color, distributedIntensity, lightPosition);

                float brightness = CalculateBrightness(directional, Array.Empty<IIntersectable>(), point, normalVector);
                if (brightness == 0)
                    continue;

                // considering distance
                brightness /= (lightPosition.GetDistance(point) * lightPosition.GetDistance(point));

                red += color.Red * distributedIntensity * brightness;
                green += color.Green * distributedIntensity * brightness;
                blue += color.Blue * distributedIntensity * brightness;
            }
        }

        Pixel resultColor = new((byte)red, (byte)green, (byte)blue);

        return resultColor;
    }
}
