namespace MandelbrotsApple;

using MandelbrotsApple.Mandelbrot;
using System.Drawing.Imaging;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public partial class MandelbrotForm : Form
{
    private IMandelbrotViewServiceProxy _mandelbrotViewServiceProxy = new MandelbrotViewServiceProxy();
    private double _resolutionFactor = 1.0;
    private Bitmap? _imageBitmap;
    private bool _mouseDown = false;

    public MandelbrotForm()
    {
        InitializeComponent();

        _mandelbrotViewServiceProxy.DrawObservable
            .ObserveOn(SynchronizationContext.Current)
            .ObserveOn(SynchronizationContext.Current)
            .ObserveOn(SynchronizationContext.Current)
            .ObserveOn(SynchronizationContext.Current)
            .Subscribe(result => DrawMandelbrotResult(result));

        this.MinimumSize = new Size(800, 600);
        this.Load += On_MandelbrotForm_Load;
        this.Resize += On_MandelbrotForm_Resize;

        canvasPanel.Paint += On_CanvasPanel_Paint;
        canvasPanel.MouseDown += On_CanvasPanel_MouseDown;
        canvasPanel.MouseUp += On_CanvasPanel_MouseUp;
        canvasPanel.MouseMove += On_CanvasPanel_MouseMove;
        canvasPanel.MouseWheel += On_CanvasPanel_MouseWheel;
    }

    private void On_MandelbrotForm_Load(object? sender, EventArgs e)
    {
        var result = _mandelbrotViewServiceProxy.InitialView(Width, Height);
        DrawMandelbrotResult(result);
    }


    private void On_MandelbrotForm_Resize(object? sender, EventArgs e)
    {
        int width = Width;
        int height = Height;
        if (width > 0 && height > 0)
        {
            _mandelbrotViewServiceProxy.ResizeView(width, height);
        }
    }

    // MouseDown-Event
    private void On_CanvasPanel_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            int canvasX = e.X;
            int canvasY = e.Y;
            var x = (int)(canvasX * _resolutionFactor);
            var y = (int)(canvasY * _resolutionFactor);
            _mandelbrotViewServiceProxy.SetMouseStart(x, y);
            _mouseDown = true;
        }
    }

    // MouseUp-Event
    private void On_CanvasPanel_MouseUp(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            _mouseDown = false;
        }
    }

    // MouseMove-Event
    private void On_CanvasPanel_MouseMove(object? sender, MouseEventArgs e)
    {
        if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
        {
            if (_mouseDown)
            {
                int canvasX = e.X;
                int canvasY = e.Y;
                var x = (int)(canvasX * _resolutionFactor);
                var y = (int)(canvasY * _resolutionFactor);
                _mandelbrotViewServiceProxy.MouseMove(x, y);
            }
        }
    }

    // MouseWheel-Event
    private void On_CanvasPanel_MouseWheel(object? sender, MouseEventArgs e)
    {
        int delta = e.Delta;
        int wheelClicks = Math.Abs(delta / 120);
        int canvasX = e.X;
        int canvasY = e.Y;
        var x = (int)(canvasX * _resolutionFactor);
        var y = (int)(canvasY * _resolutionFactor);
        _mandelbrotViewServiceProxy.MouseWheel(delta < 0, wheelClicks, x, y);
    }



    private void On_SliderResolution_Scroll(object sender, EventArgs e)
    {
        var value = sliderResolution.Value;
        _resolutionFactor = GetResolutionFactor(value);
        int width = (int)(canvasPanel.Width * _resolutionFactor);
        int height = (int)(canvasPanel.Height * _resolutionFactor);
        _mandelbrotViewServiceProxy.SetResolution(value, width, height);
    }


    private void On_SliderIteration_Scroll(object sender, EventArgs e)
    {
        var value = sliderIteration.Value;
        _mandelbrotViewServiceProxy.SetMaxIterations(value);
    }




    private static double GetResolutionFactor(int percentage)
    {
        double resolutionFactor = (percentage * 0.80 + 20) / 100;
        return resolutionFactor;
    }


    private int Width => (int)(canvasPanel.Width * _resolutionFactor);

    private int Height => (int)(canvasPanel.Height * _resolutionFactor);


    private void DrawMandelbrotResult(MandelbrotResult result)
    {
        if (result.HasErrors)
            return;

        if (_imageBitmap == null || _imageBitmap.Width != result.ImageSize.Width || _imageBitmap.Height != result.ImageSize.Height)
        {
            if (_imageBitmap != null)
            {
                _imageBitmap.Dispose();
            }

            _imageBitmap = CreateBitmap(result.ImageSize.Width, result.ImageSize.Height);
        }

        if (_imageBitmap != null && result.ImageData.Length == _imageBitmap.Width * _imageBitmap.Height * 3)
        {
            int width = canvasPanel.Width;
            int height = canvasPanel.Height;
            CopyRgbToBitmap(_imageBitmap, result.ImageData, result.ImageSize.Width, result.ImageSize.Height);
        }

        canvasPanel.Invalidate();
    }


    private void On_CanvasPanel_Paint(object? sender, PaintEventArgs e)
    {
        if (_imageBitmap != null)
        {
            Rectangle destRect = new Rectangle(0, 0, canvasPanel.Width, canvasPanel.Height);
            if (_imageBitmap != null)
                e.Graphics.DrawImage(_imageBitmap, destRect);
            else
                e.Graphics.Clear(Color.Blue);
        }
        else
        {
            e.Graphics.Clear(Color.Blue);
        }
    }


    private static Bitmap CreateBitmap(int width, int height)
    {
        Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        return bitmap;
    }


    /// <summary>
    /// Efficiently copies an RGB byte array into an existing bitmap.
    /// The array must have the size width * height * 3 (RGB, no alpha, row-wise).
    /// </summary>
    /// <param name="bitmap">Target bitmap (PixelFormat.Format24bppRgb recommended)</param>
    /// <param name="rgbData">Source array (RGB, 3 bytes per pixel)</param>
    /// <param name="width">Image width in pixels</param>
    /// <param name="height">Image height in pixels</param>
    public static void CopyRgbToBitmap(Bitmap bitmap, byte[] rgbData, int width, int height)
    {
        if (bitmap.PixelFormat != PixelFormat.Format24bppRgb)
            throw new ArgumentException("Bitmap muss Format24bppRgb haben.");

        if (rgbData.Length != width * height * 3)
            throw new ArgumentException("rgbData hat nicht die erwartete Länge.");

        var rect = new Rectangle(0, 0, width, height);
        var bmpData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, bitmap.PixelFormat);

        try
        {
            int stride = bmpData.Stride;
            IntPtr ptr = bmpData.Scan0;

            if (stride == width * 3)
            {
                Marshal.Copy(rgbData, 0, ptr, rgbData.Length);
            }
            else
            {
                for (int y = 0; y < height; y++)
                {
                    Marshal.Copy(rgbData, y * width * 3, ptr + y * stride, width * 3);
                }
            }
        }
        finally
        {
            bitmap.UnlockBits(bmpData);
        }
    }
}
