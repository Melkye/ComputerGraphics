using ImageConverter;
using RayCasting.Figures;
using RayCasting.Lighting;
using RayCasting.Objects;
using RayCasting.Scenes;
using RayCasting.Transformations;

namespace RayCasting.Casters;

public class ColorKdTreeCaster : ICaster
{
    public Pixel Cast(Scene scene, Vector3D rayDirection)
    {
        Ray3D camRay = new(scene.Camera.Position, rayDirection);

        (Point3D? closestIntersectionPoint, Vector3D? normalVectorAtIntersectionPoint) =
            GetClosestIntersectionPointAndNormalVector(camRay, scene.FiguresInBoxes);

        if (closestIntersectionPoint is null)
            return new Pixel(0, 0, 0);

        Pixel color = GetColorAtPoint(scene.Lightings, scene.FiguresInBoxes, (Point3D)closestIntersectionPoint, (Vector3D)normalVectorAtIntersectionPoint);

        return color;
    }

    private (Point3D? closestIntersectionPoint, Vector3D? normalVectorAtIntersectionPoint)
        GetClosestIntersectionPointAndNormalVector(Ray3D camRay, IIntersectable[] figures)
    {
        Point3D? closestIntersectionPoint = null;
        List<int> indicesToCheck = new List<int> { 0 };

        List<IIntersectable>? closestFilledBoxes = new List<IIntersectable>();
        float closestIntersectionDistance = float.PositiveInfinity;

        while (indicesToCheck.Count() > 0)
        {
            var leftBox = figures[2 * indicesToCheck[0] + 1];
            var rightBox = figures[2 * indicesToCheck[0] + 2];

            Point3D? leftBoxIntersection = leftBox.GetIntersectionPoint(camRay);
            Point3D? rightBoxIntersection = rightBox.GetIntersectionPoint(camRay);

            if (leftBoxIntersection is not null)
            {
                if (leftBox.GetFiguresInside() is not null)
                {
                    closestFilledBoxes.Add(leftBox);
                }
                else
                {
                    indicesToCheck.Add(2 * indicesToCheck[0] + 1);
                }
            }
            if (rightBoxIntersection is not null)
            {
                if (rightBox.GetFiguresInside() is not null)
                {
                    closestFilledBoxes.Add(rightBox);
                }
                else
                {
                    indicesToCheck.Add(2 * indicesToCheck[0] + 2);
                }
            }

            indicesToCheck.RemoveAt(0);
        }

        Vector3D? normalVectorAtIntersectionPoint = null;

        Point3D? testIntersectionPoint = null;
        foreach (var box in closestFilledBoxes)
        {
            foreach (var figure in box.GetFiguresInside())
            {
                testIntersectionPoint = figure.GetIntersectionPoint(camRay);
                if (testIntersectionPoint is not null)
                {
                    if (camRay.Origin.GetDistance((Point3D)testIntersectionPoint) < closestIntersectionDistance)
                    {
                        closestIntersectionPoint = testIntersectionPoint;
                        normalVectorAtIntersectionPoint = figure.GetNormalVector((Point3D)closestIntersectionPoint);
                        closestIntersectionDistance = camRay.Origin.GetDistance((Point3D)closestIntersectionPoint);
                    }
                }
            }
        }

        return (closestIntersectionPoint, normalVectorAtIntersectionPoint);
    }

    private Pixel GetColorAtPoint(ILighting[] lightings, IIntersectable[] figures, Point3D point, Vector3D normalVector)
    {
        // TODO learn what to do when two lightings collide
        // currently they are added (value += (color * intensity) * brightness)
        // TODO add exposure
        // TODO gamma correction

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
        List<int> indicesToCheck = new List<int> { 0 };

        List<IIntersectable>? closestFilledBoxes = new List<IIntersectable>();

        Point3D? figureIntersectionPoint;

        while (indicesToCheck.Count() > 0)

        {
            var leftBox = figures[2 * indicesToCheck[0] + 1];
            var rightBox = figures[2 * indicesToCheck[0] + 2];

            Point3D? leftBoxIntersection = leftBox.GetIntersectionPoint(rayFromPointToLight);
            Point3D? rightBoxIntersection = rightBox.GetIntersectionPoint(rayFromPointToLight);


            if (leftBoxIntersection is not null)
            {
                if (leftBox.GetFiguresInside() is not null)
                {
                    foreach (var figure in leftBox.GetFiguresInside())
                    {
                        figureIntersectionPoint = figure.GetIntersectionPoint(rayFromPointToLight);
                        if (figureIntersectionPoint.Equals(point))
                            continue;
                        if (figureIntersectionPoint is not null)
                            return true;
                    }
                }
                else
                {
                    indicesToCheck.Add(2 * indicesToCheck[0] + 1);
                }
            }
            if (rightBoxIntersection is not null)
            {
                if (rightBox.GetFiguresInside() is not null)
                {
                    foreach (var figure in rightBox.GetFiguresInside())
                    {
                        figureIntersectionPoint = figure.GetIntersectionPoint(rayFromPointToLight);
                        if (figureIntersectionPoint.Equals(point))
                            continue;
                        if (figureIntersectionPoint is not null)
                            return true;
                    }
                }
                else
                {
                    indicesToCheck.Add(2 * indicesToCheck[0] + 2);
                }
            }

            indicesToCheck.RemoveAt(0);
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
