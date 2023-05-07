using RayCasting.Casters;
using RayCasting.Objects;
using RayCasting.Scenes;

namespace RayCasting;
public class Renderer
{
    public Renderer(Scene scene, ICaster caster)
    {
        Scene = scene; // TODO: create copy constructors?
        Caster = caster;
    }

    public Scene Scene { get; }

    public ICaster Caster { get; }

    // TODO: move pixel casting corresponding to cam pos and coords
    public byte[,] Render () // TODO: change output to color?
    {
        byte[,] image = new byte[Scene.Screen.Height, Scene.Screen.Width];

        // get vector with starting angle
        // move by alpha/width or alpha/height

        // TODO: change to hFov and vFov?
        float pixelWidthAngle = (Scene.Camera.FieldOfView / Scene.Screen.Width) * (float)(Math.PI / 180);
        float pixelHeightAngle = (Scene.Camera.FieldOfView / Scene.Screen.Height) * (float)(Math.PI / 180);


        (float alpha, float beta, float gamma) cameraDirectionAngles = Scene.Camera.Direction.GetAngles();

        // adding half of angle to target the middle of pixel
        (float alpha, float beta, float gamma) leftTopmostPixelAngles = (
            cameraDirectionAngles.alpha + pixelWidthAngle * Scene.Screen.Width / 2, // + pixelWidthAngle / 2,
            cameraDirectionAngles.beta - pixelHeightAngle * Scene.Screen.Height / 2, // + pixelWidthAngle / 2,
            cameraDirectionAngles.gamma);;

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
                //Point3D pixelPoint = new(); // TODO: find pixel coords
                //image[i, j] = Caster.Cast(Scene, pixelPoint);

                // TODO: create with pixel angle
                image[i, j] = Caster.Cast(Scene, currentPixelAngles);
            }
        }

        return image;
    }
    
}
