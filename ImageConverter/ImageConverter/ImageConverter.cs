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

        writer.Write(image, destination);
    }

    /// <param name="source"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
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