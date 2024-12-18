//namespace MandelbrotsApple.Mandelbrot.Functions
//{
//    using LaYumba.Functional;
//    using MandelbrotsApple.Mandelbrot.Model;

//    public static class MandelbrotResultsConverter
//    {
//        public static MandelbrotResult ImageResult(byte[] image)
//            => new MandelbrotResult(image.ToImageData(), Array.Empty<ErrorType>(), false);

//        public static MandelbrotResult ErrorResult(IEnumerable<Error> errors) 
//            => new MandelbrotResult(string.Empty, errors.OfType<ValidationError>().Select(e => e.ErrorType).ToArray(), true);


//        private static string ToImageData(this byte[] image)  => new string(image.SelectMany(ToNibbles).ToArray());

//        private static IEnumerable<char> ToNibbles(byte value)
//        {
//            yield return ToHexDigit(value >> 4);
//            yield return ToHexDigit(value & 0x0F);
//        }

//        private static char ToHexDigit(int value)
//        =>  value switch
//            {
//                0  => '0',
//                1  => '1',
//                2  => '2',
//                3  => '3',
//                4  => '4',
//                5  => '5',
//                6  => '6',
//                7  => '7',
//                8  => '8',
//                9  => '9',
//                10 => 'A',
//                11 => 'B',
//                12 => 'C',
//                13 => 'D',
//                14 => 'E',
//                _  => 'F'
//            };
//    }
//}