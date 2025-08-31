namespace MandelbrotsApple;

public interface MandelbrotCommand { }

/*
 Mach mir einen TraceAgent der vom MandelbrotViewAgent alle Kommandos weiter geleitet bekommt und diese bei StartInit in die darin angegebene Datei per Stream schreibt. Und bei Empfang von StipTrace diese Datei schließt.
 */


public record struct StartTrace(string Path) : MandelbrotCommand;

public record struct StopTrace() : MandelbrotCommand;

public record struct Init(double Xmin, double Ymin, double Xmax, double Ymax, int IterationPercentage, int Width, int Height) : MandelbrotCommand;

public record struct MaxIteration(int IterationPercentage, int Width, int Height) : MandelbrotCommand;

public record struct Refresh(int Width, int Height) : MandelbrotCommand;

public record struct MoveLowAndFinalHigh(int Vx, int Vy, int WidthLow, int HeightLow, int WidthHigh, int HeightHigh) : MandelbrotCommand;

public record struct Move(int Vx, int Vy, int Width, int Height) : MandelbrotCommand;

public record struct ZoomLowAndHigh(bool ZoomIn, int ZoomCount, int X, int Y, int WidthLow, int HeightLow, int WidthHigh, int HeightHigh) : MandelbrotCommand;

public record struct Zoom(bool ZoomIn, int ZoomCount, int X, int Y, int Width, int Height) : MandelbrotCommand;
