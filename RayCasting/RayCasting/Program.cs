using RayCasting;
using RayCasting.Objects;
using RayCasting.Lighting;
using RayCasting.Figures;
using RayCasting.Cameras;
using RayCasting.Scenes;
using RayCasting.Casters;
using RayCasting.Writers;

Point3D coordOrigin = new(0, 0, 0);
Vector3D negativeZedDirection = new(0, 0, -1);
float fov = 60;

// TODO: fix case when hres != vres
// aspectRatio = hRes/vRes
// fov becomes vFov
// hFov = vFov  aspectRatio
int vRes = 100;
int hRes = 100;

Camera cam1 = new(coordOrigin, negativeZedDirection, new(0, 1, 0), fov);

Sphere sphere1 = new(new(0, 0, -7), 1f);
Sphere sphere2 = new(new(0.8f, 1, -5), 0.5f);
Sphere sphere3 = new(new(0.5f, 0.5f, -2), 0.3f);

Disk disk1 = new(new(0, 0, -2), 0.5f, new(3, 0, 1));
Disk disk2 = new(new(-0.3f, -0.3f, -1), 0.3f, new(2, 0, 1));

IIntersectable[] figures = { sphere1, sphere2, sphere3, disk1, disk2 };

Point3D lightOrigin = new(-1, 4, 3); // test cases: (-10, 1, 1) (-1, 10, 1) (-1, 1, 1) (-1, 4, 3) (1, 1, -1)
DirectedLightSource lightSource = new(lightOrigin, new(lightOrigin, new(0, 0, -1)));
DirectedLightSource downsideLight = new(new(), new(0, -1, 0));

Scene lotsOfThings = new(cam1, lightSource, figures);
Scene sphereInCenterHalfLighted = new(cam1, downsideLight, new IIntersectable[] { new Sphere(new(0, 0, -3), 1) });
Scene planeParallelToCam = new(cam1, downsideLight, new IIntersectable[] { new Plane(new(0, 0, 0), new(0, 1, 0)) });
Scene diskInLeftSide = new(cam1, lightSource, new IIntersectable[] { disk1 });

Scene nineShperesDeskAndPlane = new(cam1, downsideLight,
    new IIntersectable[] {
        new Sphere(new(-1.5f, 1.5f, -5), 0.5f),
        new Sphere(new(0, 1.5f, -5), 0.5f),
        new Sphere(new(1.5f, 1.5f, -5), 0.5f),
        new Sphere(new(-1.5f, 0, -5), 0.5f),
        new Sphere(new(0, 0, -5), 0.5f),
        new Sphere(new(1.5f, 0, -5), 0.5f),
        new Sphere(new(-1.5f, -1.5f, -5), 0.5f),
        new Sphere(new(0, -1.5f, -5), 0.5f),
        new Sphere(new(1.5f, -1.5f, -5), 0.5f),

        new Disk(new(0, 0, -10), 2, new(0, 1, 1)),

        new Plane(new(0, 0, -10), new(0, 10, 1))
    });

Renderer renderer = new(nineShperesDeskAndPlane, new LightNeglectingCaster());
Renderer renderer2 = new(nineShperesDeskAndPlane, new LightConsideringCaster());
Renderer2 renderer3 = new(nineShperesDeskAndPlane, new LightConsideringCaster());

//byte[,] image = renderer.Render();
byte[,] image2 = renderer2.Render(vRes, hRes);
byte[,] image3 = renderer3.Render(vRes, hRes);

ConsoleWriter writer = new();

//writer.WriteNeglectingLight(image);

Console.WriteLine("----------------------------------------------------");

writer.Write(image2);
Console.WriteLine("----------------------------------------------------");
writer.Write(image3);