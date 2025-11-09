namespace MandelbrotsApple.Mandelbrot;

using LaYumba.Functional;
using System.Diagnostics;
using static Production;

public static class View
{
    public static MandelbrotResult Initialize(MandelbrotSize mandelbrotSize, ImageSize imageSize, int maxIterations)
    {
        var parameter = new MandelbrotParameter(imageSize, mandelbrotSize, maxIterations);
        var result = GenerateMandelbrotImage(parameter);

        return result;
    }

    public static MandelbrotResult Refresh(MandelbrotParameter parameter)
        => GenerateMandelbrotImage(parameter);

    public static MandelbrotResult Zoom(MandelbrotZoomParameter zoomParameter)
    {
        var result = zoomParameter
                    .ValidateMandelbrotZoomParameter()
                    .Map(Zooming)
                    .GenerateMandelbrotImage();
        return result;
    }

    public static MandelbrotResult Move(MandelbrotMoveParameter moveParameter)
    {
        var result = moveParameter
                .ValidateMandelbrotMoveParameter()
                .Map(Moving)
                .GenerateMandelbrotImage();
        return result;
    }




    private static MandelbrotParameter Zooming(this MandelbrotZoomParameter zoomParameter)
    {
        var mousePosition = zoomParameter.MousePosition;
        var imageSize = zoomParameter.ImageSize;
        var mandelbrotSize = zoomParameter.CurrentMandelbrotSize;

        var singleZoomFactor = zoomParameter.ZoomIn ? 0.9 : 1.1;
        var zoomFactor = Math.Pow(singleZoomFactor, zoomParameter.zoomCount);

        var startMandelBrotPosition = MandelbrotPosition(mousePosition, imageSize, mandelbrotSize);

        var xMin = mandelbrotSize.Min.X - startMandelBrotPosition.X;
        var yMin = mandelbrotSize.Min.Y - startMandelBrotPosition.Y;
        var xMax = mandelbrotSize.Max.X - startMandelBrotPosition.X;
        var yMax = mandelbrotSize.Max.Y - startMandelBrotPosition.Y;

        var newXMin = xMin * zoomFactor + startMandelBrotPosition.X;
        var newYMin = yMin * zoomFactor + startMandelBrotPosition.Y;
        var newXMax = xMax * zoomFactor + startMandelBrotPosition.X;
        var newYMax = yMax * zoomFactor + startMandelBrotPosition.Y;

        var zoomedMandelbrotSize = new MandelbrotSize(new MandelbrotPosition(newXMin, newYMin), new MandelbrotPosition(newXMax, newYMax));

        var zoomedMandelbrotParameter = new MandelbrotParameter(imageSize, zoomedMandelbrotSize, zoomParameter.MaxIterations);

        return zoomedMandelbrotParameter;
    }

    public static MandelbrotParameter Moving(this MandelbrotMoveParameter moveParameter)
    {
        var imageSize = moveParameter.ImageSize;
        var mandelbrotSize = moveParameter.CurrentMandelbrotSize;
        var imageMoveVector = moveParameter.ImageMoveVector;

        var mandelbrotMoveVector = MandelbrotVector(imageMoveVector, imageSize, mandelbrotSize);

        var newXMin = mandelbrotSize.Min.X - mandelbrotMoveVector.Vx;
        var newYMin = mandelbrotSize.Min.Y - mandelbrotMoveVector.Vy;
        var newXMax = mandelbrotSize.Max.X - mandelbrotMoveVector.Vx;
        var newYMax = mandelbrotSize.Max.Y - mandelbrotMoveVector.Vy;

        var movedMandelbrotSize = new MandelbrotSize(new MandelbrotPosition(newXMin, newYMin), new MandelbrotPosition(newXMax, newYMax));
        var movedMandelbrotParameter = new MandelbrotParameter(imageSize, movedMandelbrotSize, moveParameter.MaxIterations);

        return movedMandelbrotParameter;
    }



    private static MandelbrotPosition MandelbrotPosition(ImagePosition imagePosition, ImageSize imageSize, MandelbrotSize mandelbrotSize)
    {
        // aspect ratio difference to squared image in y direction
        var diff = imageSize.Width - imageSize.Height;
        var virtualImageHeight = imageSize.Height + diff;
        var virtualImageYPosition = imagePosition.Y + diff / 2.0;

        var mandelbrotMin = mandelbrotSize.Min;
        var mandelbrotMax = mandelbrotSize.Max;
        var mandelbrotX = mandelbrotMin.X + imagePosition.X  * (mandelbrotMax.X - mandelbrotMin.X) / (imageSize.Width - 1);
        var mandelbrotY = mandelbrotMin.Y + virtualImageYPosition * (mandelbrotMax.Y - mandelbrotMin.Y) / (virtualImageHeight - 1);

        return new MandelbrotPosition(mandelbrotX, mandelbrotY);
    }

    private static MandelbrotVector MandelbrotVector(ImageVector imageVector, ImageSize imageSize, MandelbrotSize mandelbrotSize)
    {
        // aspect ratio difference to squared image in y direction
        var diff = imageSize.Width - imageSize.Height;
        var virtualImageHeight = imageSize.Height + diff;

        var mandelbrotMin = mandelbrotSize.Min;
        var mandelbrotMax = mandelbrotSize.Max;
        var mandelbrotVx = imageVector.Vx * (mandelbrotMax.X - mandelbrotMin.X) / (imageSize.Width - 1);
        var mandelbrotVy = imageVector.Vy * (mandelbrotMax.Y - mandelbrotMin.Y) / (virtualImageHeight - 1);

        return new MandelbrotVector(mandelbrotVx, mandelbrotVy);
    }
}
