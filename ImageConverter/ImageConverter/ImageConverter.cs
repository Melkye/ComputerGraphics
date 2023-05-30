using ImageConverter.Bmp;
using ImageConverter.Gif;
using ImageConverter.Interfaces;
using ImageConverter.Ppm;
using ImageConverter.Structures;

namespace ImageConverter;

public class ImageConverter
{
    /// <exception cref="ArgumentException"></exception>
    public void Convert(string source, string goalFormat, string destination)
    {
        var reader =  GetReader(source);
        var writer = GetWriter(goalFormat);

        Image image = reader.Read(source);

    /// <exception cref="ArgumentException"></exception>
        writer.Write(image, destination);
    }
    {
        IImageReader reader = null;

        List<IImageReader> availableReaders = new()
        { 
            new PpmImageReader(),
            new BmpImageReader(),
            new GifImageReader(),
        };

        foreach(var r in availableReaders)
        {
            if (r.CanRead(source))
            {
                reader = r;
                break;
            }
        }

        if (reader is null)
        {
            throw new ArgumentException("Cant read this file format");
        }

        return reader;
    }

    /// <exception cref="ArgumentException"></exception>
    {
        IImageWriter writer = null;

        List<IImageWriter> availableWriters = new()
        {
            new PpmImageWriter(),
            new BmpImageWriter(),
        };

        foreach (var w in availableWriters)
        {
            if (w.CanWrite(format))
            {
                writer = w;
                break;
            }
        }

        if (writer is null)
        {
            throw new ArgumentException("Cant write this file format");
        }

        return writer;
    }
}