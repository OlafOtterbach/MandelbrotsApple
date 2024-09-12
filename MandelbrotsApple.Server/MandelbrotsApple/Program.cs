using MandelbrotsApple.Mandelbrot.Model;
using static MandelbrotsApple.Mandelbrot.MandelbrotSetGenerator;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello Mandelbrot!");
app.MapPost("/mandelbrot", (MandelbrotParameter request) => Results.Ok(GenerateMandelbrotSet(request)));
app.Run("http://localhost:5200");
