namespace MandelbrotsApple;

public interface MandelbrotCommand { }

public record struct Init(double Xmin, double Ymin, double Xmax, double Ymax, int IterationPercentage, int Width, int Height) : MandelbrotCommand;

public record struct MaxIteration(int IterationPercentage, int Width, int Height) : MandelbrotCommand;

public record struct Refresh(int Width, int Height) : MandelbrotCommand;

public record struct MoveLowAndFinalHigh(int Vx, int Vy, int EndX, int EndY, int WidthLow, int HeightLow, int WidthHigh, int HeightHigh) : MandelbrotCommand;

public record struct Move(int Vx, int Vy, int Width, int Height) : MandelbrotCommand;

public record struct ZoomLowAndHigh(bool ZoomIn, int ZoomCount, int X, int Y, int WidthLow, int HeightLow, int WidthHigh, int HeightHigh) : MandelbrotCommand;

public record struct Zoom(bool ZoomIn, int ZoomCount, int X, int Y, int Width, int Height) : MandelbrotCommand;
