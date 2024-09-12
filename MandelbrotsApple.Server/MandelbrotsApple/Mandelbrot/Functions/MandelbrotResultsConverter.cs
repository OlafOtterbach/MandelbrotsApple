namespace MandelbrotsApple.Mandelbrot.Functions
{
    using LaYumba.Functional;
    using MandelbrotsApple.Mandelbrot.Model;

    public static class MandelbrotResultsConverter
    {
        public static MandelbrotResult ImageResult(IEnumerable<byte> image)
            => new MandelbrotResult(image.ToArray(), Array.Empty<ErrorType>(), false);

        public static MandelbrotResult ErrorResult(IEnumerable<Error> errors) 
            => new MandelbrotResult(Array.Empty<byte>(), errors.OfType<ValidationError>().Select(e => e.ErrorType).ToArray(), true);
    }
}