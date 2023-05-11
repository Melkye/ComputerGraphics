using System.Buffers.Binary;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ImageConverter.Bmp
{
    public class BmpImageReader : IImageReader
    {
        public Image Read(string source)
        {
            Pixel[,] pixelMap;
            using(var fileStream = new FileStream(source, FileMode.Open, FileAccess.Read))
            {
                //FileHeader
                string signature = ReadString(2, Encoding.ASCII, fileStream);
                if (signature != "BM")
                    throw new ArgumentException("This format of BMP is not supported");

                int fileSize = ReadInt32(fileStream);

                //there must be reserved 4 bytes
                fileStream.Position += 4;

                int dataOffset = ReadInt32(fileStream);

                //InfoHeader
                //TODO: Check that InfoHeader size == 40
                int infoHeaderSize = ReadInt32(fileStream);

                int width = ReadInt32(fileStream);

                int height = ReadInt32(fileStream);

                short planes = ReadInt16(fileStream);
                //TODO: Check that bitsPerPixel is not supported(supported: )
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

                //Pixel array
                pixelMap = new Pixel[height,width];
                int bitsInRow = width * bitsPerPixel;
                int rowPaddingSizeBits = bitsInRow % 32 == 0 ? 0 : 32 - bitsInRow % 32;
                int rowPaddingSizeInBytes = rowPaddingSizeBits / 8;
                for (int i = height - 1; i >= 0; i--)
                {
                    for (int j = 0; j < width; j++)
                    {
                        switch (bitsPerPixel)
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
                    fileStream.Position += rowPaddingSizeInBytes;
                }
            }
            return new Image(pixelMap);
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