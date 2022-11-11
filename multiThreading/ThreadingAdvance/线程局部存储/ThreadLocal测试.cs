using Xunit.Abstractions;

namespace ThreadingAdvance.线程局部存储;

public class ThreadLocal测试
{
    ThreadLocal<int> _num = new ThreadLocal<int> (() => 3);
    private readonly ITestOutputHelper _testOutputHelper;


    public ThreadLocal测试(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void Show()
    {
        void Work()
        {
            for (int i = 0; i < 100000; i++)
            {
                _num.Value++;
            }
            _testOutputHelper.WriteLine(_num.ToString());
        }

        var t1 = new Thread(Work);
        var t2 = new Thread(Work);

        t1.Start();
        t2.Start();
        t1.Join();
        t2.Join();

        _testOutputHelper.WriteLine(_num.ToString());
    }
}