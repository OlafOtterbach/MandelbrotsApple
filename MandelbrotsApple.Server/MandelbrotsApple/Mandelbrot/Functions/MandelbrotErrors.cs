namespace MandelbrotsApple.Mandelbrot.Functions;

using MandelbrotsApple.Mandelbrot.Model;

public static class MandelbrotErrors
{
    public static ValidationError ParameterIsNullError => new ValidationError(ErrorType.ParameterIsNull);

    public static ValidationError WidthIsLessThanOnePixelError => new ValidationError(ErrorType.WidthIsLessThanOnePixelError);

    public static ValidationError HeightIsLessThanOnePixelError => new ValidationError(ErrorType.HeightIsLessThanOnePixelError);

    public static ValidationError WidthIsGreaterThan10000PixelError => new ValidationError(ErrorType.WidthIsGreaterThan10000PixelError);

    public static ValidationError HeightIsGreaterThan10000PixelError => new ValidationError(ErrorType.HeightIsGreaterThan10000PixelError);

    public static ValidationError XMinIsGreaterThanXMaxError => new ValidationError(ErrorType.XMinIsGreaterThanXMaxError);

    public static ValidationError YMinIsGreaterThanYMaxError => new ValidationError(ErrorType.YMinIsGreaterThanYMaxError);

    public static ValidationError XMinAndXMaxDifferenceToSmall => new ValidationError(ErrorType.XMinAndXMaxDifferenceToSmall);

    public static ValidationError YMinAndYMaxDifferenceToSmall => new ValidationError(ErrorType.YMinAndYMaxDifferenceToSmall);

    public static ValidationError IterationGreaterThanThousandError => new ValidationError(ErrorType.IterationGreaterThanThousandError);

    public static ValidationError IterationLessOrEqualThanZeroError => new ValidationError(ErrorType.IterationLessOrEqualThanZeroError);
}

