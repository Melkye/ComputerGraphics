using RayCasting.Figures;
using RayCasting.Objects;
using System.Globalization;

namespace RayCasting
{
    internal class ObjReader
    {
        public ObjReader() { Reader(); }

        private void Reader()
        {
            int countV = 0, countF = 0;
            string[] splitLine;
            //if (File.Exists("C:\\Users\\arikt\\Documents\\Programming\\ComputerGraphics\\RayCasting\\RayCasting\\cow.obj"))
            //{
            string[] lines = File.ReadAllLines("C:\\Users\\arikt\\Documents\\Programming\\ComputerGraphics\\RayCasting\\RayCasting\\cow.obj");
            foreach (var line in lines)
            {
                if (line.StartsWith("v "))
                {
                    countV++;
                }
                if (line.StartsWith("f "))
                {
                    countF++;
                }
            }
            Point3D[] points = new Point3D[countV];
            Triangle[] triangles = new Triangle[countF];
            int i = 0, k = 0;
            foreach (var line in lines)
            {
                if (line.StartsWith("v "))
                {
                    splitLine = line.Split();
                    float test1, test2, test3;
                    test1 = float.Parse(splitLine[1], CultureInfo.InvariantCulture.NumberFormat);
                    test2 = float.Parse(splitLine[2], CultureInfo.InvariantCulture.NumberFormat);
                    test3 = float.Parse(splitLine[3], CultureInfo.InvariantCulture.NumberFormat);
                    points[i] = new(test1, test2, test3);
                    i++;
                }
                if (line.StartsWith("f "))
                {
                    List<string> coordinates = new List<string>();

                    splitLine = line.Split();

                    foreach (var coordString in splitLine)
                    {
                        if (coordString != "f")
                        {
                            string[] coordArr = coordString.Split("//");
                            string firstCoord = coordArr.First();
                            coordinates.Add(firstCoord);
                        }
                    }
                    string[] coordinatesArr = coordinates.ToArray();

                    triangles[k] = new(points[int.Parse(coordinatesArr[0]) - 1],
                        points[int.Parse(coordinatesArr[1]) - 1],
                        points[int.Parse(coordinatesArr[2]) - 1]);
                    k++;
                }
            }
        }
    }
}
