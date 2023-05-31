using System.IO;
using ImageConverter.Common.Structures;

namespace Reader.Gif;

public readonly struct ImageDescriptor
{
    public readonly short LeftPos { get; }

    public readonly short TopPos { get; }

    public readonly short Width { get; }

    public readonly short Height { get; }

    public readonly bool HasLocalColorTable { get; }

    public readonly bool IsInterlaced { get; }

    public readonly bool IsSorted { get; }

    public readonly Pixel[] LocalColorTable { get; }

    public ImageDescriptor(
        short leftPos,
        short topPos,
        short width,
        short height,
        bool isInterlaced,
        bool isSorted,
        Pixel[] localColorTable)
    {
        LeftPos = leftPos;
        TopPos = topPos;

        Width = width;
        Height = height;

        HasLocalColorTable = true;
        IsInterlaced = isInterlaced;
        IsSorted = isSorted;
        LocalColorTable = localColorTable;
    }

    public ImageDescriptor(
        short leftPos,
        short topPos,
        short width,
        short height,
        bool isInterlaced,
        bool isSorted)
    {
        LeftPos = leftPos;
        TopPos = topPos;

        Width = width;
        Height = height;

        HasLocalColorTable = false;
        IsInterlaced = isInterlaced;
        IsSorted = isSorted;
        LocalColorTable = null;
    }
}
