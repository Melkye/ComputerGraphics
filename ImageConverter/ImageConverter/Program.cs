using ImageConverter;
using ImageConverter.Bmp;
using ImageConverter.Ppm;
using static System.Net.Mime.MediaTypeNames;

internal class Program
{
    private static void Main(string[] args) // --source --goal-format --output
    {
        string source = "";
        string destination = "";
        string goal_format = "";

        string readLine = Console.ReadLine();
        string[] parameters = readLine.Split(' ', '=');
        for (int i = 0; i < parameters.Length; i++)
        {
            Console.WriteLine(parameters[i]);
        }

        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i] == "--output")
            {
                destination = parameters[i + 1];
            }
            if (parameters[i] == "--source")
            {
                source = parameters[i + 1];
            }
            if (parameters[i] == "--goal-format")
            {
                goal_format = parameters[i + 1];
            }
        }

        //C:\Users\arikt\Documents\Programming\ComputerGraphics\ImageConverter\Images\MARBLES.ppm
        //C:\Users\arikt\Documents\Programming\ComputerGraphics\ImageConverter\Images\output1.bmp

        //source = "C:\\Users\\arikt\\Documents\\Programming\\ComputerGraphics\\ImageConverter\\Images\\MARBLES.ppm";
        //destination = "C:\\Users\\arikt\\Documents\\Programming\\ComputerGraphics\\ImageConverter\\Images\\output1.bmp";

        ImageConverter.ImageConverter ic = new(new PpmImageReader(), new BmpImageWriter());

        ic.Convert(source, "", destination);
    }
}