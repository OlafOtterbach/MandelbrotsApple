using System.Threading.Tasks.Dataflow;

namespace MandelbrotsApple.Tracing;

public class TracingAgent
{
    private readonly ActionBlock<MandelbrotCommand> _actionBlock;
    private StringWriter? _writer = null;

    public TracingAgent()
    {
        _actionBlock = new ActionBlock<MandelbrotCommand>(command =>
        {
            switch (command)
            {
                case StartTrace start:
                    break;
                case StartAndState startAndState:
                    Start(startAndState);
                    break;
                case StopTrace stop:
                    Stop(stop);
                    break;
                default:
                    Trace(command);
                    break;
            }
        }, new ExecutionDataflowBlockOptions() { BoundedCapacity = -1 });
    }

    public void Tell(MandelbrotCommand command)
    {
        _actionBlock.Post(command);
    }


    private void Start(StartAndState start)
    {
        var state = start.State;
        var init = new Init(state.Size.Min.X, state.Size.Min.Y, state.Size.Max.X, state.Size.Max.Y, state.MaxIterations, 0, 0);
        InitStream();
        Trace(init);
    }

    private void Stop(StopTrace stop)
    {
        if(!IsTracing) return;

        CloseStream();
    }

    private void Trace(MandelbrotCommand command)
    {
        if(!IsTracing) return;

        switch (command)
        {
            case Init init:
                OnEvent($"Init({init.Xmin}, {init.Ymin}, {init.Xmax}, {init.Ymax}, {init.IterationPercentage})");
                break;
            case MaxIteration maxIteration:
                OnEvent($"MaxIteration({maxIteration.IterationPercentage})");
                break;
            case Refresh refresh:
                OnEvent($"Refresh()");
                break;
            case MoveLowAndFinalHigh moveLowAndFinalHigh:
                OnEvent($"MoveLowAndFinalHigh({moveLowAndFinalHigh.Vx}, {moveLowAndFinalHigh.Vy}, {moveLowAndFinalHigh.WidthLow}, {moveLowAndFinalHigh.HeightLow})");
                break;
            case Move move:
                OnEvent($"Move({move.Vx}, {move.Vy})");
                break;
            case ZoomLowAndHigh zoomLowAndHigh:
                OnEvent($"ZoomLowAndHigh({zoomLowAndHigh.ZoomIn}, {zoomLowAndHigh.ZoomCount}, {zoomLowAndHigh.X}, {zoomLowAndHigh.Y}, {zoomLowAndHigh.WidthLow}, {zoomLowAndHigh.HeightLow})");
                break;
            case Zoom zoom:
                OnEvent($"Zoom({zoom.ZoomIn}, {zoom.ZoomCount}, {zoom.X}, {zoom.Y})");
                break;
            default:
                break;
        }
    }



    private bool IsTracing => _writer != null;

    private void InitStream()
    {
        _writer = new StringWriter();
    }

    private void OnEvent(string message)
    {
        if (_writer != null)
        {
            _writer.WriteLine(message);
        }
    }

    private void CloseStream()
    {
        if (_writer != null)
        {
            Console.WriteLine("Stream-Inhalt:");
            Console.WriteLine(_writer.ToString());

            _writer.Close();
            _writer = null;
        }
    }
}
