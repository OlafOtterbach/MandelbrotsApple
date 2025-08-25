namespace MandelbrotsApple;

public record struct Init(int IterationPercentage, int Width, int Height);

public record struct MaxIteration(int IterationPercentage, int Width, int Height);

public record struct Resize(int Width, int Height);

public record struct MoveLowAndFinalHigh(int Vx, int Vy, int EndX, int EndY, int WidthLow, int HeightLow, int WidthHigh, int HeightHigh);

public record struct Move(int Vx, int Vy, int Width, int Height);

public record struct ZoomLowAndHigh(bool ZoomIn, int ZoomCount, int X, int Y, int WidthLow, int HeightLow, int WidthHigh, int HeightHigh);

public record struct Zoom(bool ZoomIn, int ZoomCount, int X, int Y, int Width, int Height);
