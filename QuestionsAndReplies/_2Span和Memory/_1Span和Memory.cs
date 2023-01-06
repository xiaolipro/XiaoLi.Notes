using System.Diagnostics;
using Xunit.Abstractions;

namespace QuestionsAndReplies._2Span和Memory;

public class _1Span和Memory
{
    private readonly ITestOutputHelper _testOutputHelper;

    public _1Span和Memory(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    
    [Fact]
    void SPAN()
    {
        Span<int> span = stackalloc int[10];
        var span2 = span.Slice(2, 20);
        span2[2] = 100;
        _testOutputHelper.WriteLine(span[4].ToString());
    }

    [Fact]
    void SPAN陷阱()
    {
        Span<int> span = stackalloc int[100000];

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int ctr = 0; ctr < span.Length; ctr++)
            span[ctr] = span[ctr] * span[ctr]/3;
        stopwatch.Stop();
        _testOutputHelper.WriteLine(stopwatch.Elapsed.TotalNanoseconds.ToString());
    }
}