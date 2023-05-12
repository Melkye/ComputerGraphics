using System.Buffers.Binary;
using System.IO;
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
                try
                {
                    BmpFileHeader fileHeaderInfo = ReadFileHeader(fileStream);

                    BmpInfoHeader bmpInfoHeader = ReadInfoHeader(fileStream);

                    pixelMap = new Pixel[bmpInfoHeader.Height, bmpInfoHeader.Width];

                    if (bmpInfoHeader.Height > 0)
                        ReadPixelMatrix(ref pixelMap, bmpInfoHeader, fileStream);
                    else
                        ReadPixelMatrixReverse(ref pixelMap, bmpInfoHeader, fileStream);
                } 
                catch(ArgumentException e)
                {
                    throw e;
                } 
                catch(Exception e)
                {
                    throw new ArgumentException("File is corrupted : " + e.Message);
                }
            }
            return new Image(pixelMap);
        }
        private BmpFileHeader ReadFileHeader(FileStream fileStream)
        {
            string signature = ReadString(2, Encoding.ASCII, fileStream);
            if (signature != "BM")
                throw new ArgumentException("This format of BMP is not supported");

            int fileSizeInBytes = ReadInt32(fileStream);

            //there must be reserved 4 bytes
            fileStream.Position += 4;

            int dataOffset = ReadInt32(fileStream);

            return new BmpFileHeader(signature, fileSizeInBytes, dataOffset);
        }
        private BmpInfoHeader ReadInfoHeader(FileStream fileStream)
        {
            int infoHeaderSize = ReadInt32(fileStream);
            
            int width = ReadInt32(fileStream);

            int height = ReadInt32(fileStream);

            short planes = ReadInt16(fileStream);
            if (planes != 1)
                throw new ArgumentException("Reader can read .bmp only where number of planes is 1");

            short bitsPerPixel = ReadInt16(fileStream);
            if (bitsPerPixel != 24)
                throw new ArgumentException("Reader can read .bmp only with 24 bits per pixel");

            int compression = ReadInt32(fileStream);
            if (compression != 0)
                throw new ArgumentException("Reader can`t read .bmp with image compression");

            int imageSize = ReadInt32(fileStream);

            int XpixelsPerM = ReadInt32(fileStream);

            int YpixelsPerM = ReadInt32(fileStream);

            int colorUsed = ReadInt32(fileStream);

            int importantColors = ReadInt32(fileStream);
            
            return new BmpInfoHeader(height, width, bitsPerPixel);
        }
        private void ReadPixelMatrix(ref Pixel[,] pixelMap, BmpInfoHeader bmpInfoHeader, FileStream fileStream)
        {
            int bytesInRow = bmpInfoHeader.Width * bmpInfoHeader.BitsPerPixel / 8;
            int rowPaddingSizeInBytes = bytesInRow % 4 == 0 ? 0 : 4 - bytesInRow % 4;
            int rowPaddingSizeInBytes = rowPaddingSizeBits / 8;
            for (int i = bmpInfoHeader.Height - 1; i >= 0; i--)
            {
                ReadPixelRow(ref pixelMap, i, bmpInfoHeader, fileStream);
                fileStream.Position += rowPaddingSizeInBytes;
            }
        }
        private void ReadPixelMatrixReverse(ref Pixel[,] pixelMap, BmpInfoHeader bmpInfoHeader, FileStream fileStream)
        {
            int bytesInRow = bmpInfoHeader.Width * bmpInfoHeader.BitsPerPixel / 8;
            int rowPaddingSizeInBytes = bytesInRow % 4 == 0 ? 0 : 4 - bytesInRow % 4;
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
                byte blue = ReadInt8(fileStream);

                byte green = ReadInt8(fileStream);

                byte red = ReadInt8(fileStream);

                pixelMap[i, j] = new Pixel(red, green, blue);
            }
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
}