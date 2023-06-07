using RayCasting.Objects;
using RayCasting.Scenes;
using RayCasting.Figures;


using ImageConverter.Bmp;
using RayCasting.Lighting;
using RayCasting.Cameras;
using RayCasting.Casters;
using RayCasting.Writers;
using ImageConverter;

namespace RayCasting.Casters;

//IIntersectable[] array should be interpreted as a tree like in the link below
//https://www.geeksforgeeks.org/given-linked-list-representation-of-complete-tree-convert-it-to-linked-representation/
public class LightNeglectingKdTreeCaster : ICaster
{
    public Pixel Cast(Scene scene, Vector3D rayDirection)
    {
        Ray3D ray = new(scene.Camera.Position, rayDirection);

        Point3D? intersectionPoint = null;

        List<int> indicesToCheck = new List<int> { 0 };

        List<IIntersectable>? closestFilledBoxes = new List<IIntersectable>();
        float closestIntersectionDistance = float.PositiveInfinity;

        while (indicesToCheck.Count() > 0)
        {
            var leftBox = scene.FiguresInBoxes[2 * indicesToCheck[0] + 1];
            var rightBox = scene.FiguresInBoxes[2 * indicesToCheck[0] + 2];

            Point3D? leftBoxIntersection = leftBox.GetIntersectionPoint(ray);
            Point3D? rightBoxIntersection = rightBox.GetIntersectionPoint(ray);

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

        //if smallest box with triangles was found, intersect with triangle
        Point3D? testIntersectionPoint = null;
        foreach (var box in closestFilledBoxes)
        {
            foreach (var figure in box.GetFiguresInside())
            {
                testIntersectionPoint = figure.GetIntersectionPoint(ray);
                if (testIntersectionPoint is not null)
                {
                    if (ray.Origin.GetDistance((Point3D)testIntersectionPoint) < closestIntersectionDistance)
                        intersectionPoint = testIntersectionPoint;
                }
            }
        }

        if (intersectionPoint is null)
            return new Pixel(0, 0, 0);

        return new Pixel(255, 255, 255);
    }
}