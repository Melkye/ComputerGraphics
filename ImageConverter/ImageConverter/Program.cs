using ImageConverter;
using ImageConverter.Bmp;
using ImageConverter.Ppm;

internal class Program
{
    private static void Main(string[] args) // --source smth --goal-format smth --output smth
    {
        string source = "";
        string destination = "";
        string goal_format = "";

        string basePath = System.IO.Directory.GetCurrentDirectory();
        //string readLine = Console.ReadLine();
        //string[] parameters = readLine.Split(' ', '=');
        //for (int i = 0; i < args.Length; i++)
        //{
        //    Console.WriteLine(args[i]);
        //}

        // TODO: check is user doesn't specify argument
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i] == "--output")
            {
                destination = args[i + 1];
                i++;
            }
            if (args[i] == "--source")
            {
                source = args[i + 1];
                i++;
            }
            if (args[i] == "--goal-format")
            {
                goal_format = args[i + 1];
                i++;
            }
        }

        //C:\Users\arikt\Documents\Programming\ComputerGraphics\ImageConverter\Images\MARBLES.ppm
        //C:\Users\arikt\Documents\Programming\ComputerGraphics\ImageConverter\Images\output1.bmp

        //source = "C:\\Users\\arikt\\Documents\\Programming\\ComputerGraphics\\ImageConverter\\Images\\MARBLES.ppm";
        //destination = "C:\\Users\\arikt\\Documents\\Programming\\ComputerGraphics\\ImageConverter\\Images\\output1.bmp";

        // TODO: create converter based on passed args
        // NOTE: consider using converter factory

        ImageConverter.ImageConverter ic = new(new PpmImageReader(), new BmpImageWriter());

        ic.Convert(source, goal_format, destination);
    }
}