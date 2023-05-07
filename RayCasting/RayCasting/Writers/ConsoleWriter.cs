
namespace RayCasting.Writers;
public class ConsoleWriter
{
    public void WriteNeglectingLight(byte[,] image)
    {
        for (int i = 0; i < image.GetLength(0); i++)
        {
            for (int j = 0; j < image.GetLength(1); j++)
            {
                char symbol = image[i, j] == 0 ? ' ' : '#';
                Console.Write(symbol);
            }
            Console.Write('\n');
        }
    }

    public void Write(byte[,] image)
    {
        for (int i = 0; i < image.GetLength(0); i++)
        {
            for (int j = 0; j < image.GetLength(1); j++)
            {
                char symbol = image[i, j] switch
                {
                    // HACK: do smth with these values, it's .8, .5. and .2 of 255
                    >204 => '#',
                    >127 => 'O',
                    >51 => '*',
                    >0 => '.',
                    0 => ' '
                };
                Console.Write(symbol);
            }
            Console.Write('\n');
        }
    }
}
