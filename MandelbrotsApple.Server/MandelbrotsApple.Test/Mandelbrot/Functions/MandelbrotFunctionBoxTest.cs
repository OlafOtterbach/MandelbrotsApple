namespace MandelbrotsApple.Server.Test.Mandelbrot
{
    using System.Drawing.Imaging;
    using System.Drawing;
    using MandelbrotsApple.Mandelbrot.Model;
    using static MandelbrotsApple.Mandelbrot.Functions.MandelbrotFunctionBox;

    public class MandelbrotFunctionBoxTest
    {
        [Fact]
        public void Step_Test()
        {
            // Act   |-------- >4< -------|
            //       0      1      2      3
            //       |------|------|------|
            //     >0.0<  0.333  0.666  >1.0<
            var step = Step(4, 0.0, 1.0);
            var sequence = XCoordinates(4, 0.0, 1.0);

            // Assert 
            Assert.Equal(1, 3 * step);
        }

        [Fact]
        public void IndexToCoordinate_Test()
        {
            // Act
            var coordinate = IndexToCoordinate(10, 100, 1);

            // Assert
            Assert.Equal(100 + 10 * 1, coordinate);
        }

        [Fact]
        public void YCoordinates_Test()
        {
            // Arrange
            var step = Step(100, 10.0, 110.0);

            // Act
            var sequence = YCoordinates(640, 100, 10.0, 110.0).ToArray();

            // Assert
            Assert.Equal(100, sequence.Count());
            Assert.Equal((0, 10.0), sequence.First());
            Assert.Equal(((100 - 1) * 640 * 4, 110), sequence.Last());
            Assert.True(sequence.Zip(sequence.Skip(1), (a, b) => b.Coordinate - a.Coordinate).All(x => Math.Abs(x - step) < 0.001));
            Assert.True(sequence.Zip(sequence.Skip(1), (a, b) => b.Adress - a.Adress).All(x => x == 640 * 4));
        }

        [Fact]
        public void XCoordinates_Test()
        {
            // Arrange
            var step = Step(100, 10.0, 110.0);

            // Act
            var sequence = XCoordinates(100, 10.0, 110.0);

            // Assert
            Assert.Equal(100, sequence.Count());
            Assert.Equal((0, 10.0), sequence.First());
            Assert.Equal(((100 - 1) * 4, 110), sequence.Last());
            Assert.True(sequence.Zip(sequence.Skip(1), (a, b) => b.Coordinate - a.Coordinate).All(x => Math.Abs(x - step) < 0.001));
            Assert.True(sequence.Zip(sequence.Skip(1), (a, b) => b.Adress - a.Adress).All(x => x == 4));
        }

        [Fact]
        public void SaveImage()
        {
            //var parameter = new MandelbrotParameter(800, 600, -1.5, 2.5, -2.0, 2.0, 255);
            var step = 0.0; //0.002;
            var parameter = new MandelbrotParameter(1024, 768, 0.763 + step, 0.768 + step, 0.0999, 0.103, 255);


            var start = DateTime.Now;
            var appleMan = MandelbrotSet(parameter);
            var end = DateTime.Now;
            var time = (end - start).TotalMicroseconds;

            Bitmap bitmap = CreateImageFromArgbByteArray(ToArgb(appleMan), parameter.Width, parameter.Height);

            bitmap.Save(@"c:\tmp\output.png", ImageFormat.Png);
        }

        private static byte[] ToArgb(byte[] data)
            => ToArgbSequence(data).ToArray();


        private static IEnumerable<byte> ToArgbSequence(byte[] data)
        {
            for(var index = 0; index < data.Length; index += 3)
            {
                yield return data[index + 2];
                yield return data[index + 1];
                yield return data[index];
                yield return 255;
            }
        }

        private static Bitmap CreateImageFromArgbByteArray(byte[] byteArray, int width, int height)
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
}
