namespace ImageConverter;
public interface IImageWriter
{
    void Write(byte[] image, string destination);
}
