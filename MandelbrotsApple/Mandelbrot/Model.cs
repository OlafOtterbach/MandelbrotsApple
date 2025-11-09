namespace MandelbrotsApple.Mandelbrot;

using LaYumba.Functional;

public readonly record struct IterationPixel(int X, int Y, int IterationPixelValue);

public readonly record struct ImagePosition(double X, double Y);

public readonly record struct ImageVector(double Vx, double Vy);

public readonly record struct ImageSize(int Width, int Height);

public readonly record struct MandelbrotPosition(double X, double Y);

public readonly record struct MandelbrotVector(double Vx, double Vy);

public readonly record struct MandelbrotSize(MandelbrotPosition Min, MandelbrotPosition Max)
{
    public static MandelbrotSize Empty => new MandelbrotSize(new MandelbrotPosition(0, 0), new MandelbrotPosition(0, 0));
}



public readonly record struct MandelbrotState(MandelbrotSize Size, int MaxIterations)
{
    public static MandelbrotState Empty => new MandelbrotState(MandelbrotSize.Empty, 255);
}

public static class MandelbrotStateExtensions
{
    public static MandelbrotState SetSize(this MandelbrotState state, MandelbrotSize size) =>
        state with { Size = size };

    public static MandelbrotState SetMaxIterations(this MandelbrotState state, int maxIterations) =>
        state with { MaxIterations = maxIterations };
}


public readonly record struct MandelbrotParameter(ImageSize ImageSize, MandelbrotSize CurrentMandelbrotSize, int MaxIterations);

public readonly record struct MandelbrotZoomParameter(ImagePosition MousePosition, bool ZoomIn, int zoomCount, ImageSize ImageSize, MandelbrotSize CurrentMandelbrotSize, int MaxIterations);

public readonly record struct MandelbrotMoveParameter(ImageVector ImageMoveVector, ImageSize ImageSize, MandelbrotSize CurrentMandelbrotSize, int MaxIterations);

public readonly record struct MandelbrotResult(byte[] ImageData, ImageSize ImageSize, MandelbrotSize MandelbrotSize, int MaxIterations, ErrorType[] Errors, bool HasErrors);


public enum ErrorType
{
    None = 0,
    ParameterIsNull = 1,
    WidthIsLessThanOnePixelError = 2,
    HeightIsLessThanOnePixelError = 3,
    WidthIsGreaterThan10000PixelError = 4,
    HeightIsGreaterThan10000PixelError = 5,
    XMinIsGreaterThanXMaxError = 6,
    YMinIsGreaterThanYMaxError = 7,
    XMinAndXMaxDifferenceToSmall = 8,
    YMinAndYMaxDifferenceToSmall = 9,
    IterationLessOrEqualThanZeroError = 10,
    IterationGreaterThanThousandError = 11,
}

public record ValidationError(ErrorType ErrorType) : Error(ErrorType.ToString());

