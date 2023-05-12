namespace ImageConverter.Bmp;

internal readonly struct BmpInfoHeader
{
    public int Width { get; }
    public int Height { get; }
    public short BitsPerPixel { get; }
    public BmpInfoHeader(int height, int width, short bitsPerPixel)
    {
        Width = width;
        Height = height;
        BitsPerPixel = bitsPerPixel;
    }
}