using Xunit.Abstractions;

namespace XiaoLi.NET.UnitTests;

public class SemaphoreTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(3, 5);
    public SemaphoreTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Theory]
    [InlineData(6)]
    void 循环测试SemaphoreSlim(int threadCnt)
    {
        Parallel.For(0, threadCnt, 进来玩);
    }

    // [Theory]
    // [InlineData(10)]
    // void 并行测试SemaphoreSlim(int threadCnt)
    // {
    //     Parallel.For(0, threadCnt, 进来玩);
    // }

    private void 进来玩(int x)
    {
        _testOutputHelper.WriteLine(x + "想进来");
        _semaphoreSlim.Wait(); // - 1 尝试进入房间   thread.sleep(-1)
        _testOutputHelper.WriteLine(x + "进来了");
        Thread.Sleep(1000);  // 业务逻辑
        _testOutputHelper.WriteLine(x + "溜了");
        _semaphoreSlim.Release(); // + 1 可用容量
    }
}