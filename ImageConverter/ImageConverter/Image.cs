namespace ImageConverter;

public class Image
{
    private Pixel[,] pixelMap;
    public int ColorMaxValue => 255;
    public int Height => pixelMap.GetLength(0);
    public int Width => pixelMap.GetLength(1);
    public Pixel this[int i,int j] => pixelMap[i,j]; 
    public Image(Pixel[,] pixelMap)
    {
        int width = pixelMap.GetLength(0);
        int height = pixelMap.GetLength(1);
        this.pixelMap = new Pixel[width, height];
        Array.Copy(pixelMap, this.pixelMap, width * height);
    }
}
