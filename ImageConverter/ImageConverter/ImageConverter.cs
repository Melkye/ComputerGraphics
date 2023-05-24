using ImageConverter.Bmp;
using ImageConverter.Gif;
using ImageConverter.Ppm;

namespace ImageConverter;

public class ImageConverter
{
    private IImageReader _reader;

    private IImageWriter _writer;

    public void Convert(string source, string goalFormat, string destination)
    {
        string sourceFormat = new FormatReader().GetFileFormat(source);

        SetReader(source);
        SetWriter(goalFormat);

        Image image = _reader.Read(source);

        _writer.Write(image, destination);
    }

    private void SetReader(string source)
    {
        List<IImageReader> readers = new()
        { 
            new PpmImageReader(),
            new BmpImageReader(),
            new GifImageReader(),
        };

        foreach(var reader in readers)
        {
            if (reader.CanRead(source))
            {
                _reader = reader;
                break;
            }
        }

        if (_reader is null)
        {
            throw new ArgumentException("Cant read this file format");
        }
    }

    public void SetWriter(string goalFormat)
    {
        List<IImageWriter> writers = new()
        {
            new PpmImageWriter(),
            new BmpImageWriter(),
        };

        foreach (var writer in writers)
        {
            if (writer.CanWrite(goalFormat))
            {
                _writer = writer;
                break;
            }
        }

        if (_writer is null)
        {
            throw new ArgumentException("Cant write this file format");
        }
    }
}