namespace MandelbrotsApple.Mandelbrot.Model;

public enum ErrorType
{
    ParameterIsNull,
    WidthIsLessThanOnePixelError,
    HeightIsLessThanOnePixelError,
    WidthIsGreaterThan10000PixelError,
    HeightIsGreaterThan10000PixelError,
    XMinIsGreaterThanXMaxError,
    YMinIsGreaterThanYMaxError,
    XMinAndXMaxDifferenceToSmall,
    YMinAndYMaxDifferenceToSmall,
    IterationLessOrEqualThanZeroError,
    IterationGreaterThanThousandError,
}


