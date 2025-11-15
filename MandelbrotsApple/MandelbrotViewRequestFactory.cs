namespace MandelbrotsApple;

using MandelbrotsApple.Mandelbrot;

public static class MandelbrotViewRequestFactory
{
    public static Func<MandelbrotState, MandelbrotResult> RequestInit(Init init)
        => state => View.Initialize(init.InitToParameter(state));

    public static Func<MandelbrotState, MandelbrotResult> RequestMaxIteration(MaxIteration maxIteration)
        => state => View.Refresh(maxIteration.MaxIterationToParameter(state));

    public static Func<MandelbrotState, MandelbrotResult> RequestRefresh(Refresh refresh)
        => state => View.Refresh(refresh.RefreshToParameter(state));

    public static Func<MandelbrotState, MandelbrotResult> RequestMove(Move move)
        => state => View.Move(move.MoveToMoveParameter(state));

    public static Func<MandelbrotState, MandelbrotResult> RequestRefresh(MoveLowAndFinalHigh move)
        => state => View.Refresh(move.MoveToParameter(state));

    public static Func<MandelbrotState, MandelbrotResult> RequestZoom(Zoom zoom)
        => state => View.Zoom(zoom.ZoomToZoomParameter(state));

    public static Func<MandelbrotState, MandelbrotResult> RequestRefresh(ZoomLowAndFinalHigh zoomLowAndFinalHigh)
        => state => View.Refresh(zoomLowAndFinalHigh.ZoomToParameter(state));




    private static MandelbrotParameter InitToParameter(this Init init, MandelbrotState state)
    {
        var maxIterationValue = GetMaxIteration(init.IterationPercentage);
        return new MandelbrotParameter(
            ImageSize: init.ImageSize,
            CurrentMandelbrotSize: init.MandelbrotSize,
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

    private static MandelbrotParameter MoveToParameter(this MoveLowAndFinalHigh move, MandelbrotState state)
    {
        return new MandelbrotParameter(
            ImageSize: move.ImageSizeHigh,
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

    private static MandelbrotParameter ZoomToParameter(this ZoomLowAndFinalHigh zoom, MandelbrotState state)
    {
        return new MandelbrotParameter(
            ImageSize: zoom.ImageSizeHigh,
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
