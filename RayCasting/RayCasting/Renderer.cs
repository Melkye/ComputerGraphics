using RayCasting.Casters;
using RayCasting.Objects;
using RayCasting.Scenes;

namespace RayCasting;
public class Renderer
{
    public Renderer(Scene scene, ICaster caster)
    {
        Scene = scene;
        Caster = caster;
    }

    public Scene Scene { get; }

    public ICaster Caster { get; }

    public byte[,] Render (int height, int width)
    {
        byte[,] image = new byte[height, width];
        float pixelWidthAngle = Scene.Camera.FieldOfView / width * (float)(Math.PI / 180);
        float pixelHeightAngle = Scene.Camera.FieldOfView / height * (float)(Math.PI / 180);

        (float alpha, float beta, float gamma) cameraDirectionAngles = Scene.Camera.Direction.GetAngles();

        (float alpha, float beta, float gamma) leftTopmostPixelAngles = (
            cameraDirectionAngles.alpha + pixelWidthAngle * width / 2,
            cameraDirectionAngles.beta - pixelHeightAngle * height / 2,
            cameraDirectionAngles.gamma);

        for (int i = 0; i < image.GetLength(0); i++)
        {
            (float alpha, float beta, float gamma) leftSidePixelAngles = (
            leftTopmostPixelAngles.alpha,
            leftTopmostPixelAngles.beta + pixelHeightAngle * i,
            leftTopmostPixelAngles.gamma);

            for (int j = 0; j < image.GetLength(1); j++)
            {
                (float alpha, float beta, float gamma) currentPixelAngles = (
                    leftSidePixelAngles.alpha - pixelWidthAngle * j,
                    leftSidePixelAngles.beta,
                    leftSidePixelAngles.gamma);

                image[i, j] = Caster.Cast(Scene, currentPixelAngles);
            }
        }

        return image;
    }
}
