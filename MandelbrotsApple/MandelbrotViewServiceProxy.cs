namespace MandelbrotsApple;

using MandelbrotsApple.Mandelbrot;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

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

        var moveSub = _mouseMoveSubject
            .Buffer(() => _mouseMoveSubject.Throttle(TimeSpan.FromMilliseconds(10)))
            .Where(buffer => buffer.Count > 0)
            .Where(buffer => buffer.Count > 0)
            .Select(buffer =>
            {
                var vx = buffer.Sum(evt => evt.ImageMoveVector.Vx);
                var vy = buffer.Sum(evt => evt.ImageMoveVector.Vy);
                var imageMoveVector = new ImageVector(vx, vy);
                var imageSizeLow = buffer.First().ImageSizeLow;
                return new Move(imageMoveVector, imageSizeLow);
            })
            .Subscribe(move => _serviceAgent.Tell(move));

        var moveEndSub = _mouseMoveSubject
            .Throttle(TimeSpan.FromMilliseconds(300))
            .Subscribe(move => _serviceAgent.Tell(new Refresh(move.ImageSizeHigh)));

        _mouseMoveSubscription = new CompositeDisposable(moveSub, moveEndSub);




        var duringWheelSub = _mouseWheelSubject
            .Buffer(() => _mouseWheelSubject.Throttle(TimeSpan.FromMilliseconds(10)))
            .Where(buffer => buffer.Count > 0)
            .Select(buffer =>
            {
                int sum = 0;
                foreach (var evt in buffer)
                {
                    sum += evt.ZoomIn ? evt.ZoomCount : -evt.ZoomCount;
                }
                bool zoomIn = sum >= 0;
                int zoomCount = Math.Abs(sum);
                var imagePosition = buffer.First().ImagePosition;
                var imageSizeLow = buffer.First().ImageSizeLow;
                var imageSizeHigh = buffer.Last().ImageSizeHigh;
                return new Zoom(zoomIn, zoomCount, imagePosition, imageSizeLow);
            })
            .Subscribe(zoom => _serviceAgent.Tell(zoom));

        var endWheelSub = _mouseWheelSubject
            .Throttle(TimeSpan.FromMilliseconds(300))
            .Subscribe(zoom => _serviceAgent.Tell(new Refresh(zoom.ImageSizeHigh)));

        _mouseWheelSubscription = new CompositeDisposable(duringWheelSub, endWheelSub);





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
