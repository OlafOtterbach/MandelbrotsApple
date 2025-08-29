using MandelbrotsApple.Mandelbrot;

namespace MandelbrotsApple;

public interface IMandelbrotViewService
{
    MandelbrotResult Init(double xmin, double ymin, double xmax, double ymax, int iterationPercentage, int width, int height);

    MandelbrotResult Resize(MandelbrotState state, int width, int height);

    MandelbrotResult MaxIterations(MandelbrotSize mandelbrotSize, int iterationPercentage, int width, int height);

    MandelbrotResult Move(MandelbrotState state, int x, int y, int width, int height);

    MandelbrotResult Zoom(MandelbrotState state, bool zoomIn, int zoomCount, int x, int y, int width, int height);
}
