namespace MandelbrotsApple;

using MandelbrotsApple.Mandelbrot;
using static MandelbrotsApple.Mandelbrot.View;

public class MandelbrotViewService : IMandelbrotViewService
{
    public MandelbrotResult InitialView(int iterationPercentage, int width, int height)
    {
        var maxIterations = GetMaxIteration(iterationPercentage);
        var result = Initialize(new ImageSize(width, height), maxIterations);
        return result;
    }

    public MandelbrotResult ResizeView(MandelbrotState state, int width, int height)
    {
        var mandelbrotParameter = new MandelbrotParameter(new ImageSize(width, height), state.Size, state.MaxIterations);
        var result = Refresh(mandelbrotParameter);
        return result;
    }

    public MandelbrotResult SetMaxIterations(MandelbrotSize mandelbrotSize, int iterationPercentage, int width, int height)
    {
        var maxIterations = GetMaxIteration(iterationPercentage);
        var mandelbrotParameter = new MandelbrotParameter(new ImageSize(width, height), mandelbrotSize, maxIterations);
        var result = Refresh(mandelbrotParameter);
        return result;
    }

    public MandelbrotResult MouseMove(MandelbrotState state, int vx, int vy, int width, int height)
    {
        var mandelbrotMoveParameter = new MandelbrotMoveParameter(
            new ImageVector(vx, vy),
            new ImageSize(width, height),
            state.Size,
            state.MaxIterations);

        var result = Move(mandelbrotMoveParameter);

        return result;
    }

    public MandelbrotResult MouseWheel(MandelbrotState state, bool zoomIn, int zoomCount, int x, int y, int width, int height)
    {
        var mandelbrotZoomParameter = new MandelbrotZoomParameter(
            new ImagePosition(x, y),
            zoomIn,
            zoomCount,
            new ImageSize(width, height),
            state.Size,
            state.MaxIterations);

        var result = Zoom(mandelbrotZoomParameter);
        
        return result;
    }


    private static int GetMaxIteration(int percentage)
    {
        const double min = 255;
        const double max = 4096;
        return (int)(min + (max - min) * (percentage / 100.0));
    }
}
