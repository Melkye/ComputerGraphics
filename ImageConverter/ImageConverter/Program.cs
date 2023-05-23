using ImageConverter.Bmp;
using ImageConverter.Gif;

internal class Program
{
    private static void Main(string[] args)
    {
        string gifImageSource = "C:\\Repos\\ComputerGraphics\\ImageConverter\\ImageConverter\\Images\\sample_640×426.gif";
        GifImageReader reader = new GifImageReader();

        var image = reader.Read(gifImageSource);

        string bmpImageDestination = "C:\\Repos\\ComputerGraphics\\ImageConverter\\ImageConverter\\Images\\GIF_TO_BMP.bmp";
        BmpImageWriter writer = new BmpImageWriter();

        writer.Write(image, bmpImageDestination);
        Input input = new Input(args);
    }
}