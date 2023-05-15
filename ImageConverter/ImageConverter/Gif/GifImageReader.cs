using System.Buffers.Binary;
using System.Drawing;
using System.Text;

namespace ImageConverter.Gif;

public class GifImageReader : IImageReader
{
    public Image Read(string source)
    {
        //Pixel[,] pixelMap;

        using FileStream fs = new(source, FileMode.Open, FileAccess.Read);

        // header
        string signature = ReadString(3, Encoding.ASCII, fs); // GIF
        string version = ReadString(3, Encoding.ASCII, fs); // 87a or 89a

        // lsd
        short width = ReadInt16(fs);
        short height = ReadInt16(fs);
        byte packed = ReadInt8(fs);

        // https://commandlinefanatic.com/art0113.png
        byte globalColorTableFlag = (byte)(packed & 0b10000000);
        byte colorResolution = (byte)(packed & 0b01110000); // informatiional
        byte sortedFlag = (byte)(packed & 0b00001000); // isn't important
        byte actualSizeOfGlobalColorTable = (byte)((packed & 0b00000111) + 1);

        byte bgColorIndex = ReadInt8(fs);
        byte aspectRatio = ReadInt8(fs); // informational

        //if (globalColorTableFlag == 128) // if flag is 1
        //{

        //byte[,] globalColorTable = new byte[rgbColorsCount, actualSizeOfGlobalColorTable];
        Pixel[] globalColorTable = new Pixel[actualSizeOfGlobalColorTable];

        for (byte i = 0; i < actualSizeOfGlobalColorTable; i++)
        {
            byte red = (byte)fs.ReadByte();
            byte green = (byte)fs.ReadByte();
            byte blue = (byte)fs.ReadByte();

            globalColorTable[i] = new(red, green, blue);
        }

        
        byte trailer = 0x3B;

        (byte[] compressedBitMap, ImageDescriptor descriptor) = ReadImage(fs);


        int uncompressedDataLength = descriptor.Width * descriptor.Height;

        Lzw.CompressedSize = compressedBitMap.Length;
        byte[] uncompressedData = Lzw.LzwDecompress(compressedBitMap);

        // TODO: parse byte array to image
        Pixel[,] pixelMap = new Pixel[descriptor.Height, descriptor.Width];

        // NOTE: this works if all images are consecutive
        for(int i = 0; i < uncompressedData.Length; i++)
        {
            int row = i % descriptor.Width;
            int column = i - row * descriptor.Width;
            pixelMap[row, column] = globalColorTable[uncompressedData[i]];
        }


        Image image = new(pixelMap);


        return image;
    }
    //private byte[] ReadData(FileStream fileStream)
    //{
    //    byte lzwMinCodeSize = ReadInt8(fileStream); // min or simple?
        

    //}
    private (byte[] compressedBitMap, ImageDescriptor descriptor) ReadImage(FileStream fileStream)
    {
        byte separator = ReadInt8(fileStream); // always 0x2C
        ImageDescriptor descriptor = ReadImageDescriptor(fileStream);

        ////TODO: write parser of sub blocks // extension blocks

        int compressedDataLength = ReadInt8(fileStream);
        byte[] compressedBitMap = new byte[compressedDataLength];

        byte lzwCodeSize = ReadInt8(fileStream);

        int blockPos = 0;
        byte blockSize = ReadInt8(fileStream);

        while (blockSize != 0)
        {
            fileStream.Read(compressedBitMap.AsSpan()[blockPos..blockSize]); // TODO check if it works UPD: it doesn't because bitmap size is smaller than block size

            blockPos += blockSize;
            blockSize = ReadInt8(fileStream);
        }
        return (compressedBitMap, descriptor);
    }
    //private byte[] ReadCompressedBlock(FileStream fileStream)
    //{
    //    byte blockSize = ReadInt8(fileStream);
    //    byte[] compressedData = new byte[blockSize];
    //    fileStream.Read(compressedData);
    //    return compressedData;
    //}

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
        byte actualSizeOfLocalColorTable = (byte)(packed & 0b00000111);

        bool hasLocalColorTable = localColorTableFlag != 0;
        bool isInterlaced = interlaceFlag != 0;
        bool isSorted = sortedFlag != 0;

        ImageDescriptor descriptor;

        if (hasLocalColorTable)
        {
            byte rgbColorsCount = 3;
            byte[,] localColorTable = new byte[rgbColorsCount, actualSizeOfLocalColorTable];
            // TODO: read local color table

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
        //TODO: write parser of sub blocks // extension blocks
        //byte extensionIntroducer = ReadInt8(fs);
        // 0x21 for graphics extension block
        // 0x21 for plain txt extension block
        // 0x21 for application extension block
        // 0x21 for comment extension block
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
