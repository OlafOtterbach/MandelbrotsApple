namespace MandelbrotsApple.Mandelbrot;

using LaYumba.Functional;
using MandelbrotsApple.Mandelbrot.Model;
using static Functions.MandelbrotFunctionBox;
using static Functions.MandelbrotResultsConverter;
using static Functions.MandelbrotValidation;

public class MandelbrotSetGenerator
{
    public static MandelbrotResult GenerateMandelbrotSet(MandelbrotParameter parameter)
        =>  Validate(parameter)
            .Map(MandelbrotSet)
            .Match(ErrorResult, 
                   ImageResult);
}
