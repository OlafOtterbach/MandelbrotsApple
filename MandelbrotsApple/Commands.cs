using MandelbrotsApple.Mandelbrot;

namespace MandelbrotsApple;

public interface MandelbrotCommand { }

public record struct StartTrace(string Path) : MandelbrotCommand;

public record struct StopTrace() : MandelbrotCommand;

public record struct Init(MandelbrotSize MandelbrotSize, int IterationPercentage, ImageSize ImageSize) : MandelbrotCommand;

public record struct MaxIteration(int IterationPercentage, ImageSize ImageSize) : MandelbrotCommand;

public record struct Refresh(ImageSize ImageSize) : MandelbrotCommand;

public record struct MoveLowAndFinalHigh(ImageVector ImageMoveVector, ImageSize ImageSizeLow, ImageSize ImageSizeHigh) : MandelbrotCommand;

public record struct Move(ImageVector ImageMoveVector, ImageSize ImageSize) : MandelbrotCommand;

public record struct ZoomLowAndHigh(bool ZoomIn, int ZoomCount, ImagePosition ImagePosition, ImageSize ImageSizeLow, ImageSize ImageSizeHigh) : MandelbrotCommand;

public record struct Zoom(bool ZoomIn, int ZoomCount, ImagePosition ImagePosition, ImageSize ImageSize) : MandelbrotCommand;
