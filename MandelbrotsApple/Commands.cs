using MandelbrotsApple.Mandelbrot;

namespace MandelbrotsApple;

public record struct Init(MandelbrotSize MandelbrotSize, int IterationPercentage, ImageSize ImageSize);

public record struct MaxIteration(int IterationPercentage, ImageSize ImageSize);

public record struct Refresh(ImageSize ImageSize);

public record struct MoveLowAndFinalHigh(ImageVector ImageMoveVector, ImageSize ImageSizeLow, ImageSize ImageSizeHigh);

public record struct Move(ImageVector ImageMoveVector, ImageSize ImageSize);

public record struct ZoomLowAndFinalHigh(bool ZoomIn, int ZoomCount, ImagePosition ImagePosition, ImageSize ImageSizeLow, ImageSize ImageSizeHigh);

public record struct Zoom(bool ZoomIn, int ZoomCount, ImagePosition ImagePosition, ImageSize ImageSize);
