﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace ParallelPrograming.PLINQ;

public class 找素数
{
    private readonly ITestOutputHelper _testOutputHelper;

    public 找素数(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void PLINQ()
    {
        IEnumerable<int> numbers = Enumerable.Range(3, 100000 - 3);

        // var parallelQuery =
        //     from n in numbers.AsParallel()//.AsOrdered()
        //     where Enumerable.Range(2, (int)Math.Sqrt(n)).All(i => n % i > 0)
        //     select n;

        var parallelQuery = numbers.AsParallel().AsOrdered().Where(x => Enumerable.Range(2, (int)Math.Sqrt(x)).All(i => x % i > 0));

        int[] primes = parallelQuery.ToArray();

        _testOutputHelper.WriteLine(string.Join(",", primes));
    }
    
    
    [Fact]
    void LINQ()
    {
        IEnumerable<int> numbers = Enumerable.Range(3, 10_0000);

        var parallelQuery = numbers.Where(x => Enumerable.Range(2, (int)Math.Sqrt(x)).All(i => x % i > 0));

        int[] primes = parallelQuery.ToArray();

        _testOutputHelper.WriteLine(string.Join(",", primes));
    }


    [Fact]
    void 缓冲行为()
    {
        IEnumerable<int> numbers = Enumerable.Range(3, 100000 - 3);

        var parallelQuery = numbers.AsParallel().AsOrdered().WithMergeOptions(ParallelMergeOptions.AutoBuffered).Where(x => Enumerable.Range(2, (int)Math.Sqrt(x)).All(i => x % i > 0))
            .AsUnordered()
            //.Aggregate()
            .Where(x=>true);

        int[] primes = parallelQuery.ToArray();

        //_testOutputHelper.WriteLine(string.Join(",", primes));
    }


    [Fact]
    void Aggregate帶seed不能不行()
    {
        //Enumerable.Range(3, 100000 - 3).AsParallel().Aggregate()
    }
}