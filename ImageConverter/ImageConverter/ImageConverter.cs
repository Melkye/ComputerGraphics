using ImageConverter.Bmp;
using ImageConverter.Gif;
using ImageConverter.Ppm;

namespace ImageConverter;

public class ImageConverter
{
    private IImageReader _reader;

    private IImageWriter _writer;

    // TODO: change source file extension retrieval
    public void Convert(string source, string goalFormat, string destination)
    {
        string sourceFormat = new FormatReader().GetFileFormat(source);

        SetReaderWriter(sourceFormat, goalFormat);

        Image image = _reader.Read(source);

        _writer.Write(image, destination);
    }

    private void SetReaderWriter(string sourceFormat, string goalFormat)
    {
        _reader = sourceFormat switch
        {
            "ppm" => new PpmImageReader(),
            "bmp" => new BmpImageReader(),
            "gif" => new GifImageReader(),
        };

        _writer = goalFormat switch
        {
            "ppm" => new PpmImageWriter(),
            "bmp" => new BmpImageWriter(),
        };
    }
}