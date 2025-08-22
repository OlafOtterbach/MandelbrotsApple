using MandelbrotsApple.Mandelbrot;

namespace MandelbrotsApple;

public interface IMandelbrotViewService
{
    MandelbrotResult InitialView(int width, int height);

    MandelbrotResult ResizeView(int width, int height);

    MandelbrotResult SetResolution(int resolutionPercentage, int width, int height);

    MandelbrotResult SetMaxIterations(int iterationPercentage);

    MandelbrotResult MouseMove(int x, int y, int width, int height);

    MandelbrotResult MouseWheel(bool zoomIn, int zoomCount, int x, int y, int width, int height);
}
