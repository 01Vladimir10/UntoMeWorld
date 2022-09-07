using System.Timers;
using Timer = System.Timers.Timer;

namespace UntoMeWorld.WasmClient.Client.Utils.Common;

public class ThrottleDispatcher : IDisposable
{
    private readonly Timer _timer;
    private readonly Queue<Func<Task>> _jobs;

    public ThrottleDispatcher(TimeSpan delay)
    {
        _timer = new Timer
        {
            AutoReset = true,
            Enabled = false,
            Interval = delay.TotalMilliseconds,
            SynchronizingObject = null
        };
        _timer.Elapsed += OnTimerElapsed;
        _jobs = new Queue<Func<Task>>();
    }

    private async void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        if (_jobs.Count < 1)
        {
            _timer.Stop();
            return;
        }
        await _jobs.Dequeue().Invoke();
    }
    public void AddTask(Func<Task> function)
    {
        _jobs.Enqueue(function);
        if (!_timer.Enabled)
            _timer.Start();
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}