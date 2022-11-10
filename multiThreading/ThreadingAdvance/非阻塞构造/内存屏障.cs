using System.Runtime.CompilerServices;
using ThreadingAdvance.延迟初始化;
using Xunit.Abstractions;

namespace ThreadingAdvance.非阻塞构造;

public class 内存屏障
{
    private readonly ITestOutputHelper _testOutputHelper;

    public 内存屏障(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    private int a, b, x, y;

    [Fact]
    // [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    void 重排序和缓存()
    {
        var dic = new Dictionary<(int, int), int>();
        
        for (int i = 0; i < 1e6; i++)
        {
            a = 0;
            b = 0;
            var task1 = Task.Run(() =>
            {
                a = 1;  // 写
                // Thread.MemoryBarrier();
                x = b;  // 读
            });
            var task2 = Task.Run(() =>
            {
                b = 2;  // 写
                //Thread.MemoryBarrier();
                y = a;  // 读
            });
            Task.WaitAll(task1, task2);
            Thread.Sleep(0);
            if (dic.ContainsKey((x, y))) dic[(x, y)]++;
            else dic.Add((x, y), 1);
        }

        foreach (var item in dic)
        {
            _testOutputHelper.WriteLine("x:" + item.Key.Item1 + " y:" + item.Key.Item2 + " " + item.Value);
        }
    }
}

public class Foo
{
    private int _answer;
    private bool _complete;

    private readonly ITestOutputHelper _testOutputHelper;

    public Foo(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    void A()
    {
        _answer = 10;
        //Thread.MemoryBarrier(); // 1
        _complete = true;
        //Thread.MemoryBarrier(); // 2
    }

    void B()
    {
        //Thread.MemoryBarrier(); // 3
        if (_complete)
        {
            _testOutputHelper.WriteLine(_answer.ToString());
        }
    }
}