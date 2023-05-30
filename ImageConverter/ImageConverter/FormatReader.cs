//using System.Text;

//namespace ImageConverter;

//public class FormatReader
//{
//    public string GetFileFormat(string filename)
//    {
//        string sourceFormat = "";

//        using (var fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
//        {
//            string starting21bytesString = ReadString(3, Encoding.ASCII, fileStream);

//            if (starting21bytesString.StartsWith("GIF"))
//            {
//                sourceFormat = "gif";
//            }
//            else if (starting21bytesString.StartsWith("BM"))
//            {
//                sourceFormat = "bmp";
//            }
//            else if (starting21bytesString.StartsWith("P3"))
//            {
//                sourceFormat = "ppm";
//            }
//            else
//            {
//                throw new ArgumentException("File format not supported");
//            }
//        }

//        return sourceFormat;
//    }
//    private string ReadString(int size, Encoding encoding, FileStream fileStream)
//    {
//        byte[] stringSplitIntoSizeBytes = new byte[size];
//        fileStream.Read(stringSplitIntoSizeBytes);
//        string result = encoding.GetString(stringSplitIntoSizeBytes);
//        return result;
//    }
//}