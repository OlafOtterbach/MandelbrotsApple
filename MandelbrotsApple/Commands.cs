using MandelbrotsApple.Mandelbrot;

namespace MandelbrotsApple;

public interface MandelbrotCommand { }

public record struct StartTrace(string Path) : MandelbrotCommand;

public record struct StartAndState(string Path, MandelbrotState State) : MandelbrotCommand;

public record struct StopTrace() : MandelbrotCommand;

public record struct Init(double Xmin, double Ymin, double Xmax, double Ymax, int IterationPercentage, int Width, int Height) : MandelbrotCommand;

public record struct MaxIteration(MandelbrotState CurrentState, int IterationPercentage, int Width, int Height) : MandelbrotCommand;

public record struct Refresh(MandelbrotState CurrentState, int Width, int Height) : MandelbrotCommand;

public record struct MoveLowAndFinalHigh(MandelbrotState CurrentState, MandelbrotPosition mandelbrotMovePosition, int WidthLow, int HeightLow, int WidthHigh, int HeightHigh) : MandelbrotCommand;

public record struct Move(MandelbrotState CurrentState, MandelbrotPosition MandelbrotMovePosition, int Width, int Height) : MandelbrotCommand;

public record struct ZoomLowAndHigh(MandelbrotState CurrentState, bool ZoomIn, int ZoomCount, int X, int Y, int WidthLow, int HeightLow, int WidthHigh, int HeightHigh) : MandelbrotCommand;

public record struct Zoom(MandelbrotState CurrentState, bool ZoomIn, int ZoomCount, int X, int Y, int Width, int Height) : MandelbrotCommand;
