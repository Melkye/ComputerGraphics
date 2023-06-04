using ImageConverter;
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

    public Image Render(int height, int width)
    {
        float aspectRatio = width / (float)height;
        float hFov = Scene.Camera.FieldOfView;
        float vFov = hFov / aspectRatio;

        Pixel[,] pixelMap = new Pixel[height, width];
        float verticalAngleBetweenTwoPixels = vFov / (height - 1) * (float)(Math.PI / 180);
        float horizontalAngleBetweenTwoPixels = hFov / (width - 1) * (float)(Math.PI / 180);

        Vector3D cameraDirectionForward = Scene.Camera.ForwardDirection;
        Vector3D cameraDirectionUp = Scene.Camera.UpDirection;
        Vector3D cameraDirectionRight = Scene.Camera.RightDirection;

        float topMostPixelVerticalAngle = vFov / 2 * (float)(Math.PI / 180);
        float leftMostPixelHorizontalAngle = - hFov / 2 * (float)(Math.PI / 180);


        for (int i = 0; i < pixelMap.GetLength(0); i++)
        {
            /// varies from height/2 (upper border) to -height/2 (lower border)

            float pixelVerticalAngle = topMostPixelVerticalAngle - verticalAngleBetweenTwoPixels * i;

            //int pixelVerticalNumCountingFromCamForwardDir = (height / 2) - i;
            //float pixelVerticalAngle = onePixelVerticalAngle * pixelVerticalNumCountingFromCamForwardDir;
            Vector3D verticalVector = (float)Math.Tan(pixelVerticalAngle) * cameraDirectionUp;
            
            Vector3D pixelVerticalDirection = cameraDirectionForward + verticalVector;

            for (int j = 0; j < pixelMap.GetLength(1); j++)
            {
                // varies from -width/2 (left border) to width/2 (right border)
                float pixelHorizontalAngle = leftMostPixelHorizontalAngle + horizontalAngleBetweenTwoPixels * j;
                //int pixelHorizontalNumCountingFromCamForwardDir = - (width / 2) + j;
                //float pixelHorizontalAngle = onePixelHorizontalAngle * pixelHorizontalNumCountingFromCamForwardDir;
                Vector3D horizontalVector = (float)Math.Tan(pixelHorizontalAngle) * cameraDirectionRight;

                Vector3D pixelDirection = pixelVerticalDirection + horizontalVector;

                pixelMap[i, j] = Caster.Cast(Scene, pixelDirection.Normalized());
            }
        }

        return new Image(pixelMap);
    }
}
