using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageConverter.Ppm
{
    internal class PpmImageWriter : IImageWriter
    {
        private const string fileFormatNumber = "P3";
        public void Write(Image image, string destination)
        {
            StreamWriter streamWriter = new StreamWriter(destination);
            streamWriter.Write(fileFormatNumber.ToString() + "\n");
            streamWriter.Write(image.Hieght.ToString() + " ");
            streamWriter.Write(image.Width.ToString() + "\n");
            streamWriter.Write(image.ColorMaxValue.ToString() + "\n");
            for (int i = 0; i < image.Hieght; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    streamWriter.Write(image[i, j].Red.ToString() + " ");
                    streamWriter.Write(image[i, j].Green.ToString() + " ");
                    streamWriter.Write(image[i, j].Blue.ToString() + " ");
                }
                streamWriter.Write("\n");
            }
            streamWriter.Close();
        }
    }
}
