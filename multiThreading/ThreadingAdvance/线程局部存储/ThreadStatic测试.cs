using Xunit.Abstractions;

namespace ThreadingAdvance.线程局部存储;

public class ThreadStatic测试
{
    private readonly ITestOutputHelper _testOutputHelper;
    [ThreadStatic] private static int _num; // zero value

    public ThreadStatic测试(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void Show()
    {
        void Work()
        {
            for (int i = 0; i < 1e5; i++)
            {
                _num++;
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