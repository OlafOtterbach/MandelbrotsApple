namespace MandelbrotsApple;

using MandelbrotsApple.Mandelbrot;

public interface IMandelbrotViewProxy
{
    IObservable<MandelbrotResult> DrawObservable { get; }

    void Init(Init init);

    void RefreshView(Refresh resize);

    void MaxIterations(MaxIteration iteration);

    void Move(MoveLowAndFinalHigh move);

    void Zoom(ZoomLowAndFinalHigh zoom);

    void Reset();
}
