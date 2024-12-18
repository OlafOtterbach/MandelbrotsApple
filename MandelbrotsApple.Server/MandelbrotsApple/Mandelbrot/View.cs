namespace MandelbrotsApple.Mandelbrot;

using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata;
using static Production;
using static System.Runtime.InteropServices.JavaScript.JSType;


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

    public static MandelbrotResult Zoom(MandelbrotZoomParameter zoomParameter)
         => GenerateMandelbrotSet(Zooming(zoomParameter));




    public static MandelbrotPosition MandelbrotPosition(CanvasPosition canvasPosition, CanvasSize canvasSize, MandelbrotSize mandelbrotSize)
    {
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

    private static MandelbrotParameter Zooming(MandelbrotZoomParameter zoomParameter)
    {
        var mousePosition = zoomParameter.MousePosition;
        var canvasSize = zoomParameter.CanvasSize;
        var mandelbrotSize = zoomParameter.CurrentMandelbrotSize;
        var zoomFactor = zoomParameter.Delta > 0 ? 1.001 : 0.999;

        var startMandelBrotPosition = MandelbrotPosition(mousePosition, canvasSize, mandelbrotSize);

        var xMin = mandelbrotSize.Min.X - startMandelBrotPosition.X;
        var yMin = mandelbrotSize.Min.Y - startMandelBrotPosition.Y;
        var xMax = mandelbrotSize.Max.X - startMandelBrotPosition.X;
        var yMax = mandelbrotSize.Max.Y - startMandelBrotPosition.Y;

        var newXMin = xMin * zoomFactor + startMandelBrotPosition.X;
        var newYMin = yMin * zoomFactor + startMandelBrotPosition.Y;
        var newXMax = xMax * zoomFactor + startMandelBrotPosition.X;
        var newYMax = yMax * zoomFactor + startMandelBrotPosition.Y;

        var zoomedMandelbrotSize = new MandelbrotSize(new MandelbrotPosition(newXMin, newYMin), new MandelbrotPosition(newXMax, newYMax));

        var zoomedMandelbrotParameter = new MandelbrotParameter(canvasSize, zoomedMandelbrotSize, zoomParameter.MaxIterations); 

        return zoomedMandelbrotParameter;
    }
}
