namespace MandelbrotsApple.Mandelbrot;

public static class Common
{
    public static int BytesPerNumber(int number)
        => (int)Math.Ceiling(BitsPerNumber(number) / 8.0);
     
    private static int BitsPerNumber(int number)
        => (int)Math.Floor(Math.Log(number, 2)) + 1;
}
