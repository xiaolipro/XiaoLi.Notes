using Xunit.Abstractions;

namespace ThreadingBase;

public class _3异常处理
{
    private readonly ITestOutputHelper _testOutputHelper;

    public _3异常处理(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    [Fact]
    void 异常捕获()
    {
        try
        {
            new Thread(Go).Start();  // 启动t线程，执行Go方法
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine("我发现异常了");
        }
    }
    
    void Go() => throw null!;  // 抛出空指针异常


    [Fact]
    void UnhandledException()
    {
        AppDomain.CurrentDomain.UnhandledException += HandleUnHandledException;
        new Thread(Go).Start();  // 启动t线程，执行Go方法
        
        Thread.Sleep(1000);
    }

    void HandleUnHandledException(object sender, UnhandledExceptionEventArgs eventArgs)
    {
        _testOutputHelper.WriteLine("我发现异常了");
    }

    [Fact]
    async Task 任务测试()
    {
        try
        {
            await GoAsync();
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine("我发现异常了");
        }
    }

    private async Task GoAsync()
    {
        await Task.Yield();
        throw null!;
    }
}