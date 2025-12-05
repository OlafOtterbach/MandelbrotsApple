namespace MandelbrotsApple;

using MandelbrotsApple.Mandelbrot;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using static MandelbrotViewRequestFactory;
using static MandelbrotViewAgentFactory;

public static class MandelbrotViewProxy
{
    public static Action<IMandelbrotCommand> CreateRequestCall(Subject<MandelbrotResult> resultCallBack)
    {
        var tell = CreateTellAgent(resultCallBack);
        var doInit = CreateInit(tell);
        var doRefresh = CreateRefresh(tell);
        var doMaxIterations = CreateMaxIterations(tell);
        var doMove = CreateMove(tell);
        var doZoom = CreateZoom(tell);
        var doRequest = (IMandelbrotCommand command) =>
        {
            switch (command)
            {
                case Init init:
                    doInit(init);
                    break;

                case Refresh refresh:
                    doRefresh(refresh);
                    break;

                case MaxIteration maxIter:
                    doMaxIterations(maxIter);
                    break;

                case MoveLowAndFinalHigh moveLowAndFinal:
                    doMove(moveLowAndFinal);
                    break;

                case ZoomLowAndFinalHigh zoomLowAndFinal:
                    doZoom(zoomLowAndFinal);
                    break;

                default:
                    // unknown command 
                    break;
            }
        };

        return doRequest;
    }

    private static Action<Init> CreateInit(Action<Func<MandelbrotState, MandelbrotResult>> tell)
    {
        var doInit = (Init init) => tell(RequestInit(init));
        return doInit;
    }

    private static Action<Refresh> CreateRefresh(Action<Func<MandelbrotState, MandelbrotResult>> tell)
    {
        var refreshViewSubject = new Subject<Refresh>();
        var refreshViewSubscription = refreshViewSubject
            .Sample(TimeSpan.FromMilliseconds(500))
            .Subscribe(refresh => tell(RequestRefresh(refresh)));

        var doRefresh = (Refresh refresh) => refreshViewSubject.OnNext(refresh);
        return doRefresh;
    }

    private static Action<MaxIteration> CreateMaxIterations(Action<Func<MandelbrotState, MandelbrotResult>> tell)
    {
        var maxIterationsSubject = new Subject<MaxIteration>();

        var maxIterationsSubscription = maxIterationsSubject
            .Throttle(TimeSpan.FromMilliseconds(500))
            .Subscribe(iter => tell(RequestMaxIteration(iter)));

        var doMaxIterations = (MaxIteration maxIterations) => maxIterationsSubject.OnNext(maxIterations);
        return doMaxIterations;
    }

    private static Action<MoveLowAndFinalHigh> CreateMove(Action<Func<MandelbrotState, MandelbrotResult>> tell)
    {
        var mouseMoveSubject = new Subject<MoveLowAndFinalHigh>();

        var moveSub = mouseMoveSubject
            .Buffer(() => mouseMoveSubject.Throttle(TimeSpan.FromMilliseconds(10)))
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
            .Subscribe(move => tell(RequestMove(move)));

        var moveEndSub = mouseMoveSubject
            .Throttle(TimeSpan.FromMilliseconds(300))
            .Subscribe(moveLowAndFinalHight => tell(RequestRefresh(moveLowAndFinalHight)));

        var mouseMoveSubscription = new CompositeDisposable(moveSub, moveEndSub);

        var doMove = (MoveLowAndFinalHigh move) => mouseMoveSubject.OnNext(move);

        return doMove;
    }

    private static Action<ZoomLowAndFinalHigh> CreateZoom(Action<Func<MandelbrotState, MandelbrotResult>> tell)
    {
        var zoomSubject = new Subject<ZoomLowAndFinalHigh>();

        var duringZoomSub = zoomSubject
            .Buffer(() => zoomSubject.Throttle(TimeSpan.FromMilliseconds(10)))
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
            .Subscribe(zoom => tell(RequestZoom(zoom)));

        var endZoomSub = zoomSubject
            .Throttle(TimeSpan.FromMilliseconds(300))
            .Subscribe(zoomLowAndFinalHight => tell(RequestRefresh(zoomLowAndFinalHight)));

        var mouseWheelSubscription = new CompositeDisposable(duringZoomSub, endZoomSub);

        var doZoom = (ZoomLowAndFinalHigh move) => zoomSubject.OnNext(move);

        return doZoom;
    }
}
