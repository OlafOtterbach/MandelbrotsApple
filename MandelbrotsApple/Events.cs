namespace MandelbrotsApple;

public record struct ResizeCommand(int Width, int Height);

public record struct MoveEvent(int Vx, int Vy, int EndX, int EndY, int WidthLow, int HeightLow, int WidthHigh, int HeightHigh);

public record struct MoveCommand(int Vx, int Vy, int Width, int Height);

public record struct RefreshCommand(int x, int y, int Width, int Height);
