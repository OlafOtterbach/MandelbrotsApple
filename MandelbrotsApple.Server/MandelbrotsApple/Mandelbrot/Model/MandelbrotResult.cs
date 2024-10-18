namespace MandelbrotsApple.Mandelbrot.Model;

public record MandelbrotResult(string ImageData, ErrorType[] Errors, bool HasErrors);

