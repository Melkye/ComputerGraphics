using System.Text;
using System.Buffers.Binary;
using ImageConverter.Common.Interfaces;
using ImageConverter.Common.Structures;
using Bmp.Common;

namespace Writer.Bmp;

public class BmpImageWriter : IImageWriter
{
    private const string FileFormatSignature = "BM";
    private const string FileFormat = "bmp";
    public void Write(Image image, string destination)
    {
        using (var fileStream = new FileStream(destination, FileMode.Create, FileAccess.Write))
        {
            //Info about pixel array row size
            int width = image.Width;
            int height = image.Height;
            short bitsPerPixel = 3 * 8;
            short amountBitsInByte = 8;
            int bytesInRow = width * bitsPerPixel / amountBitsInByte;
            int rowPaddingSizeInBytes = bytesInRow % 4 == 0 ? 0 : 4 - bytesInRow % 4;

            int fileHeaderSize = 14;
            int infoHeaderSize = 40;
            int fileSize = fileHeaderSize + infoHeaderSize + height * (bytesInRow + rowPaddingSizeInBytes);
            int dataOffset = fileHeaderSize + infoHeaderSize;

            BmpFileHeader fileHeader = new BmpFileHeader(FileFormatSignature, fileSize, dataOffset);

            BmpInfoHeader infoHeader = new BmpInfoHeader(height, width, bitsPerPixel);

            WriteFileHeader(fileHeader, fileStream);

            WriteInfoHeader(infoHeader, fileStream);

            //Pixel array
            for (int i = image.Height - 1; i >= 0; i--)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    byte blue = image[i, j].Blue;
                    WriteInt8(blue, fileStream);

                    byte green = image[i, j].Green;
                    WriteInt8(green, fileStream);

                    byte red = image[i, j].Red;
                    WriteInt8(red, fileStream);
                }
                byte[] rowPaddingBytes = new byte[rowPaddingSizeInBytes];
                fileStream.Write(rowPaddingBytes);
            }
        }
    }

    public bool CanWrite(string format)
    {
        return format == FileFormat;
    }

    private void WriteFileHeader(BmpFileHeader fileHeader, FileStream fileStream)
    {
        string signature = fileHeader.Signature;
        WriteString(signature, Encoding.ASCII, fileStream);

        int fileSize = fileHeader.FileSize;
        WriteInt32(fileSize, fileStream);

        byte[] reservedFieldBytes = new byte[4] { 0, 0, 0, 0 };
        fileStream.Write(reservedFieldBytes);

        int dataOffset = fileHeader.DataOffset;
        WriteInt32(dataOffset, fileStream);
    }
    private void WriteInfoHeader(BmpInfoHeader infoHeader, FileStream fileStream)
    {
        int infoHeaderSize = 40;
        WriteInt32(infoHeaderSize, fileStream);

        int width = infoHeader.Width;
        WriteInt32(width, fileStream);

        int height = infoHeader.Height;
        WriteInt32(height, fileStream);

        short planes = 1;
        WriteInt16(planes, fileStream);

        short bitsPerPixel = infoHeader.BitsPerPixel;
        WriteInt16(bitsPerPixel, fileStream);

        int compression = 0;
        WriteInt32(compression, fileStream);

        int bytesInRow = width * infoHeader.BitsPerPixel / 8;
        int rowPaddingSizeBytes = bytesInRow % 4 == 0 ? 0 : 4 - bytesInRow % 4;

        int imageSize = height * (bytesInRow + rowPaddingSizeBytes);
        WriteInt32(imageSize, fileStream);

        int XpixelsPerM = 0;
        WriteInt32(XpixelsPerM, fileStream);

        int YpixelsPerM = 0;
        WriteInt32(YpixelsPerM, fileStream);

        int colorUsed = 0;
        WriteInt32(colorUsed, fileStream);

        int importantColors = 0;
        WriteInt32(importantColors, fileStream);
    }
    private void WriteInt32(int value, FileStream fileStream)
    {
        byte[] int32SplitIntoFourBytes = new byte[4];
        BinaryPrimitives.WriteInt32LittleEndian(int32SplitIntoFourBytes, value);
        fileStream.Write(int32SplitIntoFourBytes);
    }
    private void WriteInt16(short value, FileStream fileStream)
    {
        byte[] int16SplitIntoTwoBytes = new byte[2];
        BinaryPrimitives.WriteInt16LittleEndian(int16SplitIntoTwoBytes, value);
        fileStream.Write(int16SplitIntoTwoBytes);
    }
    private void WriteInt8(byte value, FileStream fileStream)
    {
        byte[] int8IntoByteArray = new byte[1] { value };
        fileStream.Write(int8IntoByteArray);
    }
    private void WriteString(String value, Encoding encoding, FileStream fileStream)
    {
        byte[] stringSplitIntoBytes = encoding.GetBytes(value);
        fileStream.Write(stringSplitIntoBytes);
    }
}