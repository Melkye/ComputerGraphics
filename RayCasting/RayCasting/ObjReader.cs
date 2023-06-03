using RayCasting.Figures;
using RayCasting.Objects;
using System.Globalization;

namespace RayCasting
{
    internal class ObjReader
    {
        public ObjReader() { }

        public Triangle[] ReadTriangles(string source)
        {
            string[] fileLines;
            string[] splitLine;
            List<string> indexes = new List<string>();
            List<Point3D> points = new List<Point3D>();
            List<Triangle> triangles = new List<Triangle>();

            fileLines = File.ReadAllLines(source);

            foreach (var line in fileLines)
            {
                if (line.StartsWith("v "))
                {
                    splitLine = line.Split();
                    points.Add(new Point3D(ToFloat(splitLine[1]),
                            ToFloat(splitLine[2]),
                            ToFloat(splitLine[3])));
                }
                if (line.StartsWith("f "))
                {
                    foreach (var index in line.Split())
                    {
                        if (index != "f")
                        {
                            indexes.Add(index.Split("//").First());
                        }
                    }

                    triangles.Add(new Triangle
                        (points.ElementAt(ToInt(indexes.ElementAt(0)) - 1),
                        points.ElementAt(ToInt(indexes.ElementAt(1)) - 1),
                        points.ElementAt(ToInt(indexes.ElementAt(2)) - 1)
                        ));

                    indexes.Clear();
                }
            }
            return triangles.ToArray();
        }

        private float ToFloat(string line)
        {
            return float.Parse(line, CultureInfo.InvariantCulture.NumberFormat);
        }

        private int ToInt(string line)
        {
            return Int32.Parse(line);
        }
    }
}
