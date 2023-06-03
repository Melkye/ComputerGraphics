using RayCasting;
using RayCasting.Objects;
using RayCasting.Lighting;
using RayCasting.Figures;
using RayCasting.Cameras;
using RayCasting.Scenes;
using RayCasting.Casters;
using RayCasting.Writers;
using ImageConverter;
using ImageConverter.Bmp;
using RayCasting.Transformations;

Point3D coordOrigin = new(0, 0, 0);
Vector3D negativeZDirection = new(0, 0, -1);
Vector3D positiveYDirection = new(0, 1, 0);
float hFov = 90;

int vRes = 50;
int hRes = 50;

Camera cam1 = new(new(0, 0, 1), new(0, 0, -1), new(0, 1, 0), hFov);

// TODO: fiz lighting issue: need to invert direction for cow
DirectedLightSource lightFromCam1 = new(cam1.Position, -cam1.ForwardDirection);
//DirectedLightSource downsideLight = new(new(), new(0, -1, 0));

// cow

var cowTriangles = new ObjReader().Read();
Scene cowScene = new(cam1, lightFromCam1, cowTriangles);
Renderer renderer = new(cowScene, new LightNeglectingCaster());

var transformationsBuilder = new TransformationMatrixBuilder();
var cowTransform = transformationsBuilder.Rotate(Axes.X, -90);

foreach (var triangle in cowTriangles)
{
    triangle.Transform(cowTransform);
}

byte[,] image = renderer.Render(vRes, hRes);

BmpImageWriter bmpWriter = new();

bmpWriter.Write(new MonochromeImageCreator().OneColorByteArrayToImage(image), "C:\\Repos\\ComputerGraphics\\RayCasting\\RayCasting\\image.bmp");


// triangle hell
//var triangleHell = new SceneCreator().TriangleHell();

//var trianglesTransformation = transformationsBuilder.Translate(3).ThenScale(Axes.X, -1).ThenScale(Axes.Y, -1);

//foreach(var tri in triangleHell.Figures)
//{
//    tri.Transform(trianglesTransformation);
//}
//Renderer renderer = new(triangleHell, new LightNeglectingCaster());

//byte[,] image2 = renderer.Render(vRes, hRes);

//bmpWriter.Write(new MonochromeImageCreator().OneColorByteArrayToImage(image2), "C:\\Repos\\ComputerGraphics\\RayCasting\\RayCasting\\image2.bmp");

