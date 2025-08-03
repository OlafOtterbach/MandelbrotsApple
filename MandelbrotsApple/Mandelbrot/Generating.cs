namespace MandelbrotsApple.Mandelbrot;

using LaYumba.Functional;

public class Generating
{
    public static ParallelQuery<IterationPixel> MandelbrotSet(MandelbrotParameter parameter)
    {
        // aspect ratio difference to squared image in y direction
        var aspectRaitio = parameter.ImageSize.Width / (double)parameter.ImageSize.Height;
        var ymin = parameter.CurrentMandelbrotSize.Min.Y;
        var ymax = parameter.CurrentMandelbrotSize.Max.Y;
        var height = ymax - ymin;
        var diff = (height - (height / aspectRaitio)) / 2.0;
        ymin += diff;
        ymax -= diff;

        // generatiing mandelbrot pixels
        var result = MandelbrotSet(
            parameter.ImageSize.Width,
            parameter.ImageSize.Height,
            parameter.CurrentMandelbrotSize.Min.X,
            ymin,
            parameter.CurrentMandelbrotSize.Max.X,
            ymax,
            parameter.MaxIterations,
            XCoordinates(parameter.ImageSize.Width, parameter.CurrentMandelbrotSize.Min.X, parameter.CurrentMandelbrotSize.Max.X));

        return result;
    }


    private static ParallelQuery<IterationPixel>
    MandelbrotSet(int imageWidth, int imageHeight, double xMin, double yMin, double xMax, double yMax, int maxIterations, (int ImagePos, double MandelbrotPos)[] xCoordinates)
        => YCoordinates(imageHeight, yMin, yMax)
           .AsParallel()
           .SelectMany(y => xCoordinates.Select(x => MandelbrotPixel(x.ImagePos, y.ImagePos, x.MandelbrotPos, y.MandelbrotPos, maxIterations)));

    public static IEnumerable<(int ImagePos, double MandelbrotPos)>
    YCoordinates(int imageHeight, double yMin, double yMax) => ImageSizeToMandelbrotPositions(imageHeight, yMin, Step(imageHeight, yMin, yMax));


    public static (int ImagePos, double MandelbrotPos)[]
    XCoordinates(int imageWidth, double xMin, double xMax) => ImageSizeToMandelbrotPositions(imageWidth, xMin, Step(imageWidth, xMin, xMax)).ToArray();


    public static double
    Step(int size, double min, double max) => (max - min) / (size - 1);


    public static IEnumerable<(int imagePos, double mandelbrotPos)>
    ImageSizeToMandelbrotPositions(int imageSize, double mandelbrotMin, double step) => Enumerable.Range(0, imageSize).Select(imagePos => (imagePos, ImagePositionToMandelbrotPosition(imagePos, mandelbrotMin, step)));


    public static double
    ImagePositionToMandelbrotPosition(int imagePos, double mandelbrotMin, double step) => mandelbrotMin + imagePos * step;


    public static IterationPixel MandelbrotPixel(int xImage, int yImage, double x, double y, int maxIterations)
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

        return new IterationPixel(xImage, yImage, iteration);
    }
}