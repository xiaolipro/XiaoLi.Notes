using Xunit.Abstractions;

namespace ThreadingAdvance.拓展;

public class AsyncLocal测试
{
    private readonly ITestOutputHelper _testOutputHelper;
    AsyncLocal<int> _num = new AsyncLocal<int>();
    ThreadLocal<int> _num2 = new ThreadLocal<int>();

    public AsyncLocal测试(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }


    [Fact]
    async Task Show()
    {
        async Task Work()
        {
            for (int i = 0; i < 100000; i++)
            {
                _num.Value++;
                _num2.Value++;
            }
            _testOutputHelper.WriteLine(_num.Value.ToString());
            _testOutputHelper.WriteLine(_num2.Value.ToString());

            await Task.Delay(100);
            for (int i = 0; i < 100000; i++)
            {
                _num.Value++;
                _num2.Value++;
            }
            _testOutputHelper.WriteLine(_num.Value.ToString());
            _testOutputHelper.WriteLine(_num2.Value.ToString());
        }

        await Work();

        _testOutputHelper.WriteLine(_num.Value.ToString());
        _testOutputHelper.WriteLine(_num2.Value.ToString());
    }
}