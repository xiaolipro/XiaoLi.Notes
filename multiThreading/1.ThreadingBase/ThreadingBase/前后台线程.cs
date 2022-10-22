using Xunit.Abstractions;

namespace ThreadingBase;

public class 前后台线程
{
    private readonly ITestOutputHelper _testOutputHelper;

    public 前后台线程(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    void 创建一个前台线程()
    {
        Thread worker = new Thread ( () => _testOutputHelper.WriteLine("hello") );
        _testOutputHelper.WriteLine(worker.IsAlive.ToString());
        _testOutputHelper.WriteLine(worker.IsBackground.ToString());
        _testOutputHelper.WriteLine(worker.IsThreadPoolThread.ToString());

        worker.Start();
        
        _testOutputHelper.WriteLine(worker.IsAlive.ToString());
        _testOutputHelper.WriteLine(worker.IsBackground.ToString());
        _testOutputHelper.WriteLine(worker.IsThreadPoolThread.ToString());
        
        // Thread.Sleep(100);
        // _testOutputHelper.WriteLine(worker.IsAlive.ToString());
    }    
}