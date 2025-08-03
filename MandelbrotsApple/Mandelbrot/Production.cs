namespace MandelbrotsApple.Mandelbrot;

using LaYumba.Functional;
using static ResultConverting;
using static Validation;

public static class Production
{
    public static MandelbrotResult GenerateMandelbrotImage(MandelbrotParameter parameter)
         => Validate(parameter)
            .Map(MandelbrotImage)
            .Match(ErrorResult,
                   validResult => validResult);


    public static MandelbrotResult GenerateMandelbrotImage(this Validation<MandelbrotParameter> validatedParameter)
         => validatedParameter
            .Bind(Validate)
            .Map(MandelbrotImage)
            .Match(ErrorResult,
                   validResult => validResult);

    private static MandelbrotResult MandelbrotImage(MandelbrotParameter parameter)
        => ImageResult(ImageGenerating.MandelbrotImage(parameter), parameter.ImageSize, parameter.CurrentMandelbrotSize, parameter.MaxIterations);
}
