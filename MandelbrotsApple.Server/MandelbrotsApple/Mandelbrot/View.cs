namespace MandelbrotsApple.Mandelbrot;

using LaYumba.Functional;
using static Production;

public static class View
{
    public static MandelbrotResult Initialize(CanvasSize canvasSize)
    {
        //var mandelbrotSize = new MandelbrotSize(new MandelbrotPosition(0.763, 0.0999), new MandelbrotPosition(0.768, 0.103));
        var mandelbrotSize = new MandelbrotSize(new MandelbrotPosition(-2.0, -2.0), new MandelbrotPosition(2.0, 2.0));
        var parameter = new MandelbrotParameter(canvasSize, mandelbrotSize, 255);
        var result = GenerateMandelbrotSet(parameter);

        return result;
    }

    public static MandelbrotResult Refresh(MandelbrotParameter parameter)
        => GenerateMandelbrotSet(parameter);

    public static MandelbrotResult Zoom(MandelbrotZoomParameter zoomParameter)
    {
        var result = zoomParameter
                    .ValidateMandelbrotZoomParameter()
                    .Map(Zooming)
                    .GenerateMandelbrotSet();
        return result;
    }

    public static MandelbrotResult Move(MandelbrotMoveParameter moveParameter)
    {
        var result = moveParameter
                .ValidateMandelbrotMoveParameter()
                .Map(Moving)
                .GenerateMandelbrotSet();
        return result;
    }




    private static MandelbrotParameter Zooming(this MandelbrotZoomParameter zoomParameter)
    {
        var mousePosition = zoomParameter.MousePosition;
        var canvasSize = zoomParameter.CanvasSize;
        var mandelbrotSize = zoomParameter.CurrentMandelbrotSize;
        var zoomFactor = zoomParameter.ZoomIn ? 0.9 : 1.1;

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

    public static MandelbrotParameter Moving(this MandelbrotMoveParameter moveParameter)
    {
        var canvasVector = moveParameter.MouseVector;
        var canvasSize = moveParameter.CanvasSize;
        var mandelbrotSize = moveParameter.CurrentMandelbrotSize;

        var mandelbrotVector = MandelbrotVector(canvasVector, canvasSize, mandelbrotSize);

        var newXMin = mandelbrotSize.Min.X - mandelbrotVector.Vx;
        var newYMin = mandelbrotSize.Min.Y - mandelbrotVector.Vy;
        var newXMax = mandelbrotSize.Max.X - mandelbrotVector.Vx;
        var newYMax = mandelbrotSize.Max.Y - mandelbrotVector.Vy;

        Console.WriteLine($"OldMin({mandelbrotSize.Min.X})");
        Console.WriteLine($"NewMin({newXMin})");
        Console.WriteLine();


        var movedMandelbrotSize = new MandelbrotSize(new MandelbrotPosition(newXMin, newYMin), new MandelbrotPosition(newXMax, newYMax));

        var movedMandelbrotParameter = new MandelbrotParameter(canvasSize, movedMandelbrotSize, moveParameter.MaxIterations);

        return movedMandelbrotParameter;
    }



    public static MandelbrotPosition MandelbrotPosition(CanvasPosition canvasPosition, CanvasSize canvasSize, MandelbrotSize mandelbrotSize)
    {
        var mandelbrotMin = mandelbrotSize.Min;
        var mandelbrotMax = mandelbrotSize.Max;
        var mandelbrotX = mandelbrotMin.X + canvasPosition.X * (mandelbrotMax.X - mandelbrotMin.X) / (canvasSize.Width - 1);
        var mandelbrotY = mandelbrotMin.Y + canvasPosition.Y * (mandelbrotMax.Y - mandelbrotMin.Y) / (canvasSize.Height - 1);

        return new MandelbrotPosition(mandelbrotX, mandelbrotY);
    }

    public static MandelbrotVector MandelbrotVector(CanvasVector canvasVector, CanvasSize canvasSize, MandelbrotSize mandelbrotSize)
    {
        var mandelbrotMin = mandelbrotSize.Min;
        var mandelbrotMax = mandelbrotSize.Max;
        var mandelbrotVx = canvasVector.Vx * (mandelbrotMax.X - mandelbrotMin.X) / (canvasSize.Width - 1);
        var mandelbrotVy = canvasVector.Vy * (mandelbrotMax.Y - mandelbrotMin.Y) / (canvasSize.Height - 1);

        return new MandelbrotVector(mandelbrotVx, mandelbrotVy);
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
