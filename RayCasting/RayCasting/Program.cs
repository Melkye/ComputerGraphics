using RayCasting;
using RayCasting.Objects;
using RayCasting.Lighting;
using RayCasting.Figures;
using RayCasting.Cameras;
using RayCasting.Scenes;
using RayCasting.Casters;


//var vec1Norm = new Vector3D(1, 0.5f, -1).Normalized();
//var vec2Norm = new Vector3D(-1, 0.5f, -1).Normalized();

//var angles1 = vec1Norm.GetAngles();
//var angles2 = vec2Norm.GetAngles();

//var vec1 = new Vector3D(angles1);
//var vec2 = new Vector3D(angles2);


Camera cam1 = new(new(0, 0, -4), new(0, 0, -1), 45);
Screen screen1 = new(cam1, 50, 50);

Sphere sphere1 = new(new(0, 0, -5), 0.4f);
//var bla = sphere1.GetIntersectionPoint(new(new(0, 0, 0), new(0.7745966f, -0.44721356f, 0.44721356f)));

Scene scene1 = new(cam1, null, screen1, sphere1);

Renderer renderer = new(scene1, new LightNeglectingCaster());

byte[,] image = renderer.Render();

for (int i = 0; i < image.GetLength(0); i++)
{
    for (int j = 0; j < image.GetLength(1); j++)
    {
        char symbol = image[i, j] == 0 ? ' ' : '#';
        Console.Write(symbol);
    }
    Console.Write('\n');
}
