namespace MandelbrotsApple.Mandelbrot;

using LaYumba.Functional;
using MandelbrotsApple.Mandelbrot.Model;

using static Functions.MandelbrotFunctionBox;
using static Functions.MandelbrotValidation;
using static Functions.MandelbrotResultsConverter;
using Microsoft.AspNetCore.Mvc;

public class MandelbrotSetGenerator
{
    public static MandelbrotResult GenerateMandelbrotSet(MandelbrotParameter parameter)
        =>  Validate(parameter)
            .Map(MandelbrotSet)
            .Match(ErrorResult, 
                   ImageResult);

    public static IResult GetMandelbrotSet()
         => Validate(new MandelbrotParameter(1024, 768, 0.763, 0.768, 0.0999, 0.103, 255))
                    .Map(MandelbrotSet)
                    .Match(ImageErrorResult, ImageDataResult);
}
