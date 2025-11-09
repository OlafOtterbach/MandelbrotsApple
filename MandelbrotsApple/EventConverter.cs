using MandelbrotsApple.Mandelbrot;

namespace MandelbrotsApple;

public static class EventConverter
{
    public static MandelbrotParameter Convert(this Init init, MandelbrotState state)
    {
        var maxIterationValue = GetMaxIteration(init.IterationPercentage);
        return new MandelbrotParameter(
            ImageSize: init.ImageSize,
            CurrentMandelbrotSize: state.Size,
            MaxIterations: maxIterationValue);
    }

    public static MandelbrotParameter Convert(this MaxIteration maxIteration, MandelbrotState state)
    {
        var maxIterationValue = GetMaxIteration(maxIteration.IterationPercentage);
        return new MandelbrotParameter(
            ImageSize: maxIteration.ImageSize,
            CurrentMandelbrotSize: state.Size,
            MaxIterations: maxIterationValue);
    }

    public static MandelbrotParameter Convert(this Refresh refresh, MandelbrotState state)
    {
        return new MandelbrotParameter(
            ImageSize: refresh.ImageSize,
            CurrentMandelbrotSize: state.Size,
            MaxIterations: state.MaxIterations);
    }

    public static MandelbrotMoveParameter Convert(this Move move, MandelbrotState state)
    {
        return new MandelbrotMoveParameter(
           ImageMoveVector: move.ImageMoveVector,
           ImageSize: move.ImageSize,
           CurrentMandelbrotSize: state.Size,
           MaxIterations: state.MaxIterations);
    }

    public static MandelbrotZoomParameter Convert(this Zoom zoom, MandelbrotState state)
    {
        return new MandelbrotZoomParameter(
            MousePosition: zoom.ImagePosition,
            ZoomIn: zoom.ZoomIn,
            zoomCount: zoom.ZoomCount,
            ImageSize: zoom.ImageSize,
            CurrentMandelbrotSize: state.Size,
            MaxIterations: state.MaxIterations);
    }

    private static int GetMaxIteration(int percentage)
    {
        const double min = 255;
        const double max = 4096;
        return (int)(min + (max - min) * (percentage / 100.0));
    }
}
