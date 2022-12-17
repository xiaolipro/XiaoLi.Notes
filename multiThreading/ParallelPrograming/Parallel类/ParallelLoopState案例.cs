using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ParallelPrograming.Parallel类;

public class ParallelLoopState案例
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ParallelLoopState案例(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void 退出循环()
    {
        var res = Parallel.ForEach("Hello, worldmmmmmmmmmmmmmmm", (c, state) =>
        {
            if (c == 'l') state.Break();

            _testOutputHelper.WriteLine(c.ToString());
        });
        
        _testOutputHelper.WriteLine(res.IsCompleted.ToString());
        _testOutputHelper.WriteLine(res.LowestBreakIteration.ToString());
        
    }
}