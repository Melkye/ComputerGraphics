using ImageConverter;

internal class Program
{
    private static void Main(string[] args)
    {
        (string source, string goalFormat, string destination) = ("", "", "");
        try
        {
            (source, goalFormat, destination) = new CommandLineArgsParser().ParseArgs(args);
        }
        catch (ArgumentException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        try
        {
            ImageConverter.ImageConverter ic = new();

            ic.Convert(source, goalFormat, destination);
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.ForegroundColor = ConsoleColor.White;

        }
    }
}