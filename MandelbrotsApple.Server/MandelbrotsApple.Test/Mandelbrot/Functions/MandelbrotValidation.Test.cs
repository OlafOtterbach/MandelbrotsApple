using MandelbrotsApple.Mandelbrot;

namespace MandelbrotsApple.Server.Test.Mandelbrot;

public class ValidationTest
{
    [Fact]
    public void Validate_Difference_Between_XMin_XMax_Test__Difference_exist()
    {
        // Arrange
        var parameter = new MandelbrotParameter(new CanvasSize(320, 200), new MandelbrotSize(new MandelbrotPosition(2.0, -2.0), new MandelbrotPosition(-2.0, 2.0)), 255);

        // Act
        var result = Validation.Validate_Difference_Between_XMin_XMax(parameter);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_Difference_Between_XMin_XMax_Test__Difference_exist_not()
    {
        // Arrange
        var parameter = new MandelbrotParameter(new CanvasSize(320, 200), new MandelbrotSize(new MandelbrotPosition(2.0, -2.0), new MandelbrotPosition(2.000000001, 2.0)), 255);

        // Act
        var result = Validation.Validate_Difference_Between_XMin_XMax(parameter);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_Difference_Between_YMin_YMax_Test__Difference_exist()
    {
        // Arrange
        var parameter = new MandelbrotParameter(new CanvasSize(320, 200), new MandelbrotSize(new MandelbrotPosition(2.0, 2.0), new MandelbrotPosition(-2.0, -2.0)), 255);

        // Act
        var result = Validation.Validate_Difference_Between_YMin_YMax(parameter);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_Difference_Between_YMin_YMax_Test__Difference_exist_not()
    {
        // Arrange
        var parameter = new MandelbrotParameter(new CanvasSize(320, 200), new MandelbrotSize(new MandelbrotPosition(-2.0, 2.0), new MandelbrotPosition(2.0, 2.0000000001)), 255);

        // Act
        var result = Validation.Validate_Difference_Between_YMin_YMax(parameter);

        // Assert
        Assert.False(result.IsValid);
    }


    //[Fact]
    //public void Validate_XMin_ANd_XMax_Test__Min_greater_than_max()
    //{
    //    // Arrange
    //    var parameter = new MandelbrotParameter(320, 200, 2.0, -2.0, -2.0, 2.0, 255);

    //    // Act
    //    var result = Validation.Validate_XMin_ANd_XMax(parameter);

    //    // Assert
    //    Assert.False(result.IsValid);
    //}

    //[Fact]
    //public void Validate_XMin_ANd_XMax_Test__Min_less_than_max()
    //{
    //    // Arrange
    //    var parameter = new MandelbrotParameter(320, 200, -2.0, 2.0, -2.0, 2.0, 255);

    //    // Act
    //    var result = Validation.Validate_XMin_ANd_XMax(parameter);

    //    // Assert
    //    Assert.True(result.IsValid);
    //}

    //[Fact]
    //public void Validate_Test__Checking_error_collecting_of_two_invalid_cases()
    //{
    //    // Arrange
    //    var parameter = new MandelbrotParameter(320, 200, 2.0, -2.0, 2.0, -2.0, 255);
    //    MandelbrotParameter resultParam = new MandelbrotParameter(77, 77, 77.0, 77.0, 77.0, 77.0, 77);
    //    var errorList = new List<ValidationError>();

    //    // Act
    //    var result = Validation.Validate(parameter);
    //    result.Match(errs => errorList = errs.Cast<ValidationError>().ToList(), p => resultParam = p);

    //    // Assert
    //    Assert.False(result.IsValid);
    //    Assert.Equal(2, errorList.Count);
    //    Assert.Equal(ErrorType.XMinIsGreaterThanXMaxError, errorList[0].ErrorType);
    //    Assert.Equal(ErrorType.YMinIsGreaterThanYMaxError, errorList[1].ErrorType);
    //}

    //[Fact]
    //public void Validate_Test__Checking_parameter_delivery_if_all_valid()
    //{
    //    // Arrange
    //    var parameter = new MandelbrotParameter(320, 200, -2.0, 2.0, -2.0, 2.0, 255);
    //    MandelbrotParameter resultParam = new MandelbrotParameter(77, 77, 77.0, 77.0, 77.0, 77.0, 77);

    //    // Act
    //    var result = Validation.Validate(parameter);
    //    result.Match(_ => { }, p => resultParam = p);

    //    // Assert
    //    Assert.True(result.IsValid);
    //    Assert.Equal(parameter.Width, resultParam.Width);
    //    Assert.Equal(parameter.Height, resultParam.Height);
    //    Assert.Equal(parameter.XMin, resultParam.XMin);
    //    Assert.Equal(parameter.XMax, resultParam.XMax);
    //    Assert.Equal(parameter.YMin, resultParam.YMin);
    //    Assert.Equal(parameter.YMax, resultParam.YMax);
    //    Assert.Equal(parameter.MaxIterations, resultParam.MaxIterations);
    //}
}