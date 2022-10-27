using Xunit.Abstractions;

namespace ThreadingBase;

public class _2传递参数
{
    private readonly ITestOutputHelper _testOutputHelper;

    public _2传递参数(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void 传参()
    {
        // string msg = "123"
        var t = new Thread(msg => Go(msg));  // 开一个线程t
        t.Start("hello world!");  // 启动t线程，执行Go方法
        
        Go("main thread say：hello world!");  // 主线程执行Go方法
    }

    void Go(object? msg)
    {
        _testOutputHelper.WriteLine(msg?.ToString());
    }


    [Fact]
    public void 闭包问题()
    {
        for (int i = 0; i < 10; i++)
        {
            int t = i;
            new Thread (() => Go(t)).Start();
        }
    }
}