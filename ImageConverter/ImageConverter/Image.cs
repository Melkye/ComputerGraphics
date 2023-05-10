using System.Reflection.Metadata.Ecma335;

namespace ImageConverter;

public class Image
{
    private Pixel[,] pixelmap;
    public int ColorMaxValue { get => 255; }
    public int Hieght => pixelmap.GetLength(0);
    public int Width => pixelmap.GetLength(1);
    public Pixel this[int i,int j] => pixelmap[i,j]; 
    public Image(Pixel[,] pixelmap)
    {
        this.pixelmap = pixelmap;
    }
}
