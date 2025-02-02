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
    "/{width}/{height}/{maxIterations}/initialize",
    ([FromRoute] int width, [FromRoute] int height, [FromRoute] int maxIterations) 
        => Results.Json(Initialize(new ImageSize(width, height), maxIterations), jsonOptions));

app.MapPost(
    "/refresh",
    ([FromBody] MandelbrotParameter parameter)
        => Results.Json(Refresh(parameter), jsonOptions));

app.MapPost(
    "/zoom",
    ([FromBody] MandelbrotZoomParameter zoomParameter)
        => Results.Json(Zoom(zoomParameter), jsonOptions));

app.MapPost(
    "/move",
    ([FromBody] MandelbrotMoveParameter moveParameter)
        => Results.Json(Move(moveParameter), jsonOptions));


app.Run("http://localhost:5200");
