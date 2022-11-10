using Xunit.Abstractions;

namespace ThreadingAdvance.非阻塞构造;

public class InterlockedTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private static int _num;

    public InterlockedTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void Example()
    {
        var t1 = new Thread(() =>
        {
            for (int i = 0; i < 100000; i++)
            {
                _num++;
            }
        });
        var t2 = new Thread(() =>
        {
            for (int i = 0; i < 100000; i++)
            {
                _num++;
            }
        });
        t1.Start();
        t2.Start();
        t1.Join();
        t2.Join();

        _testOutputHelper.WriteLine(_num.ToString());
    }


    [Fact]
    void Increment()
    {
        var t1 = new Thread(() =>
        {
            for (int i = 0; i < 100000; i++)
            {
                Interlocked.Increment(ref _num);
            }
        });
        var t2 = new Thread(() =>
        {
            for (int i = 0; i < 100000; i++)
            {
                Interlocked.Increment(ref _num);
            }
        });
        t1.Start();
        t2.Start();
        t1.Join();
        t2.Join();

        _testOutputHelper.WriteLine(_num.ToString());
    }
}