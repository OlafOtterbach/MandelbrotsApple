namespace MandelbrotsApple;

using MandelbrotsApple.Mandelbrot;

public interface IMandelbrotViewServiceProxy
{
    IObservable<MandelbrotResult> DrawObservable { get; }

    void Init(Init init);

    void ResizeView(Resize resize);

    void MaxIterations(MaxIteration iteration);

    void Move(MoveLowAndFinalHigh move);

    void Zoom(ZoomLowAndHigh zoom);

    void Reset();
}
