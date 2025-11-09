namespace MandelbrotsApple;

using MandelbrotsApple.Mandelbrot;
using System.Reactive.Subjects;
using System.Threading.Tasks.Dataflow;

public class MandelbrotViewAgent
{
    private readonly ActionBlock<MandelbrotCommand> _actionBlock;

    private readonly MandelbrotViewService _service = new MandelbrotViewService();

    private MandelbrotState _state;


    public MandelbrotViewAgent(MandelbrotViewService viewService, Subject<MandelbrotResult> draw)
    {
        _service = viewService;

        _actionBlock = new ActionBlock<MandelbrotCommand>(command =>
        {
            switch (command)
            {
                case Init init:
                    var initResult = _service.Init(init.MandelbrotSize, init.IterationPercentage, init.ImageSize);
                    if (!initResult.HasErrors)
                    {
                        _state = new MandelbrotState(initResult.MandelbrotSize, initResult.MaxIterations);
                        draw.OnNext(initResult);
                    }
                    break;
                case MaxIteration maxIteration:
                    var iterationResult = _service.MaxIterations(_state.Size, maxIteration.IterationPercentage, maxIteration.ImageSize);
                    if (!iterationResult.HasErrors)
                    {
                        _state = new MandelbrotState(iterationResult.MandelbrotSize, iterationResult.MaxIterations);
                        draw.OnNext(iterationResult);
                    }
                    break;
                case Refresh refresh:
                    var refreshResult = _service.Refresh(_state, refresh.ImageSize);
                    if (!refreshResult.HasErrors)
                    {
                        _state = new MandelbrotState(refreshResult.MandelbrotSize, refreshResult.MaxIterations);
                        draw.OnNext(refreshResult);
                    }
                    break;
                case Move move:
                    var moveResult = _service.Move(_state, move.ImageMoveVector, move.ImageSize);
                    if (!moveResult.HasErrors)
                    {
                        _state = new MandelbrotState(moveResult.MandelbrotSize, moveResult.MaxIterations);
                        draw.OnNext(moveResult);
                    }
                    break;
                case Zoom zoom:
                    var zoomResult = _service.Zoom(_state, zoom.ZoomIn, zoom.ZoomCount, zoom.ImagePosition, zoom.ImageSize);
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
