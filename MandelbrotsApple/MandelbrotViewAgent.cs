namespace MandelbrotsApple;

using MandelbrotsApple.Mandelbrot;
using System.Reactive.Subjects;
using System.Threading.Tasks.Dataflow;

public class MandelbrotViewAgent
{
    private readonly ActionBlock<Func<MandelbrotState, MandelbrotResult>> _actionBlock;

    private MandelbrotState _state;

    public MandelbrotViewAgent(Subject<MandelbrotResult> draw)
    {
        _actionBlock = new ActionBlock<Func<MandelbrotState, MandelbrotResult>>(command =>
        {
            var result = command(_state);
            if (!result.HasErrors)
            {
                _state = new MandelbrotState(result.MandelbrotSize, result.MaxIterations);
                draw.OnNext(result);
            }
        }, new ExecutionDataflowBlockOptions() { BoundedCapacity = -1 });
    }

    public void Tell(Func<MandelbrotState, MandelbrotResult> command)
    {
        _actionBlock.Post(command);
    }
}
