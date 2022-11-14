namespace ThreadingAdvance.WaitPulse;

public class 模拟CountdownEvent
{
    private object _locker = new object();
    private int _initialCount;

    public 模拟CountdownEvent(int initialCount)
    {
        _initialCount = initialCount;
    }

    public void Signal()  // +1
    {
        AddCount(1);
    }

    public void AddCount(int amount)  // +amount
    {
        lock (_locker)
        {
            _initialCount -= amount;
            if (_initialCount <= 0) Monitor.PulseAll(_locker);
        }
    }

    public void Wait()
    {
        lock (_locker)
        {
            while (_initialCount > 0)
                Monitor.Wait(_locker);
        }
    }
}