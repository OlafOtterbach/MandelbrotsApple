using MandelbrotsApple.Mandelbrot;
using System.Reactive.Subjects;
using System.Threading.Tasks.Dataflow;

namespace MandelbrotsApple;

public class MandelbrotViewAgent
{
    private class Message
    {
        public static Message CreateInit(Init init) => new Message { Init = new Init(init.IterationPercentage, init.Width, init.Height) };

        public static Message CreateIteration(IterationCommand iterationCommand) => new Message { Iteration = iterationCommand };

        public static Message CreateResize(ResizeCommand resizeCommand) => new Message { Resize = resizeCommand };

        public static Message CreateMove(MoveCommand moveCommand) => new Message { Move = moveCommand };

        public static Message CreateWheel(WheelCommand wheelCommand) => new Message { Wheel = wheelCommand };

        public Init? Init { get; set; }

        public IterationCommand? Iteration { get; set; }

        public ResizeCommand? Resize { get; set; }

        public MoveCommand? Move { get; set; }

        public WheelCommand? Wheel { get; set; }
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
                var initResult = _service.InitialView(message.Init.Value.IterationPercentage, message.Init.Value.Width, message.Init.Value.Height);
                if(!initResult.HasErrors)
                {
                    _state = new MandelbrotState(initResult.MandelbrotSize, initResult.MaxIterations);
                }

                draw.OnNext(initResult);
            }

            if (message.Iteration != null)
            {
                var iterationResult = _service.SetMaxIterations(_state.Size, message.Iteration.Value.IterationPercentage, message.Iteration.Value.Width, message.Iteration.Value.Height);
                if (!iterationResult.HasErrors)
                {
                    _state = new MandelbrotState(iterationResult.MandelbrotSize, iterationResult.MaxIterations);
                }

                draw.OnNext(iterationResult);
            }

            if (message.Resize.HasValue)
            {
                var resizeResult = _service.ResizeView(_state, message.Resize.Value.Width, message.Resize.Value.Height);
                if (!resizeResult.HasErrors)
                {
                    _state = new MandelbrotState(resizeResult.MandelbrotSize, resizeResult.MaxIterations);
                }

                draw.OnNext(resizeResult);
            }

            if (message.Move.HasValue)
            {
                var moveResult = _service.MouseMove(_state, message.Move.Value.Vx, message.Move.Value.Vy, message.Move.Value.Width, message.Move.Value.Height);
                if (!moveResult.HasErrors)
                {
                    _state = new MandelbrotState(moveResult.MandelbrotSize, moveResult.MaxIterations);
                }

                draw.OnNext(moveResult);
            }

            if (message.Wheel.HasValue)
            {
                var wheelResult = _service.MouseWheel(_state, message.Wheel.Value.ZoomIn, message.Wheel.Value.ZoomCount, message.Wheel.Value.X, message.Wheel.Value.Y, message.Wheel.Value.Width, message.Wheel.Value.Height);
                if (!wheelResult.HasErrors)
                {
                    _state = new MandelbrotState(wheelResult.MandelbrotSize, wheelResult.MaxIterations);
                }

                draw.OnNext(wheelResult);
            }
        }, new ExecutionDataflowBlockOptions() { BoundedCapacity = -1 });
    }

    public void Init(Init init)
    {
        _actionBlock.Post(Message.CreateInit(init));
    }

    public void Iterate(IterationCommand iterationCommand)
    {
        _actionBlock.Post(Message.CreateIteration(iterationCommand));
    }

    public void Resize(ResizeCommand resizeCommand)
    {
        _actionBlock.Post(Message.CreateResize(resizeCommand));
    }

    public void Move(MoveCommand moveCommand)
    {
        _actionBlock.Post(Message.CreateMove(moveCommand));
    }

    public void Wheel(WheelCommand wheelCommand)
    {
        _actionBlock.Post(Message.CreateWheel(wheelCommand));
    }
}

