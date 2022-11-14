namespace ThreadingAdvance.WaitPulse;

public class 模拟事件等待句柄
{
    private readonly object _locker = new object();
    private bool _signal;

    void WaitOne()
    {
        lock (_locker)
        {
            while (!_signal) Monitor.Wait(_locker);
            // _signal = false;
        }
    }

    void Set()
    {
        lock (_locker)
        {
            _signal = true;
            Monitor.PulseAll(_locker);
            // Monitor.Pulse(_locker);
        }
    }

    void Reset()
    {
        lock (_locker) _signal = false;
    }
}