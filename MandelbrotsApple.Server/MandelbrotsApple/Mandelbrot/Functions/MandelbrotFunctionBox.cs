namespace MandelbrotsApple.Mandelbrot.Functions;

using LaYumba.Functional;
using MandelbrotsApple.Mandelbrot.Model;

public static class MandelbrotFunctionBox
{
    public static byte[] MandelbrotSet(MandelbrotParameter parameter)
    {
        var canvas = CreateCanvas(parameter.Width, parameter.Height);
        MandelbrotSet(canvas, parameter.Width, parameter.Height, parameter.XMin, parameter.XMax, parameter.YMin, parameter.YMax, parameter.MaxIterations, XCoordinates(parameter.Width, parameter.XMin, parameter.XMax));
        return canvas;
    }


    public static byte[] CreateCanvas(int width, int height) => new byte[width * height * 3];


    private static void
    MandelbrotSet(byte[] canvas, int canvasWidth, int canvasHeight, double xMin, double xMax, double yMin, double yMax, int maxIterations, (int Adress, double Coordinate)[] xCoordinates)
        => YCoordinates(canvasWidth, canvasHeight, yMin, yMax)
           .AsParallel()
           .ForAll(y => xCoordinates
                        .ForEach(x => MandelbrotPixel(canvas, y.Adress + x.Adress, x.Coordinate, y.Coordinate, maxIterations)));




    public static IEnumerable<(int Adress, double Coordinate)>
    YCoordinates(int canvasWidth, int canvasHeight, double yMin, double yMax)
        => IndicesToCoordinates(canvasWidth * 3, Step(canvasHeight, yMin, yMax), canvasHeight, yMin);


    public static (int Adress, double Coordinate)[]
    XCoordinates(int canvasWidth, double xMin, double xMax)
        => IndicesToCoordinates(3, Step(canvasWidth, xMin, xMax), canvasWidth, xMin).ToArray();


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
        var iteration = 0;
        do
        {
            yVal = 2 * xVal * yVal - y;
            xVal = xQuad - yQuad - x;
            xQuad = xVal * xVal;
            yQuad = yVal * yVal;
            iteration++;
        } while (iteration < maxIterations && xQuad + yQuad < 8);

        IterationToColor(canvas, address, iteration, maxIterations);
    }
    

    private static void IterationToColor(byte[] canvas, int address, int iteration, int maxIteration)
    {
        var setColor = (byte red, byte green, byte blue) => SetColor(canvas, address, red, green, blue);

        if (iteration >= maxIteration)
        {
            setColor(0, 0, 0);
        }
        else
        {
            switch (iteration % 16 + 1)
            {
                case  1: setColor(000, 000, 200); break;
                case  2: setColor(000, 000, 218); break;
                case  3: setColor(000, 000, 236); break;
                case  4: setColor(000, 000, 255); break;

                case  5: setColor(000, 255, 000); break;
                case  6: setColor(000, 236, 000); break;
                case  7: setColor(000, 218, 000); break;
                case  8: setColor(000, 200, 000); break;

                case  9: setColor(200, 000, 000); break;
                case 10: setColor(218, 000, 000); break;
                case 11: setColor(236, 000, 000); break;
                case 12: setColor(255, 000, 000); break;

                case 13: setColor(255, 255, 000); break;
                case 14: setColor(236, 236, 000); break;
                case 15: setColor(218, 218, 000); break;
                case 16: setColor(200, 200, 000); break;
                default: setColor(000, 000, 000); break;
            }
        }
    }

    private static void SetColor(byte[] canvas, int address, byte red, byte green, byte blue)
    {
        canvas[address + 0] = red;
        canvas[address + 1] = green;
        canvas[address + 2] = blue;
    }
}
