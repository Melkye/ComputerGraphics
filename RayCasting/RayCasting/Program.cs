//using RayCasting;
//using RayCasting.Objects;
//using RayCasting.Lighting;
//using RayCasting.Figures;
//using RayCasting.Cameras;
//using RayCasting.Scenes;
//using RayCasting.Casters;
//using RayCasting.Writers;
//using ImageConverter;

//Point3D coordOrigin = new(0, 0, -1);
//Vector3D negativeZDirection = new(0, 0, -1);
//Vector3D positiveYDirection = new(0, 1, 0);
//float vFov = 120;

//int vRes = 1080;
//int hRes = 1920;

//Camera cam1 = new(coordOrigin, negativeZDirection, positiveYDirection, vFov);

//Sphere sphere1 = new(new(0, 0, -7), 1f);
//Sphere sphere2 = new(new(0.8f, 1, -5), 0.5f);
//Sphere sphere3 = new(new(0.5f, 0.5f, -2), 0.3f);

//Disk disk1 = new(new(0, 0, -2), 0.5f, new(3, 0, 1));
//Disk disk2 = new(new(-0.3f, -0.3f, -1), 0.3f, new(2, 0, 1));

//IIntersectable[] figures = { sphere1, sphere2, sphere3, disk1, disk2 };

//Point3D lightOrigin = new(-1, 4, 3); // test cases: (-10, 1, 1) (-1, 10, 1) (-1, 1, 1) (-1, 4, 3) (1, 1, -1)
//DirectedLightSource lightSource = new(lightOrigin, new(lightOrigin, new(0, 0, -1)));
//DirectedLightSource downsideLight = new(new(), new(0, -1, 0));

//Scene lotsOfThings = new(cam1, lightSource, figures);
//Scene sphereInCenterHalfLighted = new(cam1, downsideLight, new IIntersectable[] { new Sphere(new(0, 0, -3), 1) });
//Scene planeParallelToCam = new(cam1, downsideLight, new IIntersectable[] { new Plane(new(0, 0, 0), new(0, 1, 0)) });
//Scene diskInLeftSide = new(cam1, lightSource, new IIntersectable[] { disk1 });

//Scene nineShperesDeskAndPlane = new(cam1, downsideLight,
//    new IIntersectable[] {
//        new Sphere(new(-1.5f, 1.5f, -5), 0.5f),
//        new Sphere(new(0, 1.5f, -5), 0.5f),
//        new Sphere(new(3f, 2f, -5), 0.5f),
//        new Sphere(new(-6f, 0, -5), 0.5f),
//        new Sphere(new(0, 0, -5), 0.5f),
//        new Sphere(new(1.5f, 0, -5), 0.5f),
//        new Sphere(new(-1.5f, -1.5f, -5), 0.5f),
//        new Sphere(new(0, -1.5f, -5), 0.5f),
//        new Sphere(new(1.5f, -1.5f, -5), 0.5f),

//        //new Disk(new(0, 0, -10), 2, new(0, 1, 1)),

//        //new Sphere(new(1, 0, -3), 1)

//        //new Plane(new(0, 0, -10), new(0, 10, 1))
//    });
//;

//Renderer renderer = new(nineShperesDeskAndPlane, new LightConsideringCaster());

//byte[,] image = renderer.Render(vRes, hRes);

//ConsoleWriter writer = new();
//ImageConverter.Bmp.BmpImageWriter bmpWriter = new();

//bmpWriter.Write(OneColorByteArrayToImage(image), "C:\\Repos\\ComputerGraphics\\RayCasting\\RayCasting\\image.bmp");

////writer.WriteNeglectingLight(image);

//Console.WriteLine("----------------------------------------------------");

////writer.Write(image2);
//Console.WriteLine("----------------------------------------------------");
////writer.Write(image3);

//Image OneColorByteArrayToImage(byte[,] image)
//{
//    int height = image.GetLength(0);
//    int width = image.GetLength(1);
//    Pixel[,] pixels = new Pixel[height, width];
//    for (int i = 0; i < height; i++)
//    {
//        for (int j = 0; j < width; j++)
//        {
//            byte color = image[i, j];
//            pixels[i, j] = new Pixel(color, color, color);
//        }
//    }
//    return new Image(pixels);
//}

using RayCasting;

namespace ReadATextFile
{
    class Program
    {
        static void Main(string[] args)
        {
            ObjReader reader = new ObjReader();
        }
    }
}