using MandelbrotsApple.Mandelbrot;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static MandelbrotsApple.Mandelbrot.View;

var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = null };
var builder = WebApplication.CreateBuilder(args);


// Configure JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

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
    ([FromRoute] int width, [FromRoute] int height) 
        => Results.Json(Initialize(new CanvasSize(width, height)), jsonOptions));

app.MapPost(
    "/refresh",
    ([FromBody] MandelbrotParameter parameter)
        => Results.Json(Refresh(parameter), jsonOptions));

app.MapPost(
    "/zoom",
    ([FromBody] MandelbrotZoomParameter zoomParameter)
        => Results.Json(Zoom(zoomParameter), jsonOptions));


app.Run("http://localhost:5200");
