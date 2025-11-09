using MandelbrotsApple.Mandelbrot;

namespace MandelbrotsApple;

public interface IMandelbrotViewService
{
    MandelbrotResult Init(MandelbrotSize mandelbrotSize, int iterationPercentage, ImageSize imageSiz);

    MandelbrotResult Refresh(MandelbrotState state, ImageSize imageSize);

    MandelbrotResult MaxIterations(MandelbrotSize mandelbrotSize, int iterationPercentage, ImageSize imageSize);

    MandelbrotResult Move(MandelbrotState state, ImageVector imageMoveVector, ImageSize imageSize);

    MandelbrotResult Zoom(MandelbrotState state, bool zoomIn, int zoomCount, ImagePosition imagePosition, ImageSize imageSize);
}
