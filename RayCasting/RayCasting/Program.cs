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
using static System.Net.Mime.MediaTypeNames;

Point3D coordOrigin = new(0, 0, 0);
Vector3D negativeZDirection = new(0, 0, -1);
Vector3D positiveYDirection = new(0, 1, 0);
float hFov = 90;

int vRes = 400;
int hRes = 400;

Camera cam1 = new(new(0, 0, 1), new(0, 0, -1), new(0, 1, 0), hFov);

// TODO: fiz lighting issue: need to invert direction for cow
DirectedLightSource lightFromCam1 = new(new(100, 100, 100), -cam1.ForwardDirection);
DirectedLightSource lightFromCam1LeftDiag = new(new(100, 100, 100), -new Vector3D(-1, 0, -5));

//DirectedLightSource downsideLight = new(new(), new(0, -1, 0));

DirectedLightSource lightFromLeftToRightX = new(new(-10, 0, 0), new(1, 0, 0));


// cow

var cowTriangles = new ObjReader().Read();
Scene cowScene = new(cam1, lightFromCam1LeftDiag, cowTriangles);
//Renderer rendererWithoutLight = new(cowScene, new LightNeglectingCaster());
Renderer rendererWithoutShadows = new(cowScene, new LightConsideringCaster());
//Renderer rendererWithShadows = new(cowScene, new LightAndShadowConsideringCaster());

var transformationsBuilder = new TransformationMatrixBuilder();
var cowTransform = transformationsBuilder.Rotate(Axes.X, -90).ThenRotate(Axes.Y, 30);

foreach (var triangle in cowTriangles)
{
    triangle.Transform(cowTransform);
}

//var camTransform = transformationsBuilder.Translate(Axes.X, -2).ThenTranslate()
//cam1.Rotate()

//byte[,] image1 = rendererWithoutLight.Render(vRes, hRes);
byte[,] image2 = rendererWithoutShadows.Render(vRes, hRes);
//byte[,] image3 = rendererWithShadows.Render(vRes, hRes);


BmpImageWriter bmpWriter = new();

bmpWriter.Write(new MonochromeImageCreator().OneColorByteArrayToImage(image2), "C:\\Repos\\ComputerGraphics\\RayCasting\\RayCasting\\niceCowImage.bmp");


//bmpWriter.Write(new MonochromeImageCreator().OneColorByteArrayToImage(image1), "C:\\Repos\\ComputerGraphics\\RayCasting\\RayCasting\\cowImage1.bmp");
//bmpWriter.Write(new MonochromeImageCreator().OneColorByteArrayToImage(image2), "C:\\Repos\\ComputerGraphics\\RayCasting\\RayCasting\\cowImage2.bmp");
//bmpWriter.Write(new MonochromeImageCreator().OneColorByteArrayToImage(image3), "C:\\Repos\\ComputerGraphics\\RayCasting\\RayCasting\\cowImage3.bmp");


////triangle hell
//var triangleHell = new SceneCreator().TriangleHell(cam1, lightFromCam1);

//var trianglesTransformation = transformationsBuilder.Translate(3).ThenScale(Axes.X, -1).ThenScale(Axes.Y, -1);

//foreach (var tri in triangleHell.Figures)
//{
//    tri.Transform(trianglesTransformation);
//}
//Renderer renderer = new(triangleHell, new LightConsideringCaster());

//byte[,] image2 = renderer.Render(vRes, hRes);

//bmpWriter.Write(new MonochromeImageCreator().OneColorByteArrayToImage(image2), "C:\\Repos\\ComputerGraphics\\RayCasting\\RayCasting\\image2.bmp");

