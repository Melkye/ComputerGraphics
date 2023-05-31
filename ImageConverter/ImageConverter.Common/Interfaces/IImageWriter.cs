using ImageConverter.Common.Structures;

namespace ImageConverter.Common.Interfaces;
public interface IImageWriter
{
    void Write(Image image, string destination);
    bool CanWrite(string format);
}