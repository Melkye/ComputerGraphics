//using RayCasting;
//using RayCasting.Objects;
//using RayCasting.Lighting;
//using RayCasting.Figures;
//using RayCasting.Cameras;
//using RayCasting.Scenes;
//using RayCasting.Casters;
//using RayCasting.Writers;
//using ImageConverter;
//using ImageConverter.Bmp;
//using RayCasting.Transformations;

//Point3D coordOrigin = new(0, 0, 0);
//Vector3D negativeZDirection = new(0, 0, -1);
//Vector3D positiveYDirection = new(0, 1, 0);
//float hFov = 90;

//int vRes = 500;
//int hRes = 500;

//Camera cam1 = new(coordOrigin, negativeZDirection, positiveYDirection, hFov);

////Sphere sphere1 = new(new(0, 0, -7), 1f);
////Sphere sphere2 = new(new(0.8f, 1, -5), 0.5f);
////Sphere sphere3 = new(new(0.5f, 0.5f, -2), 0.3f);

////Disk disk1 = new(new(0, 0, -2), 0.5f, new(3, 0, 1));
////Disk disk2 = new(new(-0.3f, -0.3f, -1), 0.3f, new(2, 0, 1));

////IIntersectable[] figures = { sphere1, sphere2, sphere3, disk1, disk2 };

////Point3D lightOrigin = new(-1, 4, 3); // test cases: (-10, 1, 1) (-1, 10, 1) (-1, 1, 1) (-1, 4, 3) (1, 1, -1)
////DirectedLightSource lightSource = new(lightOrigin, new(lightOrigin, new(0, 0, -1)));
////DirectedLightSource downsideLight = new(new(), new(0, -1, 0));

////Scene lotsOfThings = new(cam1, lightSource, figures);
////Scene sphereInCenterHalfLighted = new(cam1, downsideLight, new IIntersectable[] { new Sphere(new(0, 0, -3), 1) });
////Scene planeParallelToCam = new(cam1, downsideLight, new IIntersectable[] { new Plane(new(0, 0, 0), new(0, 1, 0)) });
////Scene diskInLeftSide = new(cam1, lightSource, new IIntersectable[] { disk1 });

////Scene nineShperesDeskAndPlane = new(cam1, downsideLight,
////    new IIntersectable[] {
////        new Sphere(new(-1.5f, 1.5f, -5), 0.5f),
////        new Sphere(new(0, 1.5f, -5), 0.5f),
////        new Sphere(new(3f, 2f, -5), 0.5f),
////        new Sphere(new(-6f, 0, -5), 0.5f),
////        new Sphere(new(0, 0, -5), 0.5f),
////        new Sphere(new(1.5f, 0, -5), 0.5f),
////        new Sphere(new(-1.5f, -1.5f, -5), 0.5f),
////        new Sphere(new(0, -1.5f, -5), 0.5f),
////        new Sphere(new(1.5f, -1.5f, -5), 0.5f),

////        //new Disk(new(0, 0, -10), 2, new(0, 1, 1)),

////        //new Sphere(new(1, 0, -3), 1)

////        //new Plane(new(0, 0, -10), new(0, 10, 1))
////    });
////;

////Renderer renderer = new(nineShperesDeskAndPlane, new LightConsideringCaster());

////byte[,] image = renderer.Render(vRes, hRes);

//// base image
//float coordZ = -10;

//Scene triangleHeaven = new(cam1, downsideLight,
//    new IIntersectable[]
//    {
//        new Triangle(new(-5f, 4f, coordZ), new(-4f, 6f, coordZ), new(-3f,4f, coordZ)),
//        new Triangle(new(-3f, 4f, coordZ), new(-2f, 6f, coordZ), new(-1f, 4f, coordZ)),
//        new Triangle(new(-1f, 4f, coordZ), new(0f, 6f, coordZ), new(1f, 4f, coordZ)),
//        new Triangle(new(1f, 4f, coordZ), new(2f, 6f, coordZ), new(3f, 4f, coordZ)),
//        new Triangle(new(3f, 4f, coordZ), new(4f, 6f, coordZ), new(5f, 4f, coordZ)),

//        new Triangle(new(-5f, 2f, coordZ), new(-4f, 4f, coordZ), new(-3f, 2f, coordZ)),
//        new Triangle(new(-3f, 2f, coordZ), new(-2f, 4f, coordZ), new(-1f, 2f, coordZ)),
//        new Triangle(new(-1f, 2f, coordZ), new(0f, 4f, coordZ), new(1f, 2f, coordZ)),
//        new Triangle(new(1f, 2f, coordZ), new(2f, 4f, coordZ), new(3f, 2f, coordZ)),
//        new Triangle(new(3f, 2f, coordZ), new(4f, 4f, coordZ), new(5f, 2f, coordZ)),

//        new Triangle(new(-5f, 0f, coordZ), new(-4f, 2f, coordZ), new(-3f, 0f, coordZ)),
//        new Triangle(new(-3f, 0f, coordZ), new(-2f, 2f, coordZ), new(-1f, 0f, coordZ)),
//        new Triangle(new(-1f, 0f, coordZ), new(0f, 2f, coordZ), new(1f, 0f, coordZ)),
//        new Triangle(new(1f, 0f, coordZ), new(2f, 2f, coordZ), new(3f, 0f, coordZ)),
//        new Triangle(new(3f, 0f, coordZ), new(4f, 2f, coordZ), new(5f, 0f, coordZ)),

//        new Triangle(new(-5f, -2f, coordZ), new(-4f, 0f, coordZ), new(-3f, -2f, coordZ)),
//        new Triangle(new(-3f, -2f, coordZ), new(-2f, 0f, coordZ), new(-1f, -2f, coordZ)),
//        new Triangle(new(-1f, -2f, coordZ), new(0f, 0f, coordZ), new(1f, -2f, coordZ)),
//        new Triangle(new(1f, -2f, coordZ), new(2f, 0f, coordZ), new(3f, -2f, coordZ)),
//        new Triangle(new(3f, -2f, coordZ), new(4f, 0f, coordZ), new(5f, -2f, coordZ)),

//        new Triangle(new(-5f, -4f, coordZ), new(-4f, -2f, coordZ), new(-3f, -4f, coordZ)),
//        new Triangle(new(-3f, -4f, coordZ), new(-2f, -2f, coordZ), new(-1f, -4f, coordZ)),
//        new Triangle(new(-1f, -4f, coordZ), new(0f, -2f, coordZ), new(1f, -4f, coordZ)),
//        new Triangle(new(1f, -4f, coordZ), new(2f, -2f, coordZ), new(3f, -4f, coordZ)),
//        new Triangle(new(3f, -4f, coordZ), new(4f, -2f, coordZ), new(5f, -4f, coordZ)),
//    });

//Renderer renderer = new(triangleHeaven, new LightNeglectingCaster());

//byte[,] image = renderer.Render(vRes, hRes);

////ConsoleWriter writer = new();
//BmpImageWriter bmpWriter = new();

////Console.WriteLine("----------------------------------------------------");


////// move left by -2
////var transformationBuilder = new TransformationMatrixBuilder();

////TransformationMatrix4x4 moveLeft = transformationBuilder.GetTranslationMatrix4x4(Axes.X, -2f);

////foreach (var triangle in triangleHeaven.Figures)
////{
////    triangle.Transform(moveLeft);
////}

//// rotate left by 45 degrees
//var transformationBuilder = new TransformationMatrixBuilder();


//// TODO check correcntess of multiple transform
//TransformationMatrix4x4 rotateLeft = transformationBuilder.GetRotationMatrix4x4(Axes.Y, 30);
//TransformationMatrix4x4 rotateUp = transformationBuilder.GetRotationMatrix4x4(Axes.X, 30);

//TransformationMatrix4x4 rotateLeftThenUp= rotateUp.Multiply(rotateLeft);

//foreach (var triangle in triangleHeaven.Figures)
//{
//    triangle.Transform(rotateLeft);
//}

////triangleHeaven.Camera.Transform(rotateLeft);

//triangleHeaven.Camera.Rotate(Axes.Y, 30);

//byte[,] image2 = renderer.Render(vRes, hRes);

//bmpWriter.Write(OneColorByteArrayToImage(image2), "C:\\Repos\\ComputerGraphics\\RayCasting\\RayCasting\\image2.bmp");

//foreach (var triangle in triangleHeaven.Figures)
//{
//    triangle.Transform(rotateUp);
//}

//byte[,] image3 = renderer.Render(vRes, hRes);

//bmpWriter.Write(OneColorByteArrayToImage(image2), "C:\\Repos\\ComputerGraphics\\RayCasting\\RayCasting\\image3.bmp");

////writer.WriteNeglectingLight(image);

//Console.WriteLine("----------------------------------------------------");

////writer.Write(image2);
//Console.WriteLine("----------------------------------------------------");
////writer.Write(image3);
using RayCasting;
using System.Diagnostics;
using System.IO;

namespace ReadATextFile
{
    class Program
    {
        static void Main(string[] args)
        {
            ObjReader reader = new ObjReader();

            reader.ReadTriangles();
        }
    }
}