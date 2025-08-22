namespace MandelbrotsApple;

public record struct ResizeCommand(int Width, int Height);

public record struct MoveEvent(int Vx, int Vy, int EndX, int EndY, int WidthLow, int HeightLow, int WidthHigh, int HeightHigh);

public record struct MoveCommand(int Vx, int Vy, int Width, int Height);

public record struct WheelEvent(bool ZoomIn, int ZoomCount, int X, int Y, int WidthLow, int HeightLow, int WidthHigh, int HeightHigh);

public record struct WheelCommand(bool ZoomIn, int ZoomCount, int X, int Y, int Width, int Height);
