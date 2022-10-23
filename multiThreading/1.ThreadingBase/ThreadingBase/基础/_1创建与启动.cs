using Xunit.Abstractions;

namespace ThreadingBase;

public class _1创建与启动
{
    private readonly ITestOutputHelper _testOutputHelper;

    public _1创建与启动(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void 创建一个线程()
    {
        var t = new Thread(Go);  // 开一个线程t
        t.Start();  // 启动t线程，执行Go方法
        
        Go();  // 主线程执行Go方法
    }

    void Go()
    {
        // Thread.CurrentThread属性会返回当前执行的线程
        _testOutputHelper.WriteLine(Thread.CurrentThread.Name + " say: hello!");
    }

    [Fact]
    public void 线程命名()
    {
        var t = new Thread(Go);  // 开一个线程t
        t.Name = "worker";
        t.Start();  // 启动t线程，执行Go方法
        
        Go();  // 主线程执行Go方法
    }
}