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

        public static Message CreateStart(StartCommand startCommand) => new Message { Start = startCommand };

        public static Message CreateMove(MoveCommand moveCommand) => new Message { Move = moveCommand };

        public ResizeCommand? Resize { get; set; }

        public StartCommand? Start { get; set; }

        public MoveCommand? Move { get; set; }
    }

    private readonly ActionBlock<Message> _actionBlock;

    public MandelbrotViewAgent(MandelbrotViewService viewService, Subject<MandelbrotResult> draw)
    {
        _service = viewService;

        _actionBlock = new ActionBlock<Message>(message =>
        {
            if(message.Resize.HasValue)
            {
                var resizeResult = _service.ResizeView(message.Resize.Value.Width, message.Resize.Value.Height);
                draw.OnNext(resizeResult);
            }

            if (message.Start.HasValue)
            {
                _service.SetMouseStart(message.Start.Value.X, message.Start.Value.Y);
            }

            if (message.Move.HasValue)
            {
                var moveResult = _service.MouseMove(message.Move.Value.X, message.Move.Value.Y, message.Move.Value.Width, message.Move.Value.Height);
                draw.OnNext(moveResult);
            }
        }, new ExecutionDataflowBlockOptions() { BoundedCapacity = -1 });
    }

    public void Resize(ResizeCommand resizeCommand)
    {
        _actionBlock.Post(Message.CreateResize(resizeCommand));
    }

    public void Start(StartCommand startCommand)
    {
        _actionBlock.Post(Message.CreateStart(startCommand));
    }

    public void Move(MoveCommand moveCommand)
    {
        _actionBlock.Post(Message.CreateMove(moveCommand));
    }
}

