using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ParallelPrograming.Parallel类;

public class 索引
{
    private readonly ITestOutputHelper _testOutputHelper;

    public 索引(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void 不安全的()
    {
        int i = 0;
        Parallel.ForEach("Hello, worldmmmmmmmmmmmm", (c) => { _testOutputHelper.WriteLine(i++ + c.ToString()); });
    }

    [Fact]
    void 安全的()
    {
        Parallel.ForEach("Hello, worldmmmmmmmmmmmm", (c, state, i) =>
        {
            _testOutputHelper.WriteLine(i++ + c.ToString());
        });
    }
}