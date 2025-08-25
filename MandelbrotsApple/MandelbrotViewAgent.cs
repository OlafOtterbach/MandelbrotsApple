namespace MandelbrotsApple;

using LaYumba.Functional;
using MandelbrotsApple.Mandelbrot;
using System.Reactive.Subjects;
using System.Threading.Tasks.Dataflow;
using static LaYumba.Functional.F;

public class MandelbrotViewAgent
{
    private class Message
    {
        public Message()
        {
            Init = None;
            MaxIteration = None;
            Resize = None;
            Move = None;
            Zoom = None;
        }

        public static Message CreateInit(Init init) => new Message { Init = Some(init) };

        public static Message CreateMaxIteration(MaxIteration maxIteration) => new Message { MaxIteration = Some(maxIteration) };

        public static Message CreateResize(Resize resize) => new Message { Resize = Some(resize) };

        public static Message CreateMove(Move move) => new Message { Move = Some(move) };

        public static Message CreateZoom(Zoom zoom) => new Message { Zoom = Some(zoom) };

        public Option<Init> Init { get; set; }

        public Option<MaxIteration> MaxIteration { get; private set; }

        public Option<Resize> Resize { get; private set; }

        public Option<Move> Move { get; private set; }

        public Option<Zoom> Zoom { get; private set; }
    }

    private readonly ActionBlock<Message> _actionBlock;

    private MandelbrotState _state = MandelbrotState.Empty;

    private readonly MandelbrotViewService _service = new MandelbrotViewService();

    public MandelbrotViewAgent(MandelbrotViewService viewService, Subject<MandelbrotResult> draw)
    {
        _service = viewService;

        _actionBlock = new ActionBlock<Message>(message =>
        {
            message.Init.Match(
                Some: init =>
                {
                    var initResult = _service.Init(init.IterationPercentage, init.Width, init.Height);
                    if (!initResult.HasErrors)
                    {
                        _state = new MandelbrotState(initResult.MandelbrotSize, initResult.MaxIterations);
                        draw.OnNext(initResult);
                    }
                },
                None: () => { });

            message.MaxIteration.Match(
                Some: maxIter =>
                {
                    var iterationResult = _service.MaxIterations(_state.Size, maxIter.IterationPercentage, maxIter.Width, maxIter.Height);
                    if (!iterationResult.HasErrors)
                    {
                        _state = new MandelbrotState(iterationResult.MandelbrotSize, iterationResult.MaxIterations);
                        draw.OnNext(iterationResult);
                    }
                },
                None: () => { });

            message.Resize.Match(
                Some: resize =>
                {
                    var resizeResult = _service.Resize(_state, resize.Width, resize.Height);
                    if (!resizeResult.HasErrors)
                    {
                        _state = new MandelbrotState(resizeResult.MandelbrotSize, resizeResult.MaxIterations);
                        draw.OnNext(resizeResult);
                    }
                },
                None: () => { });

            message.Move.Match(
                Some: move =>
                {
                    var moveResult = _service.Move(_state, move.Vx, move.Vy, move.Width, move.Height);
                    if (!moveResult.HasErrors)
                    {
                        _state = new MandelbrotState(moveResult.MandelbrotSize, moveResult.MaxIterations);
                        draw.OnNext(moveResult);
                    }
                },
                None: () => { });

            message.Zoom.Match(
                Some: zoom =>
                {
                    var zoomResult = _service.Zoom(_state, zoom.ZoomIn, zoom.ZoomCount, zoom.X, zoom.Y, zoom.Width, zoom.Height);
                    if (!zoomResult.HasErrors)
                    {
                        _state = new MandelbrotState(zoomResult.MandelbrotSize, zoomResult.MaxIterations);
                        draw.OnNext(zoomResult);
                    }
                },
                None: () => { });

        }, new ExecutionDataflowBlockOptions() { BoundedCapacity = -1 });
    }

    public void Init(Init init)
    {
        _actionBlock.Post(Message.CreateInit(init));
    }

    public void Iterate(MaxIteration maxIteration)
    {
        _actionBlock.Post(Message.CreateMaxIteration(maxIteration));
    }

    public void Resize(Resize resize)
    {
        _actionBlock.Post(Message.CreateResize(resize));
    }

    public void Move(Move move)
    {
        _actionBlock.Post(Message.CreateMove(move));
    }

    public void Zoom(Zoom zoom)
    {
        _actionBlock.Post(Message.CreateZoom(zoom));
    }
}
