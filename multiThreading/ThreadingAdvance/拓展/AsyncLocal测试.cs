using Xunit.Abstractions;

namespace ThreadingAdvance.拓展;

public class AsyncLocal测试
{
    private readonly ITestOutputHelper _testOutputHelper;
    AsyncLocal<int> _asyncLocalNum = new AsyncLocal<int>();
    ThreadLocal<int> _threadLocalNum = new ThreadLocal<int>();

    public AsyncLocal测试(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }


    async Task Work()
    {
        for (int i = 0; i < 100000; i++)
        {
            _asyncLocalNum.Value++;
            _threadLocalNum.Value++;
        }
        _testOutputHelper.WriteLine(_asyncLocalNum.Value.ToString());
        _testOutputHelper.WriteLine(_threadLocalNum.Value.ToString());

        await Task.Delay(100);
        for (int i = 0; i < 100000; i++)
        {
            _asyncLocalNum.Value++;
            _threadLocalNum.Value++;
        }
        _testOutputHelper.WriteLine(_asyncLocalNum.Value.ToString());
        _testOutputHelper.WriteLine(_threadLocalNum.Value.ToString());
    }
    [Fact]
    async Task Show()
    {
        await Work();
    }
}