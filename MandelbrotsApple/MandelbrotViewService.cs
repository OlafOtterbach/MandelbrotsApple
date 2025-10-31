namespace MandelbrotsApple;

using MandelbrotsApple.Mandelbrot;
using static MandelbrotsApple.Mandelbrot.View;

public class MandelbrotViewService : IMandelbrotViewService
{
    public MandelbrotResult Init(double xmin, double ymin, double xmax, double ymax, int iterationPercentage, int width, int height)
    {
        var maxIterations = GetMaxIteration(iterationPercentage);
        var result = Initialize(new MandelbrotSize(new MandelbrotPosition(xmin, ymin), new MandelbrotPosition(xmax, ymax)), new ImageSize(width, height), maxIterations);
        return result;
    }

    public MandelbrotResult Refresh(MandelbrotState state, int width, int height)
    {
        var mandelbrotParameter = new MandelbrotParameter(new ImageSize(width, height), state.Size, state.MaxIterations);
        var result = View.Refresh(mandelbrotParameter);
        return result;
    }

    public MandelbrotResult MaxIterations(MandelbrotSize mandelbrotSize, int iterationPercentage, int width, int height)
    {
        var maxIterations = GetMaxIteration(iterationPercentage);
        var mandelbrotParameter = new MandelbrotParameter(new ImageSize(width, height), mandelbrotSize, maxIterations);
        var result = View.Refresh(mandelbrotParameter);
        return result;
    }

    public MandelbrotResult Move(MandelbrotState state, MandelbrotPosition mandelbrotMovePosition, int width, int height)
    {
        var mandelbrotMoveParameter = new MandelbrotMoveParameter(
            mandelbrotMovePosition,
            new ImageSize(width, height),
            state.Size,
            state.MaxIterations);

        var result = View.Move(mandelbrotMoveParameter);

        return result;
    }

    public MandelbrotResult Zoom(MandelbrotState state, bool zoomIn, int zoomCount, int x, int y, int width, int height)
    {
        var mandelbrotZoomParameter = new MandelbrotZoomParameter(
            new ImagePosition(x, y),
            zoomIn,
            zoomCount,
            new ImageSize(width, height),
            state.Size,
            state.MaxIterations);

        var result = View.Zoom(mandelbrotZoomParameter);
        
        return result;
    }


    private static int GetMaxIteration(int percentage)
    {
        const double min = 255;
        const double max = 4096;
        return (int)(min + (max - min) * (percentage / 100.0));
    }
}
