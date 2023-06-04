using ImageConverter;
using ImageConverter.Bmp;
using RayCasting;
using RayCasting.Cameras;
using RayCasting.Casters;
using RayCasting.Figures;
using RayCasting.Lighting;
using RayCasting.Objects;
using RayCasting.Scenes;
using RayCasting.Transformations;

internal class Program
{
    private static void Main(string[] args)
    {
        (string source, string destination) = ("", "");
        try
        {
            (source, destination) = new CommandLineArguments().ParserArgs(args);
        }
        catch (ArgumentException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.ForegroundColor = ConsoleColor.White;
        }


        try
        {
            Point3D coordOrigin = new(0, 0, 0);
            Vector3D negativeZDirection = new(0, 0, -1);
            Vector3D positiveYDirection = new(0, 1, 0);
            float hFov = 90;

            int vRes = 50;
            int hRes = 50;

            Camera cam1 = new(new(0, 0, 0), new(0, 0, -1), new(0, 1, 0), hFov);

            // TODO: fix lighting issue: need to invert direction for triangle or for cow
            // at start when setting lighting dir or in caster when calculating brightness

            PointLighting pointLightingRedOneZeroOne = new(new(255, 0, 0), 1f, new(1, 0, 1));
            PointLighting pointLightingGreenMinusOneZeroOne = new(new(0, 255, 0), 1f, new(-1, 0, 1));
            DirectionalLighting blueLightToNegativeZed = new(new(0, 0, 255), 1, new(0, 0, -1));
            AmbientLighting redSun = new(new(255, 0, 0), 1f);

            var lightings = new ILighting[]
            {
                //redSun,
                blueLightToNegativeZed,
                pointLightingRedOneZeroOne,
                pointLightingGreenMinusOneZeroOne
            };

            var transformationsBuilder = new TransformationMatrixBuilder();

            //var triangleHell = new SceneCreator().TriangleHell(cam1, ligntings);

            // cow

            //var cowTriangles = new ObjReader().ReadTriangles(source);

            //var cowObjects = new List<IIntersectable>(cowTriangles)
            //{
            //    new Sphere(new(0.3f, 0f, -0.5f), 0.05f),
            //    new Sphere(new(-0.3f, 0f, -0.5f), 0.05f),

            //    // background. do not use with ambient light
            //    //new Plane(new(0, 0, -100f), -cam1.ForwardDirection)
            //};

            //Scene cowScene = new(
            //    cam1,
            //    lightings,
            //    cowObjects.ToArray());

            //var cowTransform = transformationsBuilder
            //    .Rotate(Axes.X, -90)
            //    .ThenTranslate(Axes.Z, -1);

            //foreach (var triangle in cowTriangles)
            //{
            //    triangle.Transform(cowTransform);
            //}

            // f-16

            var f16Triangles = new ObjReader().ReadTriangles(source);

            Scene f16Scene = new(cam1, lightings, f16Triangles);

            var f16Transform = transformationsBuilder
                .Rotate(Axes.Y, -90)
                .ThenTranslate(Axes.Z, -6)
                .ThenTranslate(Axes.Y, -4);

            foreach (var triangle in f16Triangles)
            {
                triangle.Transform(f16Transform);
            }

            // common

            Renderer rendererWithoutLight = new(f16Scene, new LightNeglectingCaster());
            Renderer rendererWithColors = new(f16Scene, new ColorConsideringCaster());

            Image image = rendererWithColors.Render(vRes, hRes);

            BmpImageWriter bmpWriter = new();
            bmpWriter.Write(image, destination);
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.ForegroundColor = ConsoleColor.White;

        }
    }
}
