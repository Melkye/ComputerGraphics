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
float fov = 60;


// TODO: fix case when hres != vres
int vRes = 100;
int hRes = 100;

Camera cam1 = new(coordOrigin, negativeZedDirection, fov);


Sphere sphere1 = new(new(0, 0, -7), 1f);
Sphere sphere2 = new(new(0.8f, 1, -5), 0.5f);
Sphere sphere3 = new(new(0.5f, 0.5f, -2), 0.3f);
Sphere sphere4 = new(new(1.2f, 1, -5), 0.5f);

//TODO plane.Point is not considered
//TODO add check if points aren't on the same line
Plane plane1 = new(new(0, 0, -1), new(1, 0, 0), new(0, 1, 0));

// Disk: new(new(0, 0, -2), 0.5f, new(4, 0, 1));
// has super small brightness => isn't drawn
// bit it is closer then Sphere so Disk cits it in half

Disk disk1 = new(new(0, 0, -2), 0.5f, new(3, 0, 1));
Disk disk2 = new(new(-0.3f, -0.3f, -1), 0.3f, new(2, 0, 1));

//TODO lightSource.Position is not considered (for sphere only?)
IIntersectable[] figures = { sphere1, sphere2, sphere3, disk1, disk2 };

Point3D lightOrigin = new(-1, 4, 3); // test cases: (-10, 1, 1) (-1, 10, 1) (-1, 1, 1) (-1, 4, 3) (1, 1, -1)
DirectedLightSource lightSource = new(lightOrigin, new(lightOrigin, new(0, 0, -1)));

DirectedLightSource downsideLight = new(new(), new(0, -1, 0));
//Scene scene1 = new(cam1, lightSource, screen1, { sphere1 });
//Scene scene2 = new(cam1, lightSource, screen1, new() { plane1 });
//Scene scene3 = new(cam1, lightSource, screen1, new() { disk1 });

Scene scene4 = new(cam1, lightSource, figures);

Scene scene5 = new(cam1, downsideLight,
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

         // NOTE: normalVec is the same in each point, so when rendering each pixel cam ray goes through its angle values
         // and after one moment angle between ray and normalVec becomes < 90 -> negative cos(<90) -> negative val -> plane is not observed
         // but it should be (?)
        new Plane(new(0, 0, -10), new(0, 10, 1))
    });

//IIntersectable[] frogFigures = new IIntersectable[]
//{
//    new Sphere (new(0.8f, 1, -5), 0.5f),
//    new Sphere (new(0.5f, 0.5f, -2), 0.3f),
//    new Sphere(new(1.2f, 1, -5), 0.5f)
//};

//Scene frog = new(cam1, downsideLight, screen1, frogFigures);

Renderer renderer = new(scene5, new LightNeglectingCaster());
Renderer renderer2 = new(scene5, new LightConsideringCaster());

//byte[,] image = renderer.Render();
byte[,] image2 = renderer2.Render(vRes, hRes);

ConsoleWriter writer = new();

//writer.WriteNeglectingLight(image);

Console.WriteLine("----------------------------------------------------");

writer.Write(image2);