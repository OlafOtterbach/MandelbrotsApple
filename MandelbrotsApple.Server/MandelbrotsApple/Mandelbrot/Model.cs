namespace MandelbrotsApple.Mandelbrot;

using LaYumba.Functional;




public readonly record struct CanvasPosition(double X, double Y);

public readonly record struct CanvasSize(int Width, int Height);

public readonly record struct MandelbrotPosition(double X, double Y);

public readonly record struct MandelbrotSize(MandelbrotPosition Min, MandelbrotPosition Max)
{
    public static MandelbrotSize Empty => new MandelbrotSize(new MandelbrotPosition(0, 0), new MandelbrotPosition(0, 0));
}





public readonly record struct MandelbrotParameter(CanvasSize CanvasSize, MandelbrotSize CurrentMandelbrotSize, int MaxIterations);

public readonly record struct MandelbrotZoomParameter(CanvasPosition MousePosition, CanvasSize CanvasSize, MandelbrotSize CurrentMandelbrotSize, int Delta, int MaxIterations);

public readonly record struct MandelbrotResult(string ImageData, MandelbrotSize MandelbrotSize, ErrorType[] Errors, bool HasErrors);


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

