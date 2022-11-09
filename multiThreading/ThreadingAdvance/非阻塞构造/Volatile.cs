using Xunit.Abstractions;

namespace ThreadingAdvance.非阻塞构造;

public class Volatile
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Volatile(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    //private int a, b;

    [Fact]
    // [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    void 重排序和缓存()
    {
        var dic = new Dictionary<(int, int), int>();
        for (int i = 0; i < 1e6; i++)
        {
            int a = 0, b = 0;
            int x = 0, y = 0;
            var task1 = Task.Run(() =>
            {
                Thread.VolatileWrite(ref a, 1);
                x = Thread.VolatileRead(ref b);
            });
            var task2 = Task.Run(() =>
            {
                Thread.VolatileWrite(ref b, 2);
                y = Thread.VolatileRead(ref a);
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