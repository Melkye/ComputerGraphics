namespace ImageConverter;

public class ImageConverter
{
    private IImageReader _reader;

    private IImageWriter _writer;

    public ImageConverter(IImageReader reader, IImageWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }


    public void Convert(string source, string goalFormat, string destination)
    {
        var image = _reader.Read(source);

        _writer.Write(image, destination);
    }
}