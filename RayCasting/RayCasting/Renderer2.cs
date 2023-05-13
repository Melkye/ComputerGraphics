using RayCasting.Casters;
using RayCasting.Objects;
using RayCasting.Scenes;

namespace RayCasting;

public class Renderer2
{
    public Renderer2(Scene scene, ICaster caster)
    {
        Scene = scene;
        Caster = caster;
    }

    public Scene Scene { get; }

    public ICaster Caster { get; }

    public byte[,] Render(int height, int width)
    {
        byte[,] image = new byte[height, width];
        float pixelWidthAngle = Scene.Camera.FieldOfView / width * (float)(Math.PI / 180);
        float pixelHeightAngle = Scene.Camera.FieldOfView / height * (float)(Math.PI / 180);

        Vector3D cameraDirectionForward = Scene.Camera.Direction;
        Vector3D cameraDirectionUp = Scene.Camera.UpDirection;
        Vector3D cameraDirectionRight = Scene.Camera.RightDirection;

        for (int i = 0; i < image.GetLength(0); i++)
        {
            float currentAngleVerticaly = pixelHeightAngle * height / 2 - pixelHeightAngle * i;
            Vector3D verticalVector = (float)Math.Tan(currentAngleVerticaly) * cameraDirectionUp;
            Vector3D orientationVectorWithoutWidth = cameraDirectionForward + verticalVector;

            for (int j = 0; j < image.GetLength(1); j++)
            {
                float currentAngleHorizontal = - pixelWidthAngle * width / 2 + pixelWidthAngle * j;
                Vector3D horizontalVector = (float)Math.Tan(currentAngleHorizontal) * cameraDirectionRight;
                Vector3D orientationVector = orientationVectorWithoutWidth + horizontalVector;

                image[i, j] = Caster.Cast(Scene, orientationVector.GetAngles());
            }
        }

        return image;
    }
}
