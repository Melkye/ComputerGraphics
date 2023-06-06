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
        //string sceneSelected = "";

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

        string f16Source = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Images\f-16.obj");
        string cowSource = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Images\cow.obj");

        try
        {
            Point3D coordOrigin = new(0, 0, 0);
            Vector3D negativeZDirection = new(0, 0, -1);
            Vector3D positiveYDirection = new(0, 1, 0);
            float hFov = 90;

            int vRes = 100;
            int hRes = 100;

            Camera cam1 = new(new(0, 0, 0), new(0, 0, -1), new(0, 1, 0), hFov);

            PointLighting pointLightingRedOneZeroOne = new(new(255, 0, 0), 1f, new(1, 0, 1));
            PointLighting pointLightingGreenMinusOneZeroOne = new(new(0, 255, 0), 1f, new(-1, 0, 1));
            DirectionalLighting blueLightToNegativeZed = new(new(0, 0, 255), 1, new(0, 0, -1));
            AmbientLighting redSun = new(new(255, 0, 0), 1f);

            var lightings = new ILighting[]
            {
                blueLightToNegativeZed,
                pointLightingRedOneZeroOne,
                pointLightingGreenMinusOneZeroOne
            };

            List<Scene> scenes = new List<Scene>();

            var cowTriangles = new ObjReader().ReadTriangles(cowSource);

            var f16Triangles = new ObjReader().ReadTriangles(f16Source);

            var transformationsBuilder = new TransformationMatrixBuilder();

            var triangleHell = new SceneCreator().TriangleHell("cowTriangleHell", cam1, lightings);

            var cowObjects = new List<IIntersectable>(cowTriangles)
            {
                new Sphere(new(0.3f, 0f, -0.5f), 0.05f),
                new Sphere(new(-0.3f, 0f, -0.5f), 0.05f),
            };

            var cowTransform = transformationsBuilder
                .Scale(0.5f)
                .ThenRotate(Axes.X, -90)
                .ThenRotate(Axes.Y, -180)
                .ThenTranslate(Axes.Z, -1)
                .ThenTranslate(Axes.Y, 0.03f);

            var f16Transform = transformationsBuilder
                .Rotate(Axes.Y, -90)
                .ThenTranslate(Axes.Z, -6)
                .ThenTranslate(Axes.Y, -5);

            var combine = f16Triangles.Concat(cowTriangles).ToArray();

            foreach (var triangle in f16Triangles)
            {
                triangle.Transform(f16Transform);
            }

            foreach (var triangle in cowTriangles)
            {
                triangle.Transform(cowTransform);
            }

            scenes.Add(new Scene("Standard", cam1, lightings, combine));

            Renderer rendererWithoutLight = new(scenes[0], new LightNeglectingCaster());
            Renderer rendererWithColors = new(scenes[0], new ColorConsideringCaster());

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
