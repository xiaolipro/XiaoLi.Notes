using System.Diagnostics;
using System.Runtime.InteropServices;
using Xunit.Abstractions;

namespace QuestionsAndReplaies;

public class _2Span和Memory
{
    private readonly ITestOutputHelper _testOutputHelper;

    public _2Span和Memory(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    
    [Fact]
    void SPAN()
    {
        Span<int> span = stackalloc int[100000];

        span.Slice(0, 100);
        var arr = new int[100000];
        var t = arr[1..2];
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