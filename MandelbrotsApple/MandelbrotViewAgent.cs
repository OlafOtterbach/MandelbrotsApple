namespace MandelbrotsApple;

using LaYumba.Functional;
using MandelbrotsApple.Mandelbrot;
using System.Reactive.Subjects;
using System.Threading.Tasks.Dataflow;
using static LaYumba.Functional.F;

public class MandelbrotViewAgent
{
    private readonly ActionBlock<MandelbrotCommand> _actionBlock;

    private MandelbrotState _state = MandelbrotState.Empty;

    private readonly MandelbrotViewService _service = new MandelbrotViewService();

    public MandelbrotViewAgent(MandelbrotViewService viewService, Subject<MandelbrotResult> draw)
    {
        _service = viewService;

        _actionBlock = new ActionBlock<MandelbrotCommand>(command =>
        {
            switch (command)
            {
                case Init init:
                    var initResult = _service.Init(init.IterationPercentage, init.Width, init.Height);
                    if (!initResult.HasErrors)
                    {
                        _state = new MandelbrotState(initResult.MandelbrotSize, initResult.MaxIterations);
                        draw.OnNext(initResult);
                    }
                    break;
                case MaxIteration maxIteration:
                    var iterationResult = _service.MaxIterations(_state.Size, maxIteration.IterationPercentage, maxIteration.Width, maxIteration.Height);
                    if (!iterationResult.HasErrors)
                    {
                        _state = new MandelbrotState(iterationResult.MandelbrotSize, iterationResult.MaxIterations);
                        draw.OnNext(iterationResult);
                    }
                    break;
                case Resize resize:
                    var resizeResult = _service.Resize(_state, resize.Width, resize.Height);
                    if (!resizeResult.HasErrors)
                    {
                        _state = new MandelbrotState(resizeResult.MandelbrotSize, resizeResult.MaxIterations);
                        draw.OnNext(resizeResult);
                    }
                    break;
                case Move move:
                    var moveResult = _service.Move(_state, move.Vx, move.Vy, move.Width, move.Height);
                    if (!moveResult.HasErrors)
                    {
                        _state = new MandelbrotState(moveResult.MandelbrotSize, moveResult.MaxIterations);
                        draw.OnNext(moveResult);
                    }
                    break;
                case Zoom zoom:
                    var zoomResult = _service.Zoom(_state, zoom.ZoomIn, zoom.ZoomCount, zoom.X, zoom.Y, zoom.Width, zoom.Height);
                    if (!zoomResult.HasErrors)
                    {
                        _state = new MandelbrotState(zoomResult.MandelbrotSize, zoomResult.MaxIterations);
                        draw.OnNext(zoomResult);
                    }
                    break;
            }
        }, new ExecutionDataflowBlockOptions() { BoundedCapacity = -1 });
    }

    public void Tell(MandelbrotCommand command)
    {
        _actionBlock.Post(command);
    }
}
