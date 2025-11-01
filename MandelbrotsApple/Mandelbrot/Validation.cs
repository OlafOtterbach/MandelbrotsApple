namespace MandelbrotsApple.Mandelbrot;

using LaYumba.Functional;
using static LaYumba.Functional.F;
using static Errors;

public static class Validation
{
    private static readonly double EPSILON = 0.000000001;


    public static Validation<MandelbrotZoomParameter> ValidateMandelbrotZoomParameter(this MandelbrotZoomParameter zoomParameter)
      => zoomParameter.ToMandelbrotParameter().ValidateMandelbrotParameter().Map(param => zoomParameter);

    private static MandelbrotParameter ToMandelbrotParameter(this MandelbrotZoomParameter zoomParameter)
        => new MandelbrotParameter(zoomParameter.ImageSize, zoomParameter.CurrentMandelbrotSize, zoomParameter.MaxIterations);


    public static Validation<MandelbrotMoveParameter> ValidateMandelbrotMoveParameter(this MandelbrotMoveParameter moveParameter)
        => moveParameter.ToMandelbrotParameter().ValidateMandelbrotParameter().Map(param => moveParameter);

    private static MandelbrotParameter ToMandelbrotParameter(this MandelbrotMoveParameter moveParameter)
        => new MandelbrotParameter(moveParameter.ImageSize, moveParameter.CurrentMandelbrotSize, moveParameter.MaxIterations);




    public static Validation<MandelbrotParameter> ValidateMandelbrotParameter(this MandelbrotParameter parameter) => Validate(parameter);

    public static Func<MandelbrotParameter, Validation<MandelbrotParameter>>
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

    private static Func<MandelbrotParameter, Validation<MandelbrotParameter>>
    ValidateWithValidators(params Func<MandelbrotParameter, Validation<MandelbrotParameter>>[] validators)
        => parameter =>
           {
               var errors = validators
                           .Map(validate => validate(parameter))
                           .Bind(validation => validation.Match(
                                                           Invalid: errs => Some(errs),
                                                           Valid: _ => None))
                           .ToList();
               return errors.Any() ? Invalid(errors.Flatten()) : Valid(parameter);
           };




    public static Validation<MandelbrotParameter>
    Validate_Width_Minimum(MandelbrotParameter parameter)
        => parameter.ImageSize.Width > 0
            ? parameter
            : WidthIsLessThanOnePixelError;


    public static Validation<MandelbrotParameter>
    Validate_Width_Maximum(MandelbrotParameter parameter)
        => parameter.ImageSize.Width <= 10000
            ? parameter
            : WidthIsGreaterThan10000PixelError;


    public static Validation<MandelbrotParameter>
    Validate_Height_Minimum(MandelbrotParameter parameter)
        => parameter.ImageSize.Height > 0
            ? parameter
            : HeightIsLessThanOnePixelError;


    public static Validation<MandelbrotParameter>
    Validate_Height_Maximum(MandelbrotParameter parameter)
        => parameter.ImageSize.Width <= 10000
            ? parameter
            : HeightIsGreaterThan10000PixelError;


    public static Validation<MandelbrotParameter>
    Validate_XMin_ANd_XMax(MandelbrotParameter parameter)
        => parameter.CurrentMandelbrotSize.Min.X < parameter.CurrentMandelbrotSize.Max.X
            ? parameter
            : XMinIsGreaterThanXMaxError;


    public static Validation<MandelbrotParameter>
    Validate_YMin_YMax(MandelbrotParameter parameter)
        => parameter.CurrentMandelbrotSize.Min.Y < parameter.CurrentMandelbrotSize.Max.Y
            ? parameter
            : YMinIsGreaterThanYMaxError;


    public static Validation<MandelbrotParameter>
    Validate_Difference_Between_XMin_XMax(MandelbrotParameter parameter)
        => Math.Abs(parameter.CurrentMandelbrotSize.Min.X - parameter.CurrentMandelbrotSize.Max.X) > EPSILON
            ? parameter
            : XMinAndXMaxDifferenceToSmall;


    public static Validation<MandelbrotParameter>
    Validate_Difference_Between_YMin_YMax(MandelbrotParameter parameter)
        => Math.Abs(parameter.CurrentMandelbrotSize.Min.Y - parameter.CurrentMandelbrotSize.Max.Y) > EPSILON
            ? parameter
            : YMinAndYMaxDifferenceToSmall;



    public static Validation<MandelbrotParameter>
    Validate_Iteration_Minimum(MandelbrotParameter parameter)
        => parameter.MaxIterations > 0
            ? parameter
            : IterationLessOrEqualThanZeroError;


    public static Validation<MandelbrotParameter>
    Validate_Iteration_Maximum(MandelbrotParameter parameter)
        => parameter.MaxIterations <= 10000
            ? parameter
            : IterationGreaterThanThousandError;
}

