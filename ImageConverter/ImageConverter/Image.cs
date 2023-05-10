namespace ImageConverter;

public class Image
{
    private Pixel[,] Pixelmap { get; }

    public Image(Pixel[,] pixelmap)
    {
        Pixelmap = pixelmap;
    }
}
