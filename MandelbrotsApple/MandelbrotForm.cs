namespace MandelbrotsApple;

using MandelbrotsApple.Mandelbrot;
using System.Drawing.Imaging;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public partial class MandelbrotForm : Form
{
    private IMandelbrotViewServiceProxy _mandelbrotViewServiceProxy = new MandelbrotViewServiceProxy();
    private Bitmap? _imageBitmap;
    private bool _mouseDown = false;
    private int _mouseX = 0;
    private int _mouseY = 0;
    private MandelbrotState _state;

    public MandelbrotForm()
    {
        InitializeComponent();

        _mandelbrotViewServiceProxy.DrawObservable
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
        int width = WidthHigh;
        int height = HeightHigh;
        if (width > 0 && height > 0)
        {
            var value = sliderIteration.Value;

            // 0.763, 0.0999, 0.768, 0.103;
            // -2.5, -3.0), 3.5, 3.0;
            // 0.5772661073307556, 0.6309067408680226, 0.5772670285961581, 0.630907662133425;
            _mandelbrotViewServiceProxy.Init(new Init(-2.5, -3.0, 3.5, 3.0, value, width, height));
        }
    }


    private void On_MandelbrotForm_Resize(object? sender, EventArgs e)
    {
        int width = WidthHigh;
        int height = HeightHigh;
        if (width > 0 && height > 0)
        {
            _mandelbrotViewServiceProxy.RefreshView(new Refresh(width, height));
        }
    }

    // MouseDown-Event
    private void On_CanvasPanel_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            _mouseX = XLow(e.X);
            _mouseY = YLow(e.Y);
            _mouseDown = true;
            _mandelbrotViewServiceProxy.Reset();
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
                var x = e.X;
                var y = e.Y;
                if (x != _mouseX || y != _mouseY)
                {
                    var vx = x - _mouseX;
                    var vy = y - _mouseY;
                    _mouseX = x;
                    _mouseY = y;

                    var mandelbrotMovePosition = GetMandelbrotMovePosition(
                        vx,
                        vy,
                        WidthHigh,
                        HeightHigh,
                        _state.Size);

                    _mandelbrotViewServiceProxy.Move(new MoveLowAndFinalHigh(_state, mandelbrotMovePosition, WidthLow, HeightLow, WidthHigh, HeightHigh));
                }
            }
        }
    }

    private static MandelbrotPosition GetMandelbrotMovePosition(
        double mouseVx,
        double mouseVy,
        double windowWidth,
        double windowHeight,
        MandelbrotSize mandelbrotSize)
    {
        // aspect ratio difference to squared image in y direction
        var diff = windowWidth - windowHeight;
        var virtualWindowHeight = windowHeight + diff;

        var mandelbrotMin = mandelbrotSize.Min;
        var mandelbrotMax = mandelbrotSize.Max;
        var mandelbrotVx = mouseVx * (mandelbrotMax.X - mandelbrotMin.X) / windowWidth;
        var mandelbrotVy = mouseVy * (mandelbrotMax.Y - mandelbrotMin.Y) / virtualWindowHeight;

        var newOrigin = new MandelbrotPosition(
            mandelbrotMin.X - mandelbrotVx,
            mandelbrotMin.Y - mandelbrotVy);

        return newOrigin;
    }




    // MouseWheel-Event
    private void On_CanvasPanel_MouseWheel(object? sender, MouseEventArgs e)
    {
        int delta = e.Delta;
        int wheelClicks = Math.Abs(delta / 120);
        var x = XLow(e.X);
        var y = YLow(e.Y);
        _mandelbrotViewServiceProxy.Zoom(new ZoomLowAndHigh(delta < 0, wheelClicks, x, y, WidthLow, HeightLow, WidthHigh, HeightHigh));
    }

    private void On_SliderIteration_Scroll(object sender, EventArgs e)
    {
        var value = sliderIteration.Value;
        _mandelbrotViewServiceProxy.MaxIterations(new MaxIteration(value, WidthHigh, HeightHigh));
    }


    private int XLow(int x) => (int)(x * LowFactor);

    private int YLow(int y) => (int)(y * LowFactor);

    private int WidthLow => (int)(canvasPanel.Width * LowFactor);

    private int HeightLow => (int)(canvasPanel.Height * LowFactor);

    private int WidthHigh => canvasPanel.Width;

    private int HeightHigh => canvasPanel.Height;

    private double LowFactor => 640.0 / Math.Max(canvasPanel.Width, canvasPanel.Height);


    private void DrawMandelbrotResult(MandelbrotResult result)
    {
        if (result.HasErrors)
            return;

        _state = new MandelbrotState(result.MandelbrotSize, result.MaxIterations);

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
