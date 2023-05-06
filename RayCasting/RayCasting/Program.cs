using RayCasting;
using RayCasting.Objects;
using RayCasting.Lighting;
using RayCasting.Figures;
using RayCasting.Cameras;
using RayCasting.Scenes;
using RayCasting.Casters;
using System.Net;
using RayCasting.Writers;


//var vec1Norm = new Vector3D(1, 0.5f, -1).Normalized();
//var vec2Norm = new Vector3D(-1, 0.5f, -1).Normalized();

//var angles1 = vec1Norm.GetAngles();
//var angles2 = vec2Norm.GetAngles();

//var vec1 = new Vector3D(angles1);
//var vec2 = new Vector3D(angles2);


Point3D coordOrigin = new(0, 0, 0);
Vector3D negativeZedDirection = new(0, 0, -1);
float fov = 45;

int hRes = 100;
int wRes = hRes;

Camera cam1 = new(coordOrigin, negativeZedDirection, fov);
Screen screen1 = new(cam1, hRes, wRes);


Sphere sphere1 = new(new(0, 0, -3), 1f);


//TODO plane.Point is not considered
//TODO add check if points aren't on the same line
Plane plane1 = new(new(0, 0, -1), new(1, 0, 0), new(0, 1, 0));

Disk disk1 = new(new(0, 0, -2), 0.5f, new(4, 0, 1));

//TODO lightSource.Position is not considered (for sphere only?)

Point3D lightOrigin = new(-1, 4, 3); // test cases: (-10, 1, 1) (-1, 10, 1) (-1, 1, 1) (-1, 4, 3) (1, 1, -1)
DirectedLightSource lightSource = new(lightOrigin, new(lightOrigin, new(0, 0, -1)));
Scene scene1 = new(cam1, lightSource, screen1, sphere1);
Scene scene2 = new(cam1, lightSource, screen1, plane1);
Scene scene3 = new(cam1, lightSource, screen1, disk1);

Renderer renderer = new(scene3, new LightNeglectingCaster());
Renderer renderer2 = new(scene3, new LightConsideringCaster());

byte[,] image = renderer.Render();
byte[,] image2 = renderer2.Render();

ConsoleWriter writer = new();

writer.WriteNeglectingLight(image);

Console.WriteLine("----------------------------------------------------");

writer.Write(image2);