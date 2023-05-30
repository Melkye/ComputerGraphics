using System.Text;
using ImageConverter.Interfaces;
using ImageConverter.Structures;

namespace ImageConverter.Ppm;

public class PpmImageWriter : IImageWriter
{
    private const string FileFormatNumber = "P3";
    private const string FileFormat = "ppm";
    public void Write(Image image, string destination)
    {
        using (StreamWriter streamWriter = new(destination))
        {
            streamWriter.Write(FileFormatNumber.ToString() + "\n");
            streamWriter.Write(image.Width.ToString() + " ");
            streamWriter.Write(image.Height.ToString() + "\n");
            streamWriter.Write(image.ColorMaxValue.ToString() + "\n");
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    streamWriter.Write(image[i, j].Red.ToString() + " ");
                    streamWriter.Write(image[i, j].Green.ToString() + " ");
                    streamWriter.Write(image[i, j].Blue.ToString() + " ");
                }
                streamWriter.Write("\n");
            }
        }
    }

    public bool CanWrite(string format)
    {
        return format == FileFormat;
    }
}
