namespace MandelbrotsApple;

using MandelbrotsApple.Mandelbrot;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

public class MandelbrotViewServiceProxy : IMandelbrotViewServiceProxy, IDisposable
{
    private readonly MandelbrotViewService _service = new MandelbrotViewService();
    private readonly Subject<MoveLowAndFinalHigh> _mouseMoveSubject = new();
    private readonly Subject<ZoomLowAndHigh> _mouseWheelSubject = new();
    private readonly Subject<MaxIteration> _maxIterationsSubject = new();
    private readonly Subject<Resize> _resizeViewSubject = new();
    private readonly Subject<MandelbrotResult> _drawSubject = new();
    private readonly Subject<Unit> _mouseResetSubject = new();
    private readonly MandelbrotViewAgent _serviceAgent;

    private readonly IDisposable _mouseMoveSubscription;
    private readonly IDisposable _mouseWheelSubscription;
    private readonly IDisposable _maxIterationsSubscription;
    private readonly IDisposable _resizeViewSubscription;

    private bool _disposed;

    public IObservable<MandelbrotResult> DrawObservable => _drawSubject.AsObservable();

    public MandelbrotViewServiceProxy()
    {
        _serviceAgent = new MandelbrotViewAgent(_service, _drawSubject);

        _mouseMoveSubscription =
            _mouseMoveSubject
            .Window(_mouseResetSubject.StartWith(Unit.Default)) // Startet neu bei jedem MouseDown
            .SelectMany(window =>
                window
                .Scan(
                    seed: (new MoveLowAndFinalHigh(0, 0, 0, 0, 0, 0, 0, 0), new MoveLowAndFinalHigh(0, 0, 0, 0, 0, 0, 0, 0)),
                    (acc, curr) => (acc.Item2, curr)
                )
                .Skip(1)
                .Select(pair =>
                {
                    _serviceAgent.Tell(new Move(pair.Item1.Vx, pair.Item1.Vy, pair.Item1.WidthLow, pair.Item1.HeightLow));
                    return pair;
                })
                .Throttle(TimeSpan.FromMilliseconds(300))
                .Do(pair => _serviceAgent.Tell(new Resize(pair.Item2.WidthHigh, pair.Item2.HeightHigh)))
            )
            .Subscribe();

        _mouseWheelSubscription = _mouseWheelSubject
                .Select(zoom =>
                {
                    _serviceAgent.Tell(new Zoom(zoom.ZoomIn, zoom.ZoomCount, zoom.X, zoom.Y, zoom.WidthLow, zoom.HeightLow));
                    return zoom;
                })
                .Throttle(TimeSpan.FromMilliseconds(300))
                .Do(zoom => _serviceAgent.Tell(new Resize(zoom.WidthHigh, zoom.HeightHigh)))
                .Subscribe();

        _maxIterationsSubscription = _maxIterationsSubject
            .Sample(TimeSpan.FromMilliseconds(500))
            .Subscribe(iter => _serviceAgent.Tell(iter));

        _resizeViewSubscription = _resizeViewSubject
            .Sample(TimeSpan.FromMilliseconds(500))
            .Subscribe(resize => _serviceAgent.Tell(resize));
    }

    public void Init(Init init)
        => _serviceAgent.Tell(init);

    public void ResizeView(Resize resize)
        => _resizeViewSubject.OnNext(resize);

    public void MaxIterations(MaxIteration maxIteration)
        => _maxIterationsSubject.OnNext(maxIteration);

    public void Move(MoveLowAndFinalHigh move)
        => _mouseMoveSubject.OnNext(move);

    public void Zoom(ZoomLowAndHigh zoom)
        => _mouseWheelSubject.OnNext(zoom);

    public void Reset()
    {
        _mouseResetSubject.OnNext(Unit.Default);
    }


    public void Dispose()
    {
        if (_disposed) return;
        _mouseMoveSubscription.Dispose();
        _mouseWheelSubscription.Dispose();
        _maxIterationsSubscription.Dispose();
        _resizeViewSubscription.Dispose();

        _mouseMoveSubject.Dispose();
        _mouseWheelSubject.Dispose();
        _maxIterationsSubject.Dispose();
        _resizeViewSubject.Dispose();
        _drawSubject.Dispose();
        _mouseResetSubject.Dispose();

        _disposed = true;
        GC.SuppressFinalize(this);
    }
}
