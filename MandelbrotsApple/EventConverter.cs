namespace MandelbrotsApple;

using MandelbrotsApple.Mandelbrot;
using static MandelbrotsApple.Mandelbrot.View;

public static class EventConverter
{
    public static Func<MandelbrotState, MandelbrotResult> CreateInit(this Init init)
        => state => View.Initialize(init.InitToParameter(state));

    public static Func<MandelbrotState, MandelbrotResult> CreateMaxIteration(this MaxIteration maxIteration)
        => state => View.Refresh(maxIteration.MaxIterationToParameter(state));

    public static Func<MandelbrotState, MandelbrotResult> CreateRefresh(this Refresh refresh)
        => state => View.Refresh(refresh.RefreshToParameter(state));

    public static Func<MandelbrotState, MandelbrotResult> CreateMove(this Move move)
        => state => View.Move(move.MoveToMoveParameter(state));

    public static Func<MandelbrotState, MandelbrotResult> CreateRefresh(this Move move)
        => state => View.Refresh(move.MoveToParameter(state));

    public static Func<MandelbrotState, MandelbrotResult> CreateZoom(this Zoom zoom)
        => state => View.Zoom(zoom.ZoomToZoomParameter(state));





    private static MandelbrotParameter InitToParameter(this Init init, MandelbrotState state)
    {
        var maxIterationValue = GetMaxIteration(init.IterationPercentage);
        return new MandelbrotParameter(
            ImageSize: init.ImageSize,
            CurrentMandelbrotSize: state.Size,
            MaxIterations: maxIterationValue);
    }

    private static MandelbrotParameter MaxIterationToParameter(this MaxIteration maxIteration, MandelbrotState state)
    {
        var maxIterationValue = GetMaxIteration(maxIteration.IterationPercentage);
        return new MandelbrotParameter(
            ImageSize: maxIteration.ImageSize,
            CurrentMandelbrotSize: state.Size,
            MaxIterations: maxIterationValue);
    }

    private static MandelbrotParameter RefreshToParameter(this Refresh refresh, MandelbrotState state)
    {
        return new MandelbrotParameter(
            ImageSize: refresh.ImageSize,
            CurrentMandelbrotSize: state.Size,
            MaxIterations: state.MaxIterations);
    }

    private static MandelbrotParameter MoveToParameter(this Move move, MandelbrotState state)
    {
        return new MandelbrotParameter(
            ImageSize: move.ImageSize,
            CurrentMandelbrotSize: state.Size,
            MaxIterations: state.MaxIterations);
    }

    private static MandelbrotMoveParameter MoveToMoveParameter(this Move move, MandelbrotState state)
    {
        return new MandelbrotMoveParameter(
           ImageMoveVector: move.ImageMoveVector,
           ImageSize: move.ImageSize,
           CurrentMandelbrotSize: state.Size,
           MaxIterations: state.MaxIterations);
    }

    private static MandelbrotZoomParameter ZoomToZoomParameter(this Zoom zoom, MandelbrotState state)
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
