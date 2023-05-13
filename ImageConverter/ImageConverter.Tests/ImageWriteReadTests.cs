using ImageConverter;
using ImageConverter.Ppm;

namespace ImageConverter.Tests;

public class ImageWriteReadTests
{
    [Test, Category("Ppm")]
    public void Image_WriteToPpm_HaveRightHeaders()
    {
        Pixel[,] pixels = new Pixel[2, 3];
        pixels[0, 0] = new Pixel(255, 0, 0);
        pixels[0, 1] = new Pixel(0, 255, 0);
        pixels[0, 2] = new Pixel(0, 0, 255);
        pixels[1, 0] = new Pixel(255, 0, 0);
        pixels[1, 1] = new Pixel(0, 255, 0);
        pixels[1, 2] = new Pixel(0, 0, 255);

        var image = new Image(pixels);

        var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\testPpmWriter.ppm");

        IImageWriter writer = new PpmImageWriter();
        
        writer.Write(image, path);
        StreamReader reader = new StreamReader(path);
        string output = reader.ReadToEnd();
        reader.Close();

        Assert.That(output.StartsWith("P3"));
        //TODO: Here can be another delimiter
        Assert.That(output.Contains("3 2"));
        //TODO: It`s constant for our writer
        Assert.That(output.Contains("255"));
    }
    
    [Test, Category("Ppm")]
    public void Image_WriteToPpmReadFromPpm_SameImage()
    {
        Pixel[,] pixels = new Pixel[2, 3];
        pixels[0, 0] = new Pixel(255, 0, 0);
        pixels[0, 1] = new Pixel(0, 255, 0);
        pixels[0, 2] = new Pixel(0, 0, 255);
        pixels[1, 0] = new Pixel(255, 0, 0);
        pixels[1, 1] = new Pixel(0, 255, 0);
        pixels[1, 2] = new Pixel(0, 0, 255);

        var image = new Image(pixels);

        var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\testPpmWriter.ppm");

        IImageWriter writer = new PpmImageWriter();
        IImageReader reader = new PpmImageReader();

        writer.Write(image, path);
        Image output = reader.Read(path);

        Assert.That(output.Width == image.Width && output.Height == image.Height);
        //TODO: Maybe exists another way to assert equality of two two-dimensional arrays
        for (int i = 0; i < output.Height; i++)
        {
            for (int j = 0; j < output.Width; j++)
            {
                Assert.That(output[i, j], Is.EqualTo(image[i,j]));
            }
        }
    }
}