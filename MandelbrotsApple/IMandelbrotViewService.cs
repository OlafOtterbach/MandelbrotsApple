using MandelbrotsApple.Mandelbrot;

namespace MandelbrotsApple;

public interface IMandelbrotViewService
{
    MandelbrotResult InitialView(int iterationPercentage, int width, int height);

    MandelbrotResult ResizeView(MandelbrotState state, int width, int height);

    MandelbrotResult SetMaxIterations(MandelbrotSize mandelbrotSize, int iterationPercentage, int width, int height);

    MandelbrotResult MouseMove(MandelbrotState state, int x, int y, int width, int height);

    MandelbrotResult MouseWheel(MandelbrotState state, bool zoomIn, int zoomCount, int x, int y, int width, int height);
}
