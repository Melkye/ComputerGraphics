using ImageConverter;
using RayCasting;

internal class Program
{
    private static void Main(string[] args)
    {
        (string source, string destination) = ("", "");
        try
        {
            (source, destination) = new CommandLineArguments().ParserArgs(args);
        }
        catch (ArgumentException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        try
        {

        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.ForegroundColor = ConsoleColor.White;

        }
    }
}