using ImageConverter;
using RayCasting.Objects;

namespace RayCasting.Lighting;

public class AmbientLighting : ILighting
{
    public AmbientLighting(Pixel color, float intensity)
    {
        Color = color;
        Intensity = intensity;
    }

    public Pixel Color { get; }

    public float Intensity { get; }

    public Vector3D[] GetDirections(Point3D targetPoint)
    {
        var rand = new Random();
        var directions = new List<Vector3D>();

        // TODO make creation in sphere not in cube
        for (int i = 0; i < 40; i++)
        {
            float x = (float)(rand.NextDouble()*2 - 1);
            float y = (float)(rand.NextDouble()*2 - 1);
            float z = (float)(rand.NextDouble()*2 - 1);

            directions.Add(new Vector3D(x, y, z));
        }

        return directions.ToArray();
    }
}
