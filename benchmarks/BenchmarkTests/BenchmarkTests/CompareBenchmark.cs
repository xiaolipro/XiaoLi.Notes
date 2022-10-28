using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace BenchmarkTests;

[MemoryDiagnoser]
public class CompareBenchmark
{
    // 准备两个数组，填充4MB大小的数据
    private static readonly byte[] XBytes = Enumerable.Range(0, 4096000).Select(c => (byte) c).ToArray();
    private static readonly byte[] YBytes = Enumerable.Range(0, 4096000).Select(c => (byte) c).ToArray();
    
    public CompareBenchmark()
    {
        // 修改数组最后一个元素，使其不同
        XBytes[4095999] = 1;
        YBytes[4095999] = 2;
    }
    
    [Benchmark(Baseline = true)]
    public void 普通的for循环比较()
    {
        ForCompare(XBytes, YBytes);
    }
    
    [Benchmark]
    public void 内联优化的for循环比较()
    {
        ForCompareWithInlining(XBytes, YBytes);
    }
    
    [Benchmark]
    public void 基于memcmp比较()
    {
        BytesCompare.MemcmpCompare(XBytes, YBytes);
    }
    
    [Benchmark()]
    public void 字长64位优化算法()
    {
        BytesCompare.UlongCompare(XBytes, YBytes);
    }
    
    [Benchmark]
    public void 基于Sse指令集操作128位()
    {
        BytesCompare.Sse2Compare(XBytes, YBytes);
    }
    
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    private static bool ForCompare(byte[]? x, byte[]? y)
    {
        if (ReferenceEquals(x, y)) return true;		// 引用相等，可以直接认为相等
        if (x is null || y is null) return false;	// 两者引用不相等情况下,一方为null那就不相等
        if (x.Length != y.Length) return false;		// 两者长度不等，那么肯定也不相等
        for (var index = 0; index < x.Length; index++)
        {
            if (x[index] != y[index]) return false;
        }
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static bool ForCompareWithInlining(byte[]? x, byte[]? y)
    {
        if (ReferenceEquals(x, y)) return true;		// 引用相等，可以直接认为相等
        if (x is null || y is null) return false;	// 两者引用不相等情况下,一方为null那就不相等
        if (x.Length != y.Length) return false;		// 两者长度不等，那么肯定也不相等
        for (var index = 0; index < x.Length; index++)
        {
            if (x[index] != y[index]) return false;
        }
        return true;
    }
}