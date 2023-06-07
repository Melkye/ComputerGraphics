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
        //try
        //{
        //    (source, destination) = new CommandLineArguments().ParserArgs(args);
        //}
        //catch (ArgumentException e)
        //{
        //    Console.ForegroundColor = ConsoleColor.Red;
        //    Console.WriteLine(e.Message);
        //    Console.ForegroundColor = ConsoleColor.White;
        //}

        destination = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Images\output.png");
        string f16Source = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Images\f-16.obj");
        string cowSource = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Images\cow.obj");
        string geraltSource = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Images\geralt.obj");


        try
        {
            float hFov = 90;

            int vRes = 108*3;
            int hRes = 192*3;

            Camera cam1 = new(new(0, 0, 0), new(0, 0, -1), new(0, 1, 0), hFov);

            // TODO: fix lighting issue: need to invert direction for triangle or for cow
            // at start when setting lighting dir or in caster when calculating brightness

            PointLighting pointLightingRedOneZeroOne = new(new(255, 0, 0), 1f, new(1, 0, 1));
            PointLighting pointLightingGreenMinusOneZeroOne = new(new(0, 255, 0), 1f, new(-1, 0, 1));

            PointLighting warmLightingOneFiveNegSeven = new(new(253, 244, 220), 1f, new(10, 5, -7));
            PointLighting warmLightingMinusOneFiveNegSeven = new(new(253, 244, 220), 1f, new(-10, 5, -7));

            PointLighting pinkLightingOneFiveNegSeven = new(new(255, 105, 180), 1f, new(6, 4, -7));
            PointLighting cyanLightingMinusOneFiveNegSeven = new(new(0, 100, 100), 1f, new(-6, 4, -7));

            DirectionalLighting blueLightToNegativeZed = new(new(0, 0, 255), 1, new(0, 0, -1));
            DirectionalLighting whiteLightToNegativeZed = new(new(255, 255, 255), 1, new(0, 0, -1));

            AmbientLighting redSun = new(new(255, 0, 0), 1f);
            AmbientLighting sun = new(new(255, 255, 255), 1f);

            var lightings = new ILighting[]
            {
                sun,
                //redSun,

                //blueLightToNegativeZed,
                //whiteLightToNegativeZed,

                //pointLightingRedOneZeroOne,
                //pointLightingGreenMinusOneZeroOne,

                //warmLightingOneFiveNegSeven,
                //warmLightingMinusOneFiveNegSeven,

                //pinkLightingOneFiveNegSeven,
                //cyanLightingMinusOneFiveNegSeven,
            };

            
            // cows and geralt on plane
            var cowsOnPlane = new SceneCreator().CowsOnPlane(cowSource, f16Source, lightings);

            //var transformationsBuilder = new TransformationMatrixBuilder();
            //var transformation = transformationsBuilder.Rotate(Axes.Y, 50).ThenTranslate(3, 1, -2);
            //cowsAndGeraltOnPlane.Transform(transformation);

            // cow
            //var cow = new SceneCreator().Cow(cowSource, lightings);

            //cow.Figures = cow.Figures.Concat(new IIntersectable[] { new Sphere(new(2, 2, -1), 1) }).ToArray();
            //cow.Transform(transformationBuilder.);

            Console.WriteLine(DateTime.Now);
            Image image = new Renderer(cowsOnPlane, new LightNeglectingKdTreeCaster()).Render(vRes, hRes);
            Console.WriteLine(DateTime.Now);

            new BmpImageWriter().Write(image, destination);

        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.ForegroundColor = ConsoleColor.White;

        }
    }
}