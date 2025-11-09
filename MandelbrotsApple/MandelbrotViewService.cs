namespace MandelbrotsApple;

using MandelbrotsApple.Mandelbrot;
using static MandelbrotsApple.Mandelbrot.View;

public class MandelbrotViewService : IMandelbrotViewService
{
    public MandelbrotResult Init(MandelbrotSize mandelbrotSize, int iterationPercentage, ImageSize imageSize)
    {
        var maxIterations = GetMaxIteration(iterationPercentage);
        var result = Initialize(mandelbrotSize, imageSize, maxIterations);
        return result;
    }

    public MandelbrotResult Refresh(MandelbrotState state, ImageSize imageSize)
    {
        var mandelbrotParameter = new MandelbrotParameter(imageSize, state.Size, state.MaxIterations);
        var result = View.Refresh(mandelbrotParameter);
        return result;
    }

    public MandelbrotResult MaxIterations(MandelbrotSize mandelbrotSize, int iterationPercentage, ImageSize imageSize)
    {
        var maxIterations = GetMaxIteration(iterationPercentage);
        var mandelbrotParameter = new MandelbrotParameter(imageSize, mandelbrotSize, maxIterations);
        var result = View.Refresh(mandelbrotParameter);
        return result;
    }

    public MandelbrotResult Move(MandelbrotState state, ImageVector imageMoveVector, ImageSize imageSize)
    {
        var mandelbrotMoveParameter = new MandelbrotMoveParameter(
            imageMoveVector,
            imageSize,
            state.Size,
            state.MaxIterations);

        var result = View.Move(mandelbrotMoveParameter);

        return result;
    }

    public MandelbrotResult Zoom(MandelbrotState state, bool zoomIn, int zoomCount, ImagePosition imagePosition, ImageSize imageSize)
    {
        var mandelbrotZoomParameter = new MandelbrotZoomParameter(
            imagePosition,
            zoomIn,
            zoomCount,
            imageSize,
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
