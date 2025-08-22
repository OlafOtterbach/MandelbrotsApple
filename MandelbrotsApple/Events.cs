namespace MandelbrotsApple;

public record struct ResizeCommand(int Width, int Height);

public record struct StartCommand(int X, int Y);

public record struct MoveEvent(int X, int Y, int WidthLow, int HeightLow, int WidthHigh, int HeightHigh);

public record struct MoveCommand(int X, int Y, int Width, int Height);
