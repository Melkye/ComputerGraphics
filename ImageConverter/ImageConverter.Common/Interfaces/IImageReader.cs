using ImageConverter.Common.Structures;

namespace ImageConverter.Common.Interfaces;
public interface IImageReader
{
    Image Read(string source);
    bool CanRead(string source);
}
