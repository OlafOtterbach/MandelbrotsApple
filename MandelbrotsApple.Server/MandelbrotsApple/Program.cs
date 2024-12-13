using MandelbrotsApple.ExtendedMandelbrot;
using Microsoft.AspNetCore.Mvc;
using static MandelbrotsApple.ExtendedMandelbrot.View;

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

app.MapGet(
    "/{width}/{height}/initialize",
    ([FromRoute] int width, [FromRoute] int height) => Results.Ok(Initialize(new CanvasSize(width, height))));

app.MapPost(
    "/refresh",
    ([FromBody] MandelbrotParameter parameter) => Results.Ok(Refresh));

app.Run("http://localhost:5200");
