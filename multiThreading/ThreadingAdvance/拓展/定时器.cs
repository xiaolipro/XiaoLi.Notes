using Xunit.Abstractions;

namespace ThreadingAdvance.拓展;

public class 定时器
{
    private readonly ITestOutputHelper _testOutputHelper;

    public 定时器(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void Timer测试()
    {
        
        // 首次间隔1s，之后间隔500ms
        var timer = new Timer ((data) =>
        {
            _testOutputHelper.WriteLine(data.ToString());
        }, "tick...", 1000, 500);
        Thread.Sleep(3000);
        timer.Dispose();
    }
}