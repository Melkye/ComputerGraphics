using System.Buffers.Binary;
using System.IO;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ImageConverter.Bmp
{
    public class BmpImageReader : IImageReader
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
            public short BitsPerPixel { get;  }
            public BmpInfoHeader(int height, int width, short bitsPerPixel)
            {
                Width = width;
                Height = height;
                BitsPerPixel = bitsPerPixel;        
            }
        }
        public Image Read(string source)
        {
            Pixel[,] pixelMap;
            using(var fileStream = new FileStream(source, FileMode.Open, FileAccess.Read))
            {
                BmpFileHeader fileHeaderInfo = ReadFileHeader(fileStream);

                BmpInfoHeader bmpInfoHeader = ReadInfoHeader(fileStream);
                
                pixelMap = new Pixel[bmpInfoHeader.Height, bmpInfoHeader.Width];

                if(bmpInfoHeader.Height > 0)
                    ReadPixelMatrix(ref pixelMap, bmpInfoHeader, fileStream);
                else
                    ReadPixelMatrixReverse(ref pixelMap, bmpInfoHeader, fileStream);
            }
            return new Image(pixelMap);
        }
        private BmpFileHeader ReadFileHeader(FileStream fileStream)
        {
            string signature = ReadString(2, Encoding.ASCII, fileStream);
            if (signature != "BM")
                throw new ArgumentException("This format of BMP is not supported");

            int fileSize = ReadInt32(fileStream);

            //there must be reserved 4 bytes
            fileStream.Position += 4;

            int dataOffset = ReadInt32(fileStream);

            return new BmpFileHeader(signature, fileSize, dataOffset);
        }
        private BmpInfoHeader ReadInfoHeader(FileStream fileStream)
        {
            //InfoHeader
            //TODO: Check that InfoHeader size == 40
            int infoHeaderSize = ReadInt32(fileStream);

            int width = ReadInt32(fileStream);

            int height = ReadInt32(fileStream);

            short planes = ReadInt16(fileStream);
            //TODO: Check that bitsPerPixel is not supported(supported: 24)
            short bitsPerPixel = ReadInt16(fileStream);
            //TODO: Check that compression == 0
            int compression = ReadInt32(fileStream);
            //TODO: Check that imageSizeCompressed == 0
            int imageSizeCompressed = ReadInt32(fileStream);
            //TODO: Check that XpixelsPerM == 0
            int XpixelsPerM = ReadInt32(fileStream);
            //TODO: Check that YpixelsPerM == 0
            int YpixelsPerM = ReadInt32(fileStream);
            //TODO: Check that colorUsed == 0
            int colorUsed = ReadInt32(fileStream);
            //TODO: Check that importantColors == 0
            int importantColors = ReadInt32(fileStream);
            return new BmpInfoHeader(height, width, bitsPerPixel);
        }
        private void ReadPixelMatrix(ref Pixel[,] pixelMap, BmpInfoHeader bmpInfoHeader, FileStream fileStream)
        {
            int bitsInRow = bmpInfoHeader.Width * bmpInfoHeader.BitsPerPixel;
            int rowPaddingSizeBits = bitsInRow % 32 == 0 ? 0 : 32 - bitsInRow % 32;
            int rowPaddingSizeInBytes = rowPaddingSizeBits / 8;
            for (int i = bmpInfoHeader.Height - 1; i >= 0; i--)
            {
                ReadPixelRow(ref pixelMap, i, bmpInfoHeader, fileStream);
                fileStream.Position += rowPaddingSizeInBytes;
            }
        }
        private void ReadPixelMatrixReverse(ref Pixel[,] pixelMap, BmpInfoHeader bmpInfoHeader, FileStream fileStream)
        {
            int bitsInRow = bmpInfoHeader.Width * bmpInfoHeader.BitsPerPixel;
            int rowPaddingSizeBits = bitsInRow % 32 == 0 ? 0 : 32 - bitsInRow % 32;
            int rowPaddingSizeInBytes = rowPaddingSizeBits / 8;
            for (int i = 0; i < -bmpInfoHeader.Height; i++)
            {
                ReadPixelRow(ref pixelMap, i, bmpInfoHeader, fileStream);
                fileStream.Position += rowPaddingSizeInBytes;
            }
        }
        private void ReadPixelRow(ref Pixel[,] pixelMap, int i, BmpInfoHeader bmpInfoHeader, FileStream fileStream)
        {
            for (int j = 0; j < bmpInfoHeader.Width; j++)
            {
                switch (bmpInfoHeader.BitsPerPixel)
                {
                    case 24:
                        {
                            byte blue = ReadInt8(fileStream);

                            byte green = ReadInt8(fileStream);

                            byte red = ReadInt8(fileStream);

                            pixelMap[i, j] = new Pixel(red, green, blue);
                            break;
                        }
                    default:
                        {
                            throw new ArgumentException("This bit per pixel value is not supported");
                        }
                }
            }
        }
        private int ReadInt32(FileStream fileStream)
        {
            byte[] int32Bytes = new byte[4];
            fileStream.Read(int32Bytes);
            int result = BinaryPrimitives.ReadInt32LittleEndian(int32Bytes);
            return result;
        }
        private short ReadInt16(FileStream fileStream)
        {
            byte[] int16Bytes = new byte[2];
            fileStream.Read(int16Bytes);
            short result = BinaryPrimitives.ReadInt16LittleEndian(int16Bytes);
            return result;
        }
        private byte ReadInt8(FileStream fileStream)
        {
            byte[] int8Bytes = new byte[1];
            fileStream.Read(int8Bytes);
            return int8Bytes[0];
        }
        private string ReadString(int size, Encoding encoding, FileStream fileStream)
        {
            byte[] stringBytes = new byte[size];
            fileStream.Read(stringBytes);
            string result = encoding.GetString(stringBytes);
            return result;
        }
    }
}