using Xunit.Abstractions;

namespace ThreadingAdvance.WaitPulse;

public class AutoResetEventTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly object _locker = new object();
    private bool _ok;
    private int _count;

    public AutoResetEventTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void Show()
    {
        new Thread(Work).Start();
        
        Thread.Sleep(3000); //休眠3s

        Pulse();
    }

    void Work()
    {
        lock (_locker)
        {
            while (!_ok)
            {
                Monitor.Wait(_locker);  // 释放锁，陷入等待
            }
            // 重新持有锁
        }

        _testOutputHelper.WriteLine("end");
    }

    void Pulse()
    {
        lock (_locker)
        {
            _ok = true;
            Monitor.Pulse(_locker);  // Monitor.PulseAll(_locker);
        }
    }
}