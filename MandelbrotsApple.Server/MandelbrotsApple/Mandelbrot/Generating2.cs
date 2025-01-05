namespace MandelbrotsApple.Mandelbrot;

using LaYumba.Functional;

public class Generating2
{
    public static byte[] MandelbrotImage(MandelbrotParameter parameter)
    {
        var canvas = CreateCanvas(parameter.CanvasSize);
        MandelbrotSet(
            canvas,
            parameter.CanvasSize.Width,
            parameter.CanvasSize.Height,
            parameter.CurrentMandelbrotSize.Min.X,
            parameter.CurrentMandelbrotSize.Max.X,
            parameter.CurrentMandelbrotSize.Min.Y,
            parameter.CurrentMandelbrotSize.Max.Y,
            parameter.MaxIterations,
            XCoordinates(parameter.CanvasSize.Width, parameter.CurrentMandelbrotSize.Min.X, parameter.CurrentMandelbrotSize.Max.X));
        return canvas;
    }


    private static byte[] CreateCanvas(CanvasSize canvasSize) => new byte[canvasSize.Width * canvasSize.Height];


      private static void
    MandelbrotSet(byte[] canvas, int canvasWidth, int canvasHeight, double xMin, double xMax, double yMin, double yMax, int maxIterations, (int Adress, double Coordinate)[] xCoordinates)
        => YCoordinates(canvasWidth, canvasHeight, yMin, yMax)
           .AsParallel()
           .ForAll(y => xCoordinates
                        .ForEach(x => MandelbrotPixel(canvas, y.Adress + x.Adress, x.Coordinate, y.Coordinate, maxIterations)));




      public static IEnumerable<(int Adress, double Coordinate)>
    YCoordinates(int canvasWidth, int canvasHeight, double yMin, double yMax)
        => IndicesToCoordinates(canvasWidth, Step(canvasHeight, yMin, yMax), canvasHeight, yMin);


      public static (int Adress, double Coordinate)[]
    XCoordinates(int canvasWidth, double xMin, double xMax)
        => IndicesToCoordinates(1, Step(canvasWidth, xMin, xMax), canvasWidth, xMin).ToArray();


    public static double Step(int size, double min, double max) => (max - min) / (size - 1);


      public static IEnumerable<(int Adress, double Coordinate)>
    IndicesToCoordinates(int addressStep, double step, int canvasSize, double min)
        => Enumerable.Range(0, canvasSize).Select(i => (IndexToAddress(i, addressStep), IndexToCoordinate(i, min, step)));

    public static int IndexToAddress(int index, int addressStep) => index * addressStep;

    public static double IndexToCoordinate(int index, double min, double step) => min + index * step;

    public static void MandelbrotPixel(byte[] canvas, int address, double x, double y, int maxIterations)
    {
        var xVal = 0.0;
        var yVal = 0.0;
        var xQuad = 0.0;
        var yQuad = 0.0;
        byte iteration = 0;
        do
        {
            yVal = 2 * xVal * yVal - y;
            xVal = xQuad - yQuad - x;
            xQuad = xVal * xVal;
            yQuad = yVal * yVal;
            iteration++;
        } while (iteration < maxIterations && xQuad + yQuad < 8);

        canvas[address] = iteration;
    }
}