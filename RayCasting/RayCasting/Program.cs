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

        if (Path.GetExtension(source) == "")
        { 
        
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

            var transformationsBuilder = new TransformationMatrixBuilder();

            var f16Triangles = new ObjReader().ReadTriangles(source);

            //Scene f16Scene = new(cam1, lightings, f16Triangles);
            
            List<Scene> scenes = new List<Scene>();

            scenes.Add(new("f16Scene", cam1, lightings, f16Triangles));

            var f16Transform = transformationsBuilder
                .Rotate(Axes.Y, -90)
                .ThenTranslate(Axes.Z, -6)
                .ThenTranslate(Axes.Y, -4);

            foreach (var triangle in f16Triangles)
            {
                triangle.Transform(f16Transform);
            }

            // common

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
