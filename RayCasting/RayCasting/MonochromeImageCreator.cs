using ImageConverter;

namespace RayCasting;

public class MonochromeImageCreator
{
    public Image OneColorByteArrayToImage(byte[,] imageInBytes)
    {
        int height = imageInBytes.GetLength(0);
        int width = imageInBytes.GetLength(1);

        Pixel[,] pixels = new Pixel[height, width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                byte color = imageInBytes[i, j];
                pixels[i, j] = new Pixel(color, color, color);
            }
        }

        Image image = new(pixels);

        return image;
    }
}
