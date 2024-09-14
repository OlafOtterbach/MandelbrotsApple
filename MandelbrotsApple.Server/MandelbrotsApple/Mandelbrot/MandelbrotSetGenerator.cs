namespace MandelbrotsApple.Mandelbrot;

using LaYumba.Functional;
using MandelbrotsApple.Mandelbrot.Model;

using static Functions.MandelbrotFunctionBox;
using static Functions.MandelbrotValidation;
using static Functions.MandelbrotResultsConverter;

public class MandelbrotSetGenerator
{
    public static MandelbrotResult GenerateMandelbrotSet(MandelbrotParameter parameter)
        =>  Validate(parameter)
            .Map(MandelbrotSet)
            .Match(ErrorResult, 
                   ImageResult);
}
