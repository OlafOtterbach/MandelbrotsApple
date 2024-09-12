namespace MandelbrotsApple.Mandelbrot.Functions;

using LaYumba.Functional;
using MandelbrotsApple.Mandelbrot.Model;
using static LaYumba.Functional.F;
using static MandelbrotErrors;

public static class MandelbrotValidation
{
    private static readonly double EPSILON = 0.0000001;

    public static Func<Option<MandelbrotParameter>, Validation<MandelbrotParameter>>
    Validate
        => ValidateWithValidators(
                Validate_Width_Minimum,
                Validate_Width_Maximum,
                Validate_Height_Minimum,
                Validate_Height_Maximum,
                Validate_XMin_ANd_XMax,
                Validate_YMin_YMax,
                Validate_Difference_Between_XMin_XMax,
                Validate_Difference_Between_YMin_YMax,
                Validate_Iteration_Minimum,
                Validate_Iteration_Maximum);

    private static Func<Option<MandelbrotParameter>, Validation<MandelbrotParameter>>
    ValidateWithValidators(params Func<MandelbrotParameter, Validation<MandelbrotParameter>>[] validators)
        => op => op.Match(
                            () => Invalid(ParameterIsNullError),
                            p =>
                            {
                                var errors = validators
                                            .Map(validate => validate(p))
                                            .Bind(validation => validation.Match(
                                                                            Invalid: errs => Some(errs),
                                                                            Valid: _ => None))
                                            .ToList();
                                return errors.Any() ? Invalid(errors.Flatten()) : Valid(p);
                            }
                        );





    public static Validation<MandelbrotParameter>
    Validate_Width_Minimum(MandelbrotParameter parameter)
        => parameter.Width > 0
            ? parameter
            : WidthIsLessThanOnePixelError;


    public static Validation<MandelbrotParameter>
    Validate_Width_Maximum(MandelbrotParameter parameter)
        => parameter.Width <= 10000
            ? parameter
            : WidthIsGreaterThan10000PixelError;


    public static Validation<MandelbrotParameter>
    Validate_Height_Minimum(MandelbrotParameter parameter)
        => parameter.Height > 0
            ? parameter
            : HeightIsLessThanOnePixelError;


    public static Validation<MandelbrotParameter>
    Validate_Height_Maximum(MandelbrotParameter parameter)
        => parameter.Width <= 10000
            ? parameter
            : HeightIsGreaterThan10000PixelError;


    public static Validation<MandelbrotParameter>
    Validate_XMin_ANd_XMax(MandelbrotParameter parameter)
        => parameter.XMin < parameter.XMax
            ? parameter
            : XMinIsGreaterThanXMaxError;


    public static Validation<MandelbrotParameter>
    Validate_YMin_YMax(MandelbrotParameter parameter)
        => parameter.YMin < parameter.YMax
            ? parameter
            : YMinIsGreaterThanYMaxError;


    public static Validation<MandelbrotParameter>
    Validate_Difference_Between_XMin_XMax(MandelbrotParameter parameter)
        => Math.Abs(parameter.XMin - parameter.XMax) > EPSILON
            ? parameter
            : XMinAndXMaxDifferenceToSmall;


    public static Validation<MandelbrotParameter>
    Validate_Difference_Between_YMin_YMax(MandelbrotParameter parameter)
        => Math.Abs(parameter.YMin - parameter.YMax) > EPSILON
            ? parameter
            : YMinAndYMaxDifferenceToSmall;



    public static Validation<MandelbrotParameter>
    Validate_Iteration_Minimum(MandelbrotParameter parameter)
        => parameter.MaxIterations > 0
            ? parameter
            : IterationLessOrEqualThanZeroError;


    public static Validation<MandelbrotParameter>
    Validate_Iteration_Maximum(MandelbrotParameter parameter)
        => parameter.MaxIterations <= 1000
            ? parameter
            : IterationGreaterThanThousandError;
}

