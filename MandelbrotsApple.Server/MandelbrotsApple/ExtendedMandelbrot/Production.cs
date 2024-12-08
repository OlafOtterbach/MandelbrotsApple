namespace MandelbrotsApple.ExtendedMandelbrot;

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
                    (ImageResult.Apply(parameter.CurrentMandelbrotSize)));
}
