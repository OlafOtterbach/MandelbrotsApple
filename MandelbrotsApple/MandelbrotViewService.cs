namespace MandelbrotsApple;

using MandelbrotsApple.Mandelbrot;
using static MandelbrotsApple.Mandelbrot.View;

public class MandelbrotViewService : IMandelbrotViewService
{
    private int _maxIterations = 255; // Default max iterations for Mandelbrot calculation
    private int _imageWidth = 256; // Default width
    private int _imageHeight = 256; // Default height
    private MandelbrotSize _mandelbrotSize = MandelbrotSize.Empty;

    public MandelbrotResult InitialView(int width, int height)
    {
        _imageWidth = width;
        _imageHeight = height;
        var result = Initialize(new ImageSize(width, height), _maxIterations);
        _mandelbrotSize = result.MandelbrotSize;
        return result;
    }

    public MandelbrotResult ResizeView(int width, int height)
    {
        _imageWidth = width;
        _imageHeight = height;
        var mandelbrotParameter = new MandelbrotParameter(new ImageSize(width, height), _mandelbrotSize, _maxIterations);
        var result = Refresh(mandelbrotParameter);
        _mandelbrotSize = result.MandelbrotSize;
        return result;
    }

    public MandelbrotResult SetMaxIterations(int iterationPercentage)
    {
        _maxIterations = GetMaxIteration(iterationPercentage);
        var mandelbrotParameter = new MandelbrotParameter(new ImageSize(_imageWidth, _imageHeight), _mandelbrotSize, _maxIterations);
        var result = Refresh(mandelbrotParameter);
        _mandelbrotSize = result.MandelbrotSize;
        return result;
    }

    public MandelbrotResult MouseMove(int vx, int vy, int width, int height)
    {
        var mandelbrotMoveParameter = new MandelbrotMoveParameter(
            new ImageVector(vx, vy),
            new ImageSize(width, height),
            _mandelbrotSize,
            _maxIterations);

        var result = Move(mandelbrotMoveParameter);

        if (!result.HasErrors)
        {
            _mandelbrotSize = result.MandelbrotSize;
        }

        return result;
    }

    public MandelbrotResult MouseWheel(bool zoomIn, int zoomCount, int x, int y, int width, int height)
    {
        var mandelbrotZoomParameter = new MandelbrotZoomParameter(
            new ImagePosition(x, y),
            zoomIn,
            zoomCount,
            new ImageSize(width, height),
            _mandelbrotSize,
            _maxIterations);

        var result = Zoom(mandelbrotZoomParameter);
        
        if(!result.HasErrors)
        {
            _mandelbrotSize = result.MandelbrotSize;
        }

        return result;
    }


    private static int GetMaxIteration(int percentage)
    {
        const double min = 255;
        const double max = 4096;
        return (int)(min + (max - min) * (percentage / 100.0));
    }
}
