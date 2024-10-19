using MandelbrotsApple.Mandelbrot.Model;
using static MandelbrotsApple.Mandelbrot.MandelbrotSetGenerator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors();

app.MapGet("/", () => "Hello Mandelbrot!");
app.MapPost("/mandelbrot", (MandelbrotParameter request) => Results.Ok(GenerateMandelbrotSet(request)));
app.Run("http://localhost:5200");
