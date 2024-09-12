namespace MandelbrotsApple.Server.Test.Mandelbrot;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using MandelbrotsApple.Mandelbrot.Model;
using static MandelbrotsApple.Mandelbrot.Functions.MandelbrotFuncBox;

public class MandelbrotFuncBoxTest
{
    [Fact]
    public void MandelbrotSet_Test()
    {
        // Arrange
        var parameter = new MandelbrotParameter(320, 200, -2.0, 2.0, -2.0, 2.0, 255);

        // Act
        var appleMan = MandelbrotSet(parameter).ToArray();

        // Assert
        Assert.Equal(320 * 200 * 4, appleMan.Length);
    }

    [Fact]
    public void MandelbrotSet_Test2()
    {
        // Arrange
        var parameter = new MandelbrotParameter(10, 10, -5.0, 5.0, -5.0, 5.0, 255);

        // Act
        var appleMan = MandelbrotSet(parameter).ToArray();

        // Assert
        Assert.Equal(10 * 10 * 4, appleMan.Length);
    }

    [Fact]
    public void SaveImage()
    {
        // Arrange
        int width = 2560; // Set your desired width
        int height = 1440; // Set your desired height
        var parameter = new MandelbrotParameter(width, height, -3.0, 3.0, -2.0, 2.0, 255);

        // Act
        var start = DateTime.Now;
        var appleMan = MandelbrotSet(parameter).ToArray();
        var end = DateTime.Now;
        var time = (end - start).TotalMicroseconds;


        Bitmap bitmap = CreateImageFromArgbByteArray(appleMan, width, height);

        bitmap.Save(@"c:\tmp\output.png", ImageFormat.Png);
    }



    [Fact]
    public void XCoordinates_Test()
    {
        // Act
        var sequence = XCoordinates(100, 10.0, 110.0)().ToArray();

        // Assert
        Assert.Equal(100, sequence.Count());
        Assert.Equal(10, sequence.First());

        var values = sequence.ToArray();
        var step = Step(100, 10.0, 110.0);
        Assert.True(values.Zip(values.Skip(1), (a, b) => b - a).All(x => Math.Abs(x - step) < 0.001));

        Assert.Equal(110, sequence.Last());
    }


    public static Bitmap CreateImageFromArgbByteArray(byte[] byteArray, int width, int height)
    {
        if (byteArray == null)
            throw new ArgumentNullException(nameof(byteArray));

        if (byteArray.Length != width * height * 4)
            throw new ArgumentException("Byte array length does not match the specified width and height.");


        var groupedSequence = byteArray
            .Select((value, index) => new { value, index })
            .GroupBy(x => x.index / 4)
            .Select(g => g.Select(x => x.value).ToList())
            .ToList();

        var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

        var rect = new Rectangle(0, 0, width, height);
        var bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, bitmap.PixelFormat);

        try
        {
            // Copy the byte array to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(byteArray, 0, bitmapData.Scan0, byteArray.Length);
        }
        finally
        {
            bitmap.UnlockBits(bitmapData);
        }

        for (var y = 0; y < 600; y++)
        {
            for (var x = 0; x < 800; x++)
            {
                var px = bitmap.GetPixel(x, y);
                if (px.R > 0)
                {

                }
            }
        }

        return bitmap;
    }
}


