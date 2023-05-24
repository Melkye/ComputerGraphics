using System.Text;

namespace ImageConverter.Ppm;

public class PpmImageReader : IImageReader
{
    private const string fileFormatNumber = "P3";
    public Image Read(string source)
    {
        Pixel[,] pixelmap;
        using (StreamReader streamReader = new StreamReader(source))
        {
            try
            {
                string fileFormatNumberInFile = ReadUntilDelimiter(streamReader);
                if (fileFormatNumberInFile != fileFormatNumber)
                    throw new ArgumentException("This file is not plain PPM format");
                int width = int.Parse(ReadUntilDelimiter(streamReader));
                int height = int.Parse(ReadUntilDelimiter(streamReader));
                int colorDepth = int.Parse(ReadUntilDelimiter(streamReader));
                pixelmap = new Pixel[height, width];
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
            }
            catch (ArgumentException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("File is corrupted");
            }
        }
        return new Image(pixelmap);
    }

    public bool CanRead(string source)
    {
        using (var fileStream = new FileStream(source, FileMode.Open, FileAccess.Read))
        {
            string starting14bytesString = ReadString(fileFormatNumber.Length, Encoding.ASCII, fileStream);

            return starting14bytesString.StartsWith(fileFormatNumber);
        }
    }

    private string ReadString(int size, Encoding encoding, FileStream fileStream)
    {
        byte[] stringSplitIntoSizeBytes = new byte[size];
        fileStream.Read(stringSplitIntoSizeBytes);
        string result = encoding.GetString(stringSplitIntoSizeBytes);
        return result;
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
        while (currentChar != '\n' && !stream.EndOfStream)
            currentChar = (char)stream.Read();
    }
}
