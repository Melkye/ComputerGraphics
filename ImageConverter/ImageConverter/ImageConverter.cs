using ImageConverter.Common.Interfaces;
using ImageConverter.Common.Structures;

using Reader.Ppm;
using Reader.Bmp;
using Reader.Gif;
using Writer.Ppm;
using Writer.Bmp;
// TODO: add dynamic load of readers/writers and remove proj refs

namespace ImageConverter;

public class ImageConverter
{
    /// <exception cref="ArgumentException"></exception>
    public void Convert(string source, string goalFormat, string destination)
    {
        var reader =  GetReader(source);
        var writer = GetWriter(goalFormat);

        Image image = reader.Read(source);

        writer.Write(image, destination);
    }

    /// <exception cref="ArgumentException"></exception>
    private IImageReader GetReader(string source)
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
    private IImageWriter GetWriter(string format)
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