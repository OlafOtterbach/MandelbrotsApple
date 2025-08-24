namespace MandelbrotsApple;

using MandelbrotsApple.Mandelbrot;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

public class MandelbrotViewServiceProxy : IMandelbrotViewServiceProxy, IDisposable
{
    private readonly MandelbrotViewService _service = new MandelbrotViewService();
    private readonly Subject<MoveEvent> _mouseMoveSubject = new();
    private readonly Subject<WheelEvent> _mouseWheelSubject = new();
    private readonly Subject<IterationCommand> _maxIterationsSubject = new();
    private readonly Subject<(int width, int height)> _resizeViewSubject = new();
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
                    seed: (new MoveEvent(0, 0, 0, 0, 0, 0, 0, 0), new MoveEvent(0, 0, 0, 0, 0, 0, 0, 0)),
                    (acc, curr) => (acc.Item2, curr)
                )
                .Skip(1)
                .Select(pair =>
                {
                    _serviceAgent.Move(new MoveCommand(pair.Item1.Vx, pair.Item1.Vy, pair.Item1.WidthLow, pair.Item1.HeightLow));
                    return pair;
                })
                .Throttle(TimeSpan.FromMilliseconds(300))
                .Do(pair => _serviceAgent.Resize(new ResizeCommand(pair.Item2.WidthHigh, pair.Item2.HeightHigh)))
            )
            .Subscribe();

        _mouseWheelSubscription = _mouseWheelSubject
                .Select(wheel =>
                {
                    _serviceAgent.Wheel(new WheelCommand(wheel.ZoomIn, wheel.ZoomCount, wheel.X, wheel.Y, wheel.WidthLow, wheel.HeightLow));
                    return wheel;
                })
                .Throttle(TimeSpan.FromMilliseconds(300))
                .Do(wheel => _serviceAgent.Resize(new ResizeCommand(wheel.WidthHigh, wheel.HeightHigh)))
                .Subscribe();

        _maxIterationsSubscription = _maxIterationsSubject
            .Sample(TimeSpan.FromMilliseconds(500))
            .Subscribe(iter => _serviceAgent.Iterate(iter));

        _resizeViewSubscription = _resizeViewSubject
            .Sample(TimeSpan.FromMilliseconds(500))
            .Subscribe(args => _serviceAgent.Resize(new ResizeCommand(args.width, args.height)));
    }

    public void Init(int IterationPercentage, int width, int height)
        => _serviceAgent.Init(new Init(IterationPercentage, width, height));

    public void Reset()
    {
        _mouseResetSubject.OnNext(Unit.Default);
    }

    public void ResizeView(int width, int height)
        => _resizeViewSubject.OnNext((width, height));

    public void SetMaxIterations(IterationCommand iterationCommand)
        => _maxIterationsSubject.OnNext(iterationCommand);

    public void MouseMove(MoveEvent moveEvent)
        => _mouseMoveSubject.OnNext(moveEvent);

    public void MouseWheel(WheelEvent wheelEvent)
        => _mouseWheelSubject.OnNext(wheelEvent);

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