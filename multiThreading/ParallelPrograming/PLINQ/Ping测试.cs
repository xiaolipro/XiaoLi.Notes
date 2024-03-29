﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace ParallelPrograming.PLINQ;

public class Ping测试
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Ping测试(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void Ping()
    {
        new[]
            {
                "www.oreilly.com",
                "stackoverflow.com",
            }
            .AsParallel().WithDegreeOfParallelism(2).Select(site =>
            {
                var p = new Ping().Send(site);
                return new
                {
                    site,
                    Result = p.Status,
                    Time = p.RoundtripTime
                };
            }).ForAll(res => { _testOutputHelper.WriteLine(res.site + $" coast {res.Time}ms : " + res.Result); });
    }


    [Fact]
    void IOBUND()
    {
        var sw = new Stopwatch();
        sw.Start();
        
        Enumerable.Repeat(() => File.ReadLines("WordLookup.txt"), 240).AsParallel().WithDegreeOfParallelism(10).Select(x =>
        {
            var res = x.Invoke();
            var cnt = res.Count();
            return cnt;
        }).ToArray();
        sw.Stop();
        
        _testOutputHelper.WriteLine(sw.ElapsedMilliseconds.ToString());
    }
}