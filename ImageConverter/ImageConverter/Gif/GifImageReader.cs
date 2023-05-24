using System.Buffers.Binary;
using System.Text;

namespace ImageConverter.Gif;

public class GifImageReader : IImageReader
{
    public Image Read(string source)
    {
        using FileStream fs = new(source, FileMode.Open, FileAccess.Read);

        string signature = ReadString(3, Encoding.ASCII, fs); // GIF
        string version = ReadString(3, Encoding.ASCII, fs); // 87a or 89a

        short width = ReadInt16(fs);
        short height = ReadInt16(fs);

        byte packed = ReadInt8(fs);

        byte globalColorTableFlag = (byte)(packed & 0b10000000);
        byte colorResolution = (byte)(packed & 0b01110000); // informatiional
        byte sortedFlag = (byte)(packed & 0b00001000); // isn't important
        byte sizeOfGlobalColorTableLog2 = (byte)((packed & 0b00000111) + 1);

        int numOfColors = 1 << sizeOfGlobalColorTableLog2;

        byte bgColorIndex = ReadInt8(fs);
        byte aspectRatio = ReadInt8(fs); // informational

        // TODO: add branches if global table exists / not exists -> use local table if global not exists

        Pixel[] globalColorTable = ReadColorTable(fs, numOfColors);

        // TODO: consider adding extension parser
        SkipExtensions(fs);

        byte separator = ReadInt8(fs); // always 0x2C

        (byte[] compressedBitMap, ImageDescriptor descriptor, byte lzwMinimumCodeSize) = ReadImage(fs);

        // TODO: write own compresser
        OwnLzwCompresser compresser = new();
        byte[]? uncompressedData = compresser.Decompress(compressedBitMap, lzwMinimumCodeSize);


        Pixel[,] pixelMap = CreateImageFromColorReferences(uncompressedData, globalColorTable, descriptor.Height, descriptor.Width);
        Image image = new(pixelMap);

        return image;
    }

    private Pixel[,] CreateImageFromColorReferences(byte[] colorReferences, Pixel[] colorTable, int height, int width)
    {
        Pixel[,] pixelMap = new Pixel[height, width];

        for (int i = 0; i < colorReferences.Length; i++)
        {
            int row = i / width;
            int column = i % width;

            byte colorReference = colorReferences[i];
            pixelMap[row, column] = colorTable[colorReference];
        }

        return pixelMap;
    }

    private Pixel[] ReadColorTable(FileStream fileStream, int numOfColors)
    {
        Pixel[] colorTable = new Pixel[numOfColors];

        for (int i = 0; i < numOfColors; i++)
        {
            byte red = (byte)fileStream.ReadByte();
            byte green = (byte)fileStream.ReadByte();
            byte blue = (byte)fileStream.ReadByte();

            colorTable[i] = new(red, green, blue);
        }

        return colorTable;
    }

    private (byte[] compressedBitMap, ImageDescriptor descriptor, byte lzwMinimumCodeSize) ReadImage(FileStream fileStream)
    {
        ImageDescriptor descriptor = ReadImageDescriptor(fileStream);

        byte lzwMinimumCodeSize = ReadInt8(fileStream);

        int blockStart = 0;
        byte blockSize = ReadInt8(fileStream);

        byte[] compressedBitMap = new byte[blockSize];

        while (blockSize != 0)
        {
            int blockEnd = blockStart + blockSize;

            if (blockEnd > compressedBitMap.Length)
            {
                byte[] largerCompressedBitMap = new byte[blockEnd];

                compressedBitMap.CopyTo(largerCompressedBitMap, 0);

                compressedBitMap = largerCompressedBitMap;
            }

            fileStream.Read(compressedBitMap.AsSpan()[blockStart..blockEnd]);

            blockStart += blockSize;
            blockSize = ReadInt8(fileStream);
        }
        return (compressedBitMap, descriptor, lzwMinimumCodeSize);
    }

    private void SkipExtensions(FileStream fileStream)
    {
        byte separator = 0x2C;

        byte element = (byte)fileStream.ReadByte();

        while (element != separator)
        {
            element = (byte)fileStream.ReadByte();
        }

        fileStream.Position -= 1;
    }

    private ImageDescriptor ReadImageDescriptor(FileStream fileStream)
    {
        short leftPos = ReadInt16(fileStream);
        short topPos = ReadInt16(fileStream);
        short width = ReadInt16(fileStream);
        short height = ReadInt16(fileStream);
        byte packed = ReadInt8(fileStream);

        byte localColorTableFlag = (byte)(packed & 0b10000000);
        byte interlaceFlag = (byte)(packed & 0b01000000);
        byte sortedFlag = (byte)(packed & 0b00100000);
        byte sizeOfLocalColorTableLog2 = (byte)((packed & 0b00000111) + 1);

        bool hasLocalColorTable = localColorTableFlag != 0;
        bool isInterlaced = interlaceFlag != 0;
        bool isSorted = sortedFlag != 0;

        ImageDescriptor descriptor;

        if (hasLocalColorTable)
        {
            Pixel[] localColorTable = ReadColorTable(fileStream, sizeOfLocalColorTableLog2);

            descriptor = new ImageDescriptor(
                leftPos,
                topPos,
                width,
                height,
                isInterlaced,
                isSorted,
                localColorTable);
        }
        else
        {

            descriptor = new ImageDescriptor(
                leftPos,
                topPos,
                width,
                height,
                isInterlaced,
                isSorted);
        }

        return descriptor;
    }

    private int ReadInt32(FileStream fileStream)
    {
        byte[] int32SplitIntoFourBytes = new byte[4];
        fileStream.Read(int32SplitIntoFourBytes);
        int result = BinaryPrimitives.ReadInt32LittleEndian(int32SplitIntoFourBytes);
        return result;
    }

    private short ReadInt16(FileStream fileStream)
    {
        byte[] int16SplitIntoTwoBytes = new byte[2];
        fileStream.Read(int16SplitIntoTwoBytes);
        short result = BinaryPrimitives.ReadInt16LittleEndian(int16SplitIntoTwoBytes);
        return result;
    }

    private byte ReadInt8(FileStream fileStream)
    {
        byte[] int8SplitIntoByte = new byte[1];
        fileStream.Read(int8SplitIntoByte);
        return int8SplitIntoByte[0];
    }

    private string ReadString(int size, Encoding encoding, FileStream fileStream)
    {
        byte[] stringSplitIntoSizeBytes = new byte[size];
        fileStream.Read(stringSplitIntoSizeBytes);
        string result = encoding.GetString(stringSplitIntoSizeBytes);
        return result;
    }
}