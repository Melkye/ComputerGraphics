using ImageConverter;
using ImageConverter.Bmp;
using ImageConverter.Ppm;

internal class Program
{
    private static void Main(string[] args) // --source --goal-format --output
    {
        //Console.WriteLine("Hello, World!");

        ImageConverter.ImageConverter ic = new(new PpmImageReader(), new BmpImageWriter());

        string source = "C:\\Repos\\ComputerGraphics\\ImageConverter\\Images\\MARBLES.ppm";
        string destination = "C:\\Repos\\ComputerGraphics\\ImageConverter\\Images\\output.bmp";

        ic.Convert(source, "", destination);
    }
}