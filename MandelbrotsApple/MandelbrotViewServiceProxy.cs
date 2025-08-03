namespace MandelbrotsApple;

using MandelbrotsApple.Mandelbrot;
using System.Reactive.Linq;
using System.Reactive.Subjects;

public class MandelbrotViewServiceProxy : IMandelbrotViewServiceProxy, IDisposable
{
    private readonly MandelbrotViewService _service = new MandelbrotViewService();
    private readonly Subject<(int x, int y)> _mouseMoveSubject = new();
    private readonly Subject<(bool zoomIn, int zoomCount, int x, int y)> _mouseWheelSubject = new();
    private readonly Subject<int> _maxIterationsSubject = new();
    private readonly Subject<(int resolutionPercentage, int width, int height)> _resolutionSubject = new();
    private readonly Subject<(int width, int height)> _resizeViewSubject = new();
    private readonly Subject<MandelbrotResult> _drawSubject = new();

    private readonly IDisposable _mouseMoveSubscription;
    private readonly IDisposable _mouseWheelSubscription;
    private readonly IDisposable _maxIterationsSubscription;
    private readonly IDisposable _resolutionSubscription;
    private readonly IDisposable _resizeViewSubscription;

    private bool _disposed;

    public IObservable<MandelbrotResult> DrawObservable => _drawSubject.AsObservable();

    public MandelbrotViewServiceProxy()
    {
        _mouseMoveSubscription = _mouseMoveSubject
            .Sample(TimeSpan.FromMilliseconds(300))
            .Select(pos => Observable.FromAsync(() => Task.Run(() => _service.MouseMove(pos.x, pos.y))))
            .Concat()
            .Subscribe(result => _drawSubject.OnNext(result));

        _mouseWheelSubscription = _mouseWheelSubject
            .Buffer(() => _mouseWheelSubject.Throttle(TimeSpan.FromMilliseconds(100)))
            .Where(buffer => buffer.Count > 0)
            .Select(buffer =>
            {
                int sum = 0;
                int x = buffer.First().x;
                int y = buffer.First().y;
                foreach (var evt in buffer)
                {
                    sum += evt.zoomIn ? evt.zoomCount : -evt.zoomCount;
                }
                bool zoomIn = sum >= 0;
                int zoomCount = Math.Abs(sum);
                return (zoomIn, zoomCount, x, y);
            })
            .Select(args => Observable.FromAsync(() => Task.Run(() => _service.MouseWheel(args.zoomIn, args.zoomCount, args.x, args.y))))
            .Concat()
            .Subscribe(result => _drawSubject.OnNext(result));

        _maxIterationsSubscription = _maxIterationsSubject
            .Throttle(TimeSpan.FromMilliseconds(500))
            .Select(iter => Observable.FromAsync(() => Task.Run(() => _service.SetMaxIterations(iter))))
            .Concat()
            .Subscribe(result => _drawSubject.OnNext(result));

        _resolutionSubscription = _resolutionSubject
            .Throttle(TimeSpan.FromMilliseconds(500))
            .Select(args => Observable.FromAsync(() => Task.Run(() => _service.SetResolution(args.resolutionPercentage, args.width, args.height))))
            .Concat()
            .Subscribe(result => _drawSubject.OnNext(result));

        _resizeViewSubscription = _resizeViewSubject
            .Sample(TimeSpan.FromMilliseconds(500))
            .Select(args => Observable.FromAsync(() => Task.Run(() => _service.ResizeView(args.width, args.height))))
            .Concat()
            .Subscribe(result => _drawSubject.OnNext(result));
    }

    public MandelbrotResult InitialView(int width, int height)
        => _service.InitialView(width, height);

    // Now handled via RX
    public void ResizeView(int width, int height)
        => _resizeViewSubject.OnNext((width, height));

    public void SetResolution(int resolutionPercentage, int width, int height)
        => _resolutionSubject.OnNext((resolutionPercentage, width, height));

    public void SetMaxIterations(int iterationPercentage)
        => _maxIterationsSubject.OnNext(iterationPercentage);

    public void SetMouseStart(int x, int y)
        => _service.SetMouseStart(x, y);

    public void MouseMove(int x, int y)
        => _mouseMoveSubject.OnNext((x, y));

    public void MouseWheel(bool zoomIn, int zoomCount, int x, int y)
        => _mouseWheelSubject.OnNext((zoomIn, zoomCount, x, y));

    public void Dispose()
    {
        if (_disposed) return;
        _mouseMoveSubscription.Dispose();
        _mouseWheelSubscription.Dispose();
        _maxIterationsSubscription.Dispose();
        _resolutionSubscription.Dispose();
        _resizeViewSubscription.Dispose();

        _mouseMoveSubject.Dispose();
        _mouseWheelSubject.Dispose();
        _maxIterationsSubject.Dispose();
        _resolutionSubject.Dispose();
        _resizeViewSubject.Dispose();
        _drawSubject.Dispose();

        _disposed = true;
        GC.SuppressFinalize(this);
    }
}