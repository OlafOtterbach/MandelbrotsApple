namespace MandelbrotsApple;

using MandelbrotsApple.Mandelbrot;

public interface IMandelbrotViewServiceProxy
{
    IObservable<MandelbrotResult> DrawObservable { get; }

    MandelbrotResult InitialView(int width, int height);

    void ResizeView(int width, int height);

    void SetResolution(int resolutionPercentage, int width, int height);

    void SetMaxIterations(int iterationPercentage);

    void Reset();

    void SetMouseStart(int x, int y);

    void MouseMove(MoveEvent moveEvent);

    void MouseWheel(bool zoomIn, int zoomCount, int x, int y);
}
