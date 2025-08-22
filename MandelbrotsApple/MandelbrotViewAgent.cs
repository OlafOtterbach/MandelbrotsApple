using MandelbrotsApple.Mandelbrot;
using System.Reactive.Subjects;
using System.Threading.Tasks.Dataflow;

namespace MandelbrotsApple;

public class MandelbrotViewAgent
{
    private readonly MandelbrotViewService _service = new MandelbrotViewService();

    private class Message
    {
        public static Message CreateResize(ResizeCommand resizeCommand) => new Message { Resize = resizeCommand };

        public static Message CreateMove(MoveCommand moveCommand) => new Message { Move = moveCommand };

        public static Message CreateWheel(WheelCommand wheelCommand) => new Message { Wheel = wheelCommand };

        public ResizeCommand? Resize { get; set; }

        public MoveCommand? Move { get; set; }

        public WheelCommand? Wheel { get; set; }
    }

    private readonly ActionBlock<Message> _actionBlock;

    public MandelbrotViewAgent(MandelbrotViewService viewService, Subject<MandelbrotResult> draw)
    {
        _service = viewService;

        _actionBlock = new ActionBlock<Message>(message =>
        {
            if (message.Resize.HasValue)
            {
                var resizeResult = _service.ResizeView(message.Resize.Value.Width, message.Resize.Value.Height);
                draw.OnNext(resizeResult);
            }

            if (message.Move.HasValue)
            {
                var moveResult = _service.MouseMove(message.Move.Value.Vx, message.Move.Value.Vy, message.Move.Value.Width, message.Move.Value.Height);
                draw.OnNext(moveResult);
            }

            if (message.Wheel.HasValue)
            {
                var wheelResult = _service.MouseWheel(message.Wheel.Value.ZoomIn, message.Wheel.Value.ZoomCount, message.Wheel.Value.X, message.Wheel.Value.Y, message.Wheel.Value.Width, message.Wheel.Value.Height);
                draw.OnNext(wheelResult);
            }
        }, new ExecutionDataflowBlockOptions() { BoundedCapacity = -1 });
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

