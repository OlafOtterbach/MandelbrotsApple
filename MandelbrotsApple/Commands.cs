using MandelbrotsApple.Mandelbrot;

namespace MandelbrotsApple;

public interface IMandelbrotCommand { }

public record struct Init(MandelbrotSize MandelbrotSize, int IterationPercentage, ImageSize ImageSize) : IMandelbrotCommand;

public record struct MaxIteration(int IterationPercentage, ImageSize ImageSize) : IMandelbrotCommand;

public record struct Refresh(ImageSize ImageSize) : IMandelbrotCommand;

public record struct MoveLowAndFinalHigh(ImageVector ImageMoveVector, ImageSize ImageSizeLow, ImageSize ImageSizeHigh) : IMandelbrotCommand;

public record struct Move(ImageVector ImageMoveVector, ImageSize ImageSize) : IMandelbrotCommand;

public record struct ZoomLowAndFinalHigh(bool ZoomIn, int ZoomCount, ImagePosition ImagePosition, ImageSize ImageSizeLow, ImageSize ImageSizeHigh) : IMandelbrotCommand;

public record struct Zoom(bool ZoomIn, int ZoomCount, ImagePosition ImagePosition, ImageSize ImageSize) : IMandelbrotCommand;
