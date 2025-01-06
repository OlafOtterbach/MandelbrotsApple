namespace MandelbrotsApple.Mandelbrot;

using LaYumba.Functional;
using static Generating;
using static Validation;
using static ResultConverting;

public static class Production
{
    public static MandelbrotResult GenerateMandelbrotSet(MandelbrotParameter parameter)
         => Validate(parameter)
            .Map(MandelbrotSet)
            .Match(ErrorResult,
                   validResult => validResult);


    public static MandelbrotResult GenerateMandelbrotSet(this Validation<MandelbrotParameter> validatedParameter)
         => validatedParameter
            .Bind(Validate)
            .Map(MandelbrotSet)
            .Match(ErrorResult,
                   validResult => validResult);

    private static MandelbrotResult MandelbrotSet(MandelbrotParameter parameter)
        => ImageResult(parameter.CurrentMandelbrotSize, MandelbrotImage(parameter));
}
