namespace MandelbrotsApple.ExtendedMandelbrot;

using static Production;


public static class View
{
    public static MandelbrotResult Initialize(CanvasSize canvasSize)
    {
        var mandelbrotSize = new MandelbrotSize(new MandelbrotPosition(0.763, 0.0999), new MandelbrotPosition(0.768, 0.103));
        var parameter = new MandelbrotParameter(canvasSize, mandelbrotSize, 255);
        var result = GenerateMandelbrotSet(parameter);

        return result;
    }

    public static MandelbrotResult Refresh(MandelbrotParameter parameter)
        => GenerateMandelbrotSet(parameter);

    public static MandelbrotResult ZoomIn()
    {
        return new MandelbrotResult();
    }

    public static MandelbrotResult ZoomOut()
    {
        return new MandelbrotResult();
    }


    public static MandelbrotPosition MandelbrotPosition(CanvasPosition canvasPosition, CanvasSize canvasSize, MandelbrotSize mandelbrotSize) {
        var mandelbrotMin = mandelbrotSize.Min;
        var mandelbrotMax = mandelbrotSize.Max;
        var mandelbrotX = mandelbrotMin.X + canvasPosition.X * (mandelbrotMax.X - mandelbrotMin.X) / (canvasSize.Width - 1);
        var mandelbrotY = mandelbrotMin.Y + canvasPosition.Y * (mandelbrotMax.Y - mandelbrotMin.Y) / (canvasSize.Height - 1);

        return new MandelbrotPosition(mandelbrotX, mandelbrotY);
    }


    public static CanvasPosition CanvasPosition(MandelbrotPosition mandelbrotPosition, CanvasSize canvasSize, MandelbrotSize mandelbrotSize)
    {
        var mandelbrotMin = mandelbrotSize.Min;
        var mandelbrotMax = mandelbrotSize.Max;
        var canvasX = (mandelbrotPosition.X - mandelbrotMin.X) * (canvasSize.Width - 1) / (mandelbrotMax.X - mandelbrotMin.X);
        var canvasY = (mandelbrotPosition.Y - mandelbrotMin.Y) * (canvasSize.Height - 1) / (mandelbrotMax.Y - mandelbrotMin.Y);

        return new CanvasPosition(canvasX, canvasY);
    }
}
