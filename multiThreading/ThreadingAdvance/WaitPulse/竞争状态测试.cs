using Xunit.Abstractions;

namespace ThreadingAdvance.WaitPulse;

public class 竞争状态测试
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly object _locker = new object();
    private bool _ok;

    public 竞争状态测试(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void Show()
    {
        new Thread(() =>
        {
            Thread.Sleep(100);
            for (int i = 0; i < 5; i++)
                lock (_locker)
                {
                    while (!_ok) Monitor.Wait(_locker);
                    _ok = false;
                    _testOutputHelper.WriteLine("Wassup?");
                }
        }).Start();

        for (int i = 0; i < 5; i++)
        {
            lock (_locker)
            {
                _ok = true;
                Monitor.Pulse(_locker);
            }
        }
    }
}