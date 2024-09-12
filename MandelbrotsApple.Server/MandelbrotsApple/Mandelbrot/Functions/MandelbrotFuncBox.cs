namespace MandelbrotsApple.Mandelbrot.Functions;

using MandelbrotsApple.Mandelbrot.Model;

public static class MandelbrotFuncBox
{
    public static IEnumerable<byte> MandelbrotSet(MandelbrotParameter parameter)
        => MandelbrotSet(parameter.Width, parameter.Height, parameter.XMin, parameter.XMax, parameter.YMin, parameter.YMax, parameter.MaxIterations, XCoordinates(parameter.Width, parameter.XMin, parameter.XMax));

    public static IEnumerable<byte> MandelbrotSet(int canvasWidth, int canvasHeight, double xMin, double xMax, double yMin, double yMax, int maxIterations, Func<IEnumerable<double>> xCoordinates)
        => YCoordinates(canvasHeight, yMin, yMax).SelectMany(y => xCoordinates().SelectMany(x => MandelbrotPixel(x, y, maxIterations)));

    public static IEnumerable<byte> MandelbrotPixel(double x, double y, int maxIterations)
        => MandelbrotPixelRecursive(x, y, 0.0, 0.0, maxIterations, 0);

    private static IEnumerable<byte> MandelbrotPixelRecursive(double x, double y, double xVal, double yVal, int maxIterations, int iteration)
    {
        var xQuad = xVal * xVal;
        var yQuad = yVal * yVal;

        if (iteration >= maxIterations || xQuad + yQuad >= 8)
            return IterationToColor(iteration);

        var newYVal = 2 * xVal * yVal - y;
        var newXVal = xQuad - yQuad - x;

        return MandelbrotPixelRecursive(x, y, newXVal, newYVal, maxIterations, iteration + 1);
    }

    private static IEnumerable<byte> IterationToColor(int iteration)
        => (iteration % 4) switch
        {
            1 => new byte[] { 255, 255, 0, 0 },
            2 => new byte[] { 255, 0, 255, 0 },
            3 => new byte[] { 255, 0, 0, 255 },
            _ => new byte[] { 255, 0, 0, 0 }
        };

    public static Func<IEnumerable<double>> XCoordinates(int canvasWidth, double xMin, double xMax)
    {
        double[] _xCoordinates = null;
        return () =>
        {
            if(_xCoordinates == null)
                _xCoordinates = Coordinates(canvasWidth, xMin, xMax).ToArray();

            return _xCoordinates;
        };
    }

    public static IEnumerable<double> YCoordinates(int canvasHeight, double yMin, double yMax) => Coordinates(canvasHeight, yMin, yMax);

    private static IEnumerable<double> Coordinates(int canvasSize, double min, double max) => IndicesToCoordinates(Step(canvasSize, min, max), canvasSize, min);

    public static IEnumerable<double> IndicesToCoordinates(double step, int canvasSize, double min) => Enumerable.Range(0, canvasSize).Select(i => IndexToCoordinate(i, min, step));

    public static double IndexToCoordinate(int index, double min, double step) => min + index * step;

    public static double Step(int size, double min, double max) => (max - min) / (size - 1);
}
