namespace ImageConverter;
public interface IImageWriter
{
    void Write(Image image, string destination);
    bool CanWrite(string format);
}
