namespace ImageConverter.Ppm;

public class PpmImageReader : IImageReader
{
    public Image Read(string source)
    {
        StreamReader streamReader = new StreamReader(source);
        string fileFormatNumber = ReadUntilDelimiter(streamReader);
        //TODO: ensure that fileFormatNumber P3
        int width = int.Parse(ReadUntilDelimiter(streamReader));
        int height = int.Parse(ReadUntilDelimiter(streamReader));
        //TODO: think about using colorDepth
        int colorDepth = int.Parse(ReadUntilDelimiter(streamReader));
        Pixel[,] pixelmap = new Pixel[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                string red = ReadUntilDelimiter(streamReader);
                byte redValue = byte.Parse(red);
                string green = ReadUntilDelimiter(streamReader);
                byte greenValue = byte.Parse(green);
                string blue = ReadUntilDelimiter(streamReader);
                byte blueValue = byte.Parse(blue);
                pixelmap[i, j] = new Pixel(redValue, greenValue, blueValue);
            }
        }
        return new Image(pixelmap);
    }

    private string ReadUntilDelimiter(StreamReader stream)
    {
        StringBuilder result = new();
        char currentChar = (char)stream.Read();
        while (!char.IsLetterOrDigit(currentChar))
        {
            if (currentChar == '#')
            {
                SkipToEndOfLine(stream);
            }
            currentChar = (char)stream.Read();
        }
        while (char.IsLetterOrDigit(currentChar))
        {
            result.Append(currentChar);
            currentChar = (char)stream.Read();
        }
        return result.ToString();
    }

    private void SkipToEndOfLine(StreamReader stream)
    {
        char currentChar = (char)stream.Read();
        while (currentChar != '\n')
            currentChar = (char)stream.Read();
    }
}
