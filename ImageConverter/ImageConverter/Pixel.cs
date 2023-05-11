namespace ImageConverter;

public readonly struct Pixel
{
    public byte Red { get; }
    public byte Green { get; }
    public byte Blue { get; }

    public Pixel(byte red, byte green, byte blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }
}
