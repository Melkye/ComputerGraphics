namespace ImageConverter;

public class Image
{
    public Pixel[,] Pixelmap { get; }
    public int ColorMaxValue { get => 255; }
    public int Hieght => Pixelmap.GetLength(0);
    public int Width => Pixelmap.GetLength(1);
    public Image(Pixel[,] pixelmap)
    {
        Pixelmap = pixelmap;
    }
}
