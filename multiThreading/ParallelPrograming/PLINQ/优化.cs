using System;
using System.Collections.Concurrent;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace ParallelPrograming.PLINQ;

public class 优化
{
    private readonly ITestOutputHelper _testOutputHelper;

    public 优化(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void 输入端优化()
    {
        var rangePartitioning = ParallelEnumerable.Repeat(1,10);
        
        int[] numbers = { 3, 4, 5, 6, 7, 8, 9 };
        var chunkPartitioning = Partitioner.Create(numbers, true).AsParallel();
        
    }

    [Fact]
    void 输出端优化()
    {
        "abcdef".AsParallel().Select(c => char.ToUpper(c)).ForAll(item => _testOutputHelper.WriteLine(item.ToString()));
    }
}