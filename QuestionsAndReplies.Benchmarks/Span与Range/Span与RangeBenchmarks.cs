using BenchmarkDotNet.Attributes;

namespace QuestionsAndReplies.Benchmarks.Span与Range;

[MemoryDiagnoser]
public class Span与RangeBenchmarks
{
    [Params(1, 10000)]
    public int start { get; set; }
    [Params(40001,100000)]
    public int end { get; set; }
    [Benchmark]
    public void Span()
    {
        var arr = new byte[100000];
        var bytes = arr.AsSpan().Slice(start, end).ToArray();
    }
    
    [Benchmark]
    public void Range()
    {
        var arr = new byte[100000];
        var bytes = arr[start..end];
    }
}