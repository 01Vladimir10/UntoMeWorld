using Timer = System.Timers.Timer;

namespace UntoMeWorld.WasmClient.Client.Utils.Common;

public class ThrottleDispatcher
{
    private readonly Timer _timer;
    private Queue<Task> _tasks;

    public ThrottleDispatcher(TimeSpan delay)
    {
        _timer = new Timer
        {
            AutoReset = true,
            Enabled = true,
            Interval = delay.TotalMilliseconds,
            SynchronizingObject = null
        };
        _tasks = new Queue<Task>();
    }

    public void AddTask(Func<Task> function)
    {
        
    }

}