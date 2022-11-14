namespace ThreadingAdvance.WaitPulse;

public class 模拟信号量
{
    private readonly object _locker = new object();
    private int _count, _initialCount;
    public 模拟信号量(int initialCount)
    {
        _initialCount = initialCount;
    }
    
    void WaitOne()  // +1
    {
        lock (_locker)
        {
            _count++;
            while (_count >= _initialCount)
            {
                Monitor.Wait(_locker);
            }
        }
    }

    void Release()  // -1
    {
        lock (_locker)
        {
            _count --;
            Monitor.Pulse(_locker);
        }
    }
}