namespace MandelbrotsApple.Mandelbrot.Model;

public record MandelbrotResult(char[] ImageData, ErrorType[] errors, bool HasErrors);

