namespace MandelbrotsApple.Mandelbrot;

using LaYumba.Functional;
using static Common;

public class Generating
{
    public static byte[] MandelbrotImage(MandelbrotParameter parameter)
        => MandelbrotImage(parameter, BytesPerNumber(parameter.MaxIterations));

    private static byte[] MandelbrotImage(MandelbrotParameter parameter, int bytesPerPixel)
        => MandelbrotImage(parameter, CreateImage(parameter.ImageSize, bytesPerPixel), BytesPerNumber(parameter.MaxIterations));

    private static byte[] MandelbrotImage(MandelbrotParameter parameter, byte[] image, int bytesPerPixel)
    {
        var aspectRaitio = parameter.ImageSize.Width / (double)parameter.ImageSize.Height;
        var ymin = parameter.CurrentMandelbrotSize.Min.Y;
        var ymax = parameter.CurrentMandelbrotSize.Max.Y;
        var height = ymax - ymin;
        var diff = (height - height / aspectRaitio) / 2.0;
        ymin += diff;
        ymax -= diff;

        MandelbrotSet(
            image,
            parameter.ImageSize.Width,
            parameter.ImageSize.Height,
            parameter.CurrentMandelbrotSize.Min.X,
            parameter.CurrentMandelbrotSize.Max.X,
            ymin,
            ymax,
            parameter.MaxIterations,
            bytesPerPixel,
            XCoordinates(parameter.ImageSize.Width, parameter.CurrentMandelbrotSize.Min.X, parameter.CurrentMandelbrotSize.Max.X, bytesPerPixel));
        return image;
    }

    private static byte[] CreateImage(ImageSize imageSize, int bytesPerPixel) => new byte[imageSize.Width * imageSize.Height * bytesPerPixel];

    private static void
    MandelbrotSet(byte[] image, int imageWidth, int imageHeight, double xMin, double xMax, double yMin, double yMax, int maxIterations, int bytesPerPixel, (int Adress, double Coordinate)[] xCoordinates)
        => YCoordinates(imageWidth, imageHeight, yMin, yMax, bytesPerPixel)
           .AsParallel()
           .ForAll(y => xCoordinates
                        .ForEach(x => MandelbrotPixel(image, y.Adress + x.Adress, x.Coordinate, y.Coordinate, maxIterations)));




    public static IEnumerable<(int Adress, double Coordinate)>
  YCoordinates(int imageWidth, int imageHeight, double yMin, double yMax, int bytesPerPixel)
      => IndicesToCoordinates(imageWidth * bytesPerPixel, Step(imageHeight, yMin, yMax), imageHeight, yMin);


    public static (int Adress, double Coordinate)[]
  XCoordinates(int imageWidth, double xMin, double xMax, int bytesPerPixel)
      => IndicesToCoordinates(bytesPerPixel, Step(imageWidth, xMin, xMax), imageWidth, xMin).ToArray();


    public static double Step(int size, double min, double max) => (max - min) / (size - 1);


    public static IEnumerable<(int Adress, double Coordinate)>
  IndicesToCoordinates(int addressStep, double step, int imageSize, double min)
      => Enumerable.Range(0, imageSize).Select(i => (IndexToAddress(i, addressStep), IndexToCoordinate(i, min, step)));

    public static int IndexToAddress(int index, int addressStep) => index * addressStep;

    public static double IndexToCoordinate(int index, double min, double step) => min + index * step;

    public static void MandelbrotPixel(byte[] image, int address, double x, double y, int maxIterations)
    {
        var xVal = 0.0;
        var yVal = 0.0;
        var xQuad = 0.0;
        var yQuad = 0.0;
        int iteration = 0;
        do
        {
            yVal = 2 * xVal * yVal - y;
            xVal = xQuad - yQuad - x;
            xQuad = xVal * xVal;
            yQuad = yVal * yVal;
            iteration++;
        } while (iteration < maxIterations && xQuad + yQuad < 8);

        var original = iteration;

        var bytesPerPixel = BytesPerNumber(maxIterations);
        for (var i = bytesPerPixel - 1; i >= 0; i--)
        {
            image[address + i] = (byte)(iteration & 0xFF);
            iteration >>= 8;
        }
        if (address == 0)
        {
            var h = image[0];
            var l = image[1];
        }
    }
}