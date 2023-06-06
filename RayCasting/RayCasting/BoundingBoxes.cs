using RayCasting.Objects;
using RayCasting.Scenes;
using RayCasting.Figures;

namespace RayCasting.Casters;

//IIntersectable[] array should be interpreted as a tree like in the link below
//https://www.geeksforgeeks.org/given-linked-list-representation-of-complete-tree-convert-it-to-linked-representation/
public class BoundingBoxes
{
    public static IIntersectable[] GetBoundingBoxes(IIntersectable[] figures, int maxFiguresInBox)
    {
        List<Cuboid> boundingBoxes = new List<Cuboid> { GetBoundingBox(figures) };

        var predicates = new Func<IIntersectable, float>[] {
            ob => ob.GetCentralPoint().X,
            ob => ob.GetCentralPoint().Y,
            ob => ob.GetCentralPoint().Z };

        List<IIntersectable[]> sameLevelFigureSubsets = LevelFigureLeaves(
            new List<IIntersectable[]> { figures },
            predicates[0]);

        var maxSubsetSize = sameLevelFigureSubsets.Max(ob => ob.Count());



        //chop down figures into subsets until they are small enough
        int i = 1;
        while (maxSubsetSize > maxFiguresInBox)
        {
            foreach (var subset in sameLevelFigureSubsets)
            {
                boundingBoxes.Add(GetBoundingBox(subset));
            }

            sameLevelFigureSubsets = LevelFigureLeaves(
            sameLevelFigureSubsets,
            predicates[i]);

            maxSubsetSize = sameLevelFigureSubsets.Max(ob => ob.Count());

            i++;
            if (i >= predicates.Count())
                i = 0;
        }

        //for chopped down figures, insert them into final smallest bounding boxes
        foreach (var subset in sameLevelFigureSubsets)
        {
            Cuboid box = GetBoundingBox(subset);
            box.figuresInside = subset;
            boundingBoxes.Add(box);
        }


        // if (figures.Count() > 5)
        // {
        //     var polygonsSortedByX = figures.OrderByDescending(ob => ob.GetCentralPoint().X).ToArray();
        //     var polLeft = polygonsSortedByX.Take(polygonsSortedByX.Count() / 2).ToArray();
        //     var polRight = polygonsSortedByX.Skip(polygonsSortedByX.Count() / 2).ToArray();

        //     var pL = polygonsSortedByX[..(polygonsSortedByX.Count() / 2)];
        //     var pR = polygonsSortedByX[(polygonsSortedByX.Count() / 2)..];

        //     var firstBox = GetBoundingBox(polygonsSortedByX.ToArray());
        // }

        // foreach (var polygon in polygonsSortedByX.GetRange(0, polygonsSortedByX.Count / 2))
        // {
        //     foreach (var point in polygon.GetDiscretePoints())
        //     {
        //         minX > point.X ? minX = point.X : maxX < point.X ? maxX = po;
        //     }
        // }

        // polygonsSortedByX.Count;

        return boundingBoxes.Cast<IIntersectable>().ToArray(); ;
    }

    private static List<IIntersectable[]> LevelFigureLeaves(List<IIntersectable[]> figureParents, Func<IIntersectable, float> predicate)
    {
        List<IIntersectable[]> leaves = new List<IIntersectable[]>();
        foreach (var parent in figureParents)
        {
            var polygonsSortedBy = parent.OrderByDescending(predicate).ToArray();

            leaves.Add(polygonsSortedBy[..(polygonsSortedBy.Count() / 2)]); //left leaf
            leaves.Add(polygonsSortedBy[(polygonsSortedBy.Count() / 2)..]); //right leaf
        }

        return leaves;
    }

    private static Cuboid GetBoundingBox(IIntersectable[] figuresSubset)
    {
        float minX, maxX, minY, maxY, minZ, maxZ;
        minX = minY = minZ = float.PositiveInfinity;
        maxX = maxY = maxZ = float.NegativeInfinity;

        float tempXValue;
        for (int i = 0; i < figuresSubset[0].GetDiscretePoints().Count; i++)
        {
            tempXValue = figuresSubset.Min(ob => ob.GetDiscretePoints()[i].X);
            if (minX > tempXValue)
                minX = tempXValue;

            tempXValue = figuresSubset.Max(ob => ob.GetDiscretePoints()[i].X);
            if (maxX < tempXValue)
                maxX = tempXValue;
        }

        float tempYValue;
        for (int i = 0; i < figuresSubset[0].GetDiscretePoints().Count; i++)
        {
            tempYValue = figuresSubset.Min(ob => ob.GetDiscretePoints()[i].Y);
            if (minY > tempYValue)
                minY = tempYValue;

            tempYValue = figuresSubset.Max(ob => ob.GetDiscretePoints()[i].Y);
            if (maxY < tempYValue)
                maxY = tempYValue;
        }

        float tempZValue;
        for (int i = 0; i < figuresSubset[0].GetDiscretePoints().Count; i++)
        {
            tempZValue = figuresSubset.Min(ob => ob.GetDiscretePoints()[i].Z);
            if (minZ > tempZValue)
                minZ = tempZValue;

            tempZValue = figuresSubset.Max(ob => ob.GetDiscretePoints()[i].Z);
            if (maxZ < tempZValue)
                maxZ = tempZValue;
        }

        return new Cuboid(minX, maxX, minY, maxY, minZ, maxZ);
    }
}
