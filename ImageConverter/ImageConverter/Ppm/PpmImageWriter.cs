using System.Text;

namespace ImageConverter.Ppm;

public class PpmImageWriter : IImageWriter
{
    private const string fileFormatNumber = "P3";
    private const string fileFormat = "ppm";
    public void Write(Image image, string destination)
    {
        using (StreamWriter streamWriter = new StreamWriter(destination))
        {
            streamWriter.Write(fileFormatNumber.ToString() + "\n");
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
        if (format == fileFormat)
        {
            return true;
        }
        return false;
    }
}
