using System.Text;
using System.Buffers.Binary;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.IO.Pipes;

namespace ImageConverter.Bmp
{
    public class BmpImageWriter : IImageWriter
    {
        struct BmpFileHeader
        {
            public string Signature { get; }
            public int FileSize { get; }
            public int DataOffset { get; }
            public BmpFileHeader(string signature, int fileSize, int dataOffset)
            {
                Signature = signature;
                FileSize = fileSize;
                DataOffset = dataOffset;
            }
        }
        struct BmpInfoHeader
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
        public void Write(Image image, string destination)
        {
            using (var fileStream = new FileStream(destination, FileMode.Truncate, FileAccess.Write))
            {
                //Info about pixel array row size
                short bitsPerPixel = 3 * 8;
                int bitsInRow = image.Width * bitsPerPixel;
                int rowPaddingSizeBits = bitsInRow % 32 == 0 ? 0 : 32 - bitsInRow % 32;
                int rowPaddingSizeInBytes = rowPaddingSizeBits / 8;

                int fileHeaderSize = 14;
                int infoHeaderSize = 40;
                int fileSize = fileHeaderSize * 8 + infoHeaderSize * 8 + image.Height * (bitsInRow + rowPaddingSizeBits);
                int dataOffset = fileHeaderSize + infoHeaderSize;
                BmpFileHeader fileHeader = new BmpFileHeader(signature : "BM", fileSize, dataOffset);

                int width = image.Width;
                int height = image.Height;
                BmpInfoHeader infoHeader = new BmpInfoHeader(height, width, bitsPerPixel : 24);

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
                    fileStream.Position += rowPaddingSizeInBytes; 
                }
            }
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

            int imageSizeCompressed = 0;
            WriteInt32(imageSizeCompressed, fileStream);

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
            byte[] int32Bytes = new byte[4];
            BinaryPrimitives.WriteInt32LittleEndian(int32Bytes, value);
            fileStream.Write(int32Bytes);
        }
        private void WriteInt16(short value, FileStream fileStream)
        {
            byte[] int16Bytes = new byte[2];
            BinaryPrimitives.WriteInt16LittleEndian(int16Bytes, value);
            fileStream.Write(int16Bytes);
        }
        private void WriteInt8(byte value, FileStream fileStream)
        {
            byte[] int8Bytes = new byte[1] { value };
            fileStream.Write(int8Bytes);
        }
        private void WriteString(String value, Encoding encoding, FileStream fileStream)
        {
            byte[] stringBytes = encoding.GetBytes(value);
            fileStream.Write(stringBytes);
        }
    }
}