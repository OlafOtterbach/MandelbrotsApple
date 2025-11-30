namespace MandelbrotsApple;

using MandelbrotsApple.Mandelbrot;
using System.Reactive.Subjects;
using System.Threading.Tasks.Dataflow;

public static class MandelbrotViewAgentFactory
{
    public static Action<Func<MandelbrotState, MandelbrotResult>> Create(Subject<MandelbrotResult> draw)
    {
        ActionBlock<Func<MandelbrotState, MandelbrotResult>> actionBlock;
        MandelbrotState state = MandelbrotState.Empty;

        actionBlock = new ActionBlock<Func<MandelbrotState, MandelbrotResult>>(command =>
        {
            var result = command(state);
            if (!result.HasErrors)
            {
                state = new MandelbrotState(result.MandelbrotSize, result.MaxIterations);
                draw.OnNext(result);
            }
        }, new ExecutionDataflowBlockOptions() { BoundedCapacity = -1 });

        var tell = new Action<Func<MandelbrotState, MandelbrotResult>>(command =>
        {
            actionBlock.Post(command);
        });

        return tell;
    }
}
