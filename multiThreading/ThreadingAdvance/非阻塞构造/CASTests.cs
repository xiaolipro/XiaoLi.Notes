using ThreadingAdvance.延迟初始化;
using Xunit.Abstractions;

namespace ThreadingAdvance.非阻塞构造;

public class CASTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private static int _num;

    public CASTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    void CAS()
    {
        var t1 = new Thread(() =>
        {
            for (int i = 0; i < 100000; i++)
            {
                Interlocked.CompareExchange(ref _num, 1000, 500);
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


    [Fact]
    void ABA()
    {
        int x = 500;
        var t1 = new Thread(() =>
        {
            Thread.Sleep(1000);
            if (Interlocked.CompareExchange(ref x, 1000, 500) == 500)
            {
                _testOutputHelper.WriteLine(x.ToString());
            }
        });
        var t2 = new Thread(() =>
        {
            x = 1000;
            Thread.MemoryBarrier();
            x = 500;
            Thread.MemoryBarrier();
        });
        t1.Start();
        t2.Start();
        t1.Join();
        t2.Join();
    }
    
    int a ,b;
    void Exam()
    {
        
        if (a > b)
            b = a;
    }


    [Fact]
    void 自旋CAS()
    {
        int oldVal, newVal;
        do
        {
            oldVal = b;
            if (a > oldVal)
                newVal = a;
            else
            {
                break;
            }
        } while (Interlocked.CompareExchange(ref b, newVal, oldVal) != oldVal);
    }
}

public static class InterlockedEx
{
    public static T Change<T>(ref T location, Func<T, T> operation) where T: class
    {
        T oldVal, newVal;
        do
        {
            oldVal = location;
            newVal = operation(oldVal);
        }
        while (Interlocked.CompareExchange(ref location, newVal, oldVal) != oldVal);
        return newVal;
    }
}