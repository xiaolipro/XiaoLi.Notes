using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace ParallelPrograming.PLINQ;

public class 聚合
{
    private readonly ITestOutputHelper _testOutputHelper;

    public 聚合(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void LINQ_SUM()
    {
        int sum = Enumerable.Range(1, 5_0000).Aggregate(0, (pre, cur) => pre + cur);
        int sum2 = Enumerable.Range(1, 5_0000).Sum();
        _testOutputHelper.WriteLine("sum: " + sum);
        _testOutputHelper.WriteLine("sum2: " + sum2);
    }


    [Fact]
    void PLINQ_SUM()
    {
        int sum = Enumerable.Range(1, 5_0000).AsParallel().Aggregate(0, (pre, cur) => pre + cur);
        _testOutputHelper.WriteLine("sum: " + sum);
    }

    [Fact]
    void PLINQ_SUM_SEED()
    {
        int sum = Enumerable.Range(1, 5_0000).AsParallel().Aggregate(
            () => 0,
            (pre, cur) => pre + cur,
            (main, local) => main + local,
            x => x);
        _testOutputHelper.WriteLine("sum: " + sum);
    }

    [Fact]
    void 统计字母出现频率()
    {
        string text = "Let’s suppose this is a really long string";
        var letterFrequencies = new int[26];
        foreach (char c in text)
        {
            int index = char.ToUpper(c) - 'A';
            if (index is >= 0 and <= 26) letterFrequencies[index]++;
        }

        ;
    }

    [Fact]
    void LINQ_统计字母出现频率()
    {
        "Let’s suppose this is a really long string"
            .Aggregate(
                new int[26],
                (pre, cur) =>
                {
                    int index = char.ToUpper(cur) - 'A';
                    if (index is >= 0 and <= 26) pre[index]++;
                    return pre;
                });
    }

    [Fact]
    void PLINQ_统计字母出现频率()
    {
        "Let’s suppose this is a really long string"
            .AsParallel()
            .Aggregate(
                () => new int[26],
                (pre, cur) =>
                {
                    int index = char.ToUpper(cur) - 'A';
                    if (index is >= 0 and <= 26) pre[index]++;
                    return pre;
                },
                (main, local) => main.Zip(local, (a, b) => a + b).ToArray(),
                x => x);
    }
}