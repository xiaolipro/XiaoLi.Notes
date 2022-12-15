using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace ParallelPrograming.PLINQ;

public class AsParallel
{
    private readonly ITestOutputHelper _testOutputHelper;

    public AsParallel(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void 找素数()
    {
        IEnumerable<int> numbers = Enumerable.Range(3, 100000 - 3);

        var parallelQuery =
            from n in numbers.AsParallel()//.AsOrdered()
            where Enumerable.Range(2, (int)Math.Sqrt(n)).All(i => n % i > 0)
            select n;

        //var parallelQuery = numbers.AsParallel().Where(x => Enumerable.Range(2, (int)Math.Sqrt(x)).All(i => x % i > 0));

        int[] primes = parallelQuery.ToArray();

        _testOutputHelper.WriteLine(string.Join(",", primes));
    }
    
    
    [Fact]
    void LINQ()
    {
        IEnumerable<int> numbers = Enumerable.Range(3, 100000 - 3);

        var parallelQuery = numbers.Where(x => Enumerable.Range(2, (int)Math.Sqrt(x)).All(i => x % i > 0));

        int[] primes = parallelQuery.Skip(1).Take(3).ToArray();

        _testOutputHelper.WriteLine(string.Join(",", primes));
    }
}