namespace MandelbrotsApple;

using MandelbrotsApple.Mandelbrot;
using System.Reactive.Subjects;
using System.Threading.Tasks.Dataflow;

public class MandelbrotViewAgent
{
    private class Message
    {
        public static Message CreateInit(Init init) => new Message { Init = new Init(init.IterationPercentage, init.Width, init.Height) };

        public static Message CreateMaxIteration(MaxIteration maxIteration) => new Message { MaxIteration = maxIteration };

        public static Message CreateResize(Resize resize) => new Message { Resize = resize };

        public static Message CreateMove(Move move) => new Message { Move = move };

        public static Message CreateZoom(Zoom zoom) => new Message { Zoom = zoom };

        public Init? Init { get; set; }

        public MaxIteration? MaxIteration { get; set; }

        public Resize? Resize { get; set; }

        public Move? Move { get; set; }

        public Zoom? Zoom { get; set; }
    }

    private readonly ActionBlock<Message> _actionBlock;

    private MandelbrotState _state = MandelbrotState.Empty;

    private readonly MandelbrotViewService _service = new MandelbrotViewService();

    public MandelbrotViewAgent(MandelbrotViewService viewService, Subject<MandelbrotResult> draw)
    {
        _service = viewService;

        _actionBlock = new ActionBlock<Message>(message =>
        {
            if(message.Init != null)
            {
                var initResult = _service.Init(message.Init.Value.IterationPercentage, message.Init.Value.Width, message.Init.Value.Height);
                if(!initResult.HasErrors)
                {
                    _state = new MandelbrotState(initResult.MandelbrotSize, initResult.MaxIterations);
                }

                draw.OnNext(initResult);
            }

            if (message.MaxIteration != null)
            {
                var iterationResult = _service.MaxIterations(_state.Size, message.MaxIteration.Value.IterationPercentage, message.MaxIteration.Value.Width, message.MaxIteration.Value.Height);
                if (!iterationResult.HasErrors)
                {
                    _state = new MandelbrotState(iterationResult.MandelbrotSize, iterationResult.MaxIterations);
                }

                draw.OnNext(iterationResult);
            }

            if (message.Resize.HasValue)
            {
                var resizeResult = _service.Resize(_state, message.Resize.Value.Width, message.Resize.Value.Height);
                if (!resizeResult.HasErrors)
                {
                    _state = new MandelbrotState(resizeResult.MandelbrotSize, resizeResult.MaxIterations);
                }

                draw.OnNext(resizeResult);
            }

            if (message.Move.HasValue)
            {
                var moveResult = _service.Move(_state, message.Move.Value.Vx, message.Move.Value.Vy, message.Move.Value.Width, message.Move.Value.Height);
                if (!moveResult.HasErrors)
                {
                    _state = new MandelbrotState(moveResult.MandelbrotSize, moveResult.MaxIterations);
                }

                draw.OnNext(moveResult);
            }

            if (message.Zoom.HasValue)
            {
                var zoomResult = _service.Zoom(_state, message.Zoom.Value.ZoomIn, message.Zoom.Value.ZoomCount, message.Zoom.Value.X, message.Zoom.Value.Y, message.Zoom.Value.Width, message.Zoom.Value.Height);
                if (!zoomResult.HasErrors)
                {
                    _state = new MandelbrotState(zoomResult.MandelbrotSize, zoomResult.MaxIterations);
                }

                draw.OnNext(zoomResult);
            }
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
