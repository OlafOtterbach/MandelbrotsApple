namespace MandelbrotsApple.Mandelbrot;

using LaYumba.Functional;

public static class ResultConverting
{
    public static MandelbrotResult ImageResult(byte[] image, ImageSize ImageSize, MandelbrotSize mandelbrotSize, int maxIterations)
        => new MandelbrotResult(image, ImageSize, mandelbrotSize, maxIterations, Array.Empty<ErrorType>(), false);

    public static MandelbrotResult ErrorResult(IEnumerable<Error> errors)
        => new MandelbrotResult([], new ImageSize(), MandelbrotSize.Empty, 0, errors.OfType<ValidationError>().Select(e => e.ErrorType).ToArray(), true);
}
