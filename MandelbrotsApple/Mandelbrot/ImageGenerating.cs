namespace MandelbrotsApple.Mandelbrot;

using static Generating;

public static class ImageGenerating
{
    private static int RGBSize = 3;

    public static byte[] 
    MandelbrotImage(MandelbrotParameter parameter)
        => MandelbrotSet(parameter).Aggregate(CreateImage(parameter), (currentImage, pixel) => AggregatePixel(currentImage, parameter.ImageSize.Width, pixel, parameter.MaxIterations));


    private static byte[] CreateImage(MandelbrotParameter parameter)
        => new byte[parameter.ImageSize.Width * parameter.ImageSize.Height * RGBSize];

    private static byte[] AggregatePixel(byte[] image, int imageWidth, IterationPixel pixel, int maxIterations)
    {
        var index = (pixel.Y * imageWidth + pixel.X) * RGBSize;
        var color = Color(pixel.IterationPixelValue, maxIterations);
        image[index] = color.Red;
        image[index + 1] = color.Green;
        image[index + 2] = color.Blue;
        return image;
    }

    private static (byte Red, byte Green, byte Blue) Color(int iteration, int maxIteration) {
        if (iteration >= maxIteration) {
                return (0, 0, 0);
        }
        else
        {
            switch (iteration % 16 + 1)
            {
                case 1:  return (0, 0, 200);
                case 2:  return (0, 0, 218);
                case 3:  return (0, 0, 236);
                case 4:  return (0, 0, 255);
                                
                case 5:  return (0, 255, 0);
                case 6:  return (0, 236, 0);
                case 7:  return (0, 218, 0);
                case 8:  return (0, 200, 0);
                                
                case 9:  return (200, 0, 0);
                case 10: return (218, 0, 0);
                case 11: return (236, 0, 0);
                case 12: return (255, 0, 0);
                                
                case 13: return (255, 255, 0);
                case 14: return (236, 236, 0);
                case 15: return (218, 218, 0);
                case 16: return (200, 200, 0);
                default: return (0, 0, 0);
            }
        }
    }


}
