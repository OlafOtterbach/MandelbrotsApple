namespace MandelbrotsApple.Mandelbrot.Model;

public record MandelbrotResult(byte[] Image, ErrorType[] errors, bool HasErrors);

