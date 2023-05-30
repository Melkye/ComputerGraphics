using ImageConverter.Structures;

namespace ImageConverter.Interfaces;
public interface IImageReader
{
    Image Read(string source);
    bool CanRead(string source);
}
