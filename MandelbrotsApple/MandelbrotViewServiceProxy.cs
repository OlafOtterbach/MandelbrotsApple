namespace MandelbrotsApple;

using MandelbrotsApple.Mandelbrot;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Disposables;
using System.Linq;

public class MandelbrotViewServiceProxy : IMandelbrotViewServiceProxy, IDisposable
{
    private readonly MandelbrotViewService _service = new MandelbrotViewService();
    private readonly Subject<MoveLowAndFinalHigh> _mouseMoveSubject = new();
    private readonly Subject<ZoomLowAndHigh> _mouseWheelSubject = new();
    private readonly Subject<MaxIteration> _maxIterationsSubject = new();
    private readonly Subject<Refresh> _refreshViewSubject = new();
    private readonly Subject<MandelbrotResult> _drawSubject = new();
    private readonly Subject<Unit> _mouseResetSubject = new();
    private readonly MandelbrotViewAgent _serviceAgent;

    private readonly IDisposable _mouseMoveSubscription;
    private readonly IDisposable _mouseWheelSubscription;
    private readonly IDisposable _maxIterationsSubscription;
    private readonly IDisposable _refreshViewSubscription;

    private bool _disposed;

    public IObservable<MandelbrotResult> DrawObservable => _drawSubject.AsObservable();

    public MandelbrotViewServiceProxy()
    {
        _serviceAgent = new MandelbrotViewAgent(_service, _drawSubject);

        // Send low-res moves at most every 20ms (sample latest)
        // Send a final high-res move when 300ms of inactivity occurred (Throttle)
        var lowSub = _mouseMoveSubject
            .Sample(TimeSpan.FromMilliseconds(20))
            .Subscribe(move =>
            {
                // low-resolution move
                _serviceAgent.Tell(new Move(move.CurrentState, move.mandelbrotMovePosition, move.WidthLow, move.HeightLow));
            });

        var highSub = _mouseMoveSubject
            .Throttle(TimeSpan.FromMilliseconds(300))
            .Subscribe(move =>
            {
                // high-resolution final move after inactivity
                _serviceAgent.Tell(new Move(move.CurrentState,move.mandelbrotMovePosition, move.WidthHigh, move.HeightHigh));
            });

        _mouseMoveSubscription = new CompositeDisposable(lowSub, highSub);

        _mouseWheelSubscription = _mouseWheelSubject
            .Buffer(() => _mouseWheelSubject.Throttle(TimeSpan.FromMilliseconds(100)))
            .Where(buffer => buffer.Count > 0)
            .Select(buffer =>
            {
                int sum = 0;
                int x = buffer.First().X;
                int y = buffer.First().Y;
                foreach (var evt in buffer)
                {
                    sum += evt.ZoomIn ? evt.ZoomCount : -evt.ZoomCount;
                }
                bool zoomIn = sum >= 0;
                int zoomCount = Math.Abs(sum);
                var widthLow = buffer.First().WidthLow;
                var heightLow = buffer.First().HeightLow;
                var widthHigh = buffer.Last().WidthHigh;
                var heightHigh = buffer.Last().HeightHigh;
                var currentState = buffer.Last().CurrentState;
                return new ZoomLowAndHigh(currentState, zoomIn, zoomCount, x, y, widthLow, heightLow, widthHigh, heightHigh);
            })
            .Select(zoom =>
            {
                _serviceAgent.Tell(new Zoom(zoom.CurrentState, zoom.ZoomIn, zoom.ZoomCount, zoom.X, zoom.Y, zoom.WidthLow, zoom.HeightLow));
                return zoom;
            })
            .Throttle(TimeSpan.FromMilliseconds(300))
            .Do(zoom => _serviceAgent.Tell(new Refresh(zoom.WidthHigh, zoom.HeightHigh)))
            .Subscribe();

        _maxIterationsSubscription = _maxIterationsSubject
            .Sample(TimeSpan.FromMilliseconds(500))
            .Subscribe(iter => _serviceAgent.Tell(iter));

        _refreshViewSubscription = _refreshViewSubject
            .Sample(TimeSpan.FromMilliseconds(500))
            .Subscribe(resize => _serviceAgent.Tell(resize));
    }

    public void Init(Init init)
        => _serviceAgent.Tell(init);

    public void RefreshView(Refresh refresh)
        => _refreshViewSubject.OnNext(refresh);

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
        _refreshViewSubscription.Dispose();

        _mouseMoveSubject.Dispose();
        _mouseWheelSubject.Dispose();
        _maxIterationsSubject.Dispose();
        _refreshViewSubject.Dispose();
        _drawSubject.Dispose();
        _mouseResetSubject.Dispose();

        _disposed = true;
        GC.SuppressFinalize(this);
    }
}
