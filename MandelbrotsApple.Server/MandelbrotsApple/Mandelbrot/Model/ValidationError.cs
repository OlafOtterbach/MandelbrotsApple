namespace MandelbrotsApple.Mandelbrot.Model;

using LaYumba.Functional;

public record ValidationError(ErrorType ErrorType) : Error(ErrorType.ToString());

