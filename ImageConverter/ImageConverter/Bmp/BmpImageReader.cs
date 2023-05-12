﻿using System.Buffers.Binary;
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

            int fileSize = ReadInt32(fileStream);

            //there must be reserved 4 bytes
            fileStream.Position += 4;

            int dataOffset = ReadInt32(fileStream);

            return new BmpFileHeader(signature, fileSize, dataOffset);
        }
        private BmpInfoHeader ReadInfoHeader(FileStream fileStream)
        {
            int infoHeaderSize = ReadInt32(fileStream);
            if (infoHeaderSize != 40)
                throw new ArgumentException("Reader can read .bmp only where infoHeaderSize is 40");
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

            int imageSizeCompressed = ReadInt32(fileStream);
            if (imageSizeCompressed != 0)
                throw new ArgumentException("Reader can`t read .bmp with image compression");

            int XpixelsPerM = ReadInt32(fileStream);
            if (XpixelsPerM != 0)
                throw new ArgumentException("Reader can read .bmp only where XpixelsPerM is 0");

            int YpixelsPerM = ReadInt32(fileStream);
            if (YpixelsPerM != 0)
                throw new ArgumentException("Reader can read .bmp only where YpixelsPerM is 0");

            int colorUsed = ReadInt32(fileStream);
            if (colorUsed != 0)
                throw new ArgumentException("Reader can read .bmp only where colorUsed is 0");

            int importantColors = ReadInt32(fileStream);
            if (importantColors != 0)
                throw new ArgumentException("Reader can read .bmp only where importantColors is 0");
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
                byte blue = ReadInt8(fileStream);

                byte green = ReadInt8(fileStream);

                byte red = ReadInt8(fileStream);

                pixelMap[i, j] = new Pixel(red, green, blue);
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