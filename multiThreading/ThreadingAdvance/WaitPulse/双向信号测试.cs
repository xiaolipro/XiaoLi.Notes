using Xunit.Abstractions;

namespace ThreadingAdvance.WaitPulse;

public class 双向信号测试
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly object _locker = new();
    private bool _entry; // 我是否可以工作了
    private bool _ready; // 我是否可以继续投递了

    public 双向信号测试(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void Show()
    {
        new Thread(() =>
        {
            for (int i = 0; i < 5; i++)
            {
                lock (_locker)
                {
                    _ready = true;
                    Monitor.PulseAll(_locker);
                    while (!_entry) Monitor.Wait(_locker);
                    _entry = false;
                    _testOutputHelper.WriteLine("Wassup?");
                }
            }
        }).Start();

        for (int i = 0; i < 5; i++)
        {
            lock (_locker)
            {
                while (!_ready) Monitor.Wait(_locker);
                _ready = false;
                _entry = true;
                Monitor.PulseAll(_locker);
            }
        }
    }
}