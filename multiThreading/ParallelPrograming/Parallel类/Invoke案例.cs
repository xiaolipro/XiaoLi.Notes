using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ParallelPrograming.Parallel类;

public class Invoke案例
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Invoke案例(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void Ping()
    {
        Parallel.Invoke(
            () => new Ping().Send("www.oreilly.com"),
            () => new Ping().Send("stackoverflow.com"));
    }


    [Fact]
    void 结果收集()
    {
        var res = new ConcurrentBag<string>();
        Parallel.Invoke(
            () =>
            {
                var p = new Ping().Send("www.oreilly.com");
                res.Add(p.Address + $" coast {p.Status}ms : " + p.RoundtripTime);
            },
            () =>
            {
                var p = new Ping().Send("stackoverflow.com");
                res.Add(p.Address + $" coast {p.Status}ms : " + p.RoundtripTime);
            });

        foreach (var item in res)
        {
            _testOutputHelper.WriteLine(item);
        }
    }

    [Fact]
    void 取消()
    {
        var res = new ConcurrentBag<string>();

        var source = new CancellationTokenSource();
        source.CancelAfter(1);

        Parallel.Invoke(
            new ParallelOptions
            {
                CancellationToken = source.Token,
            },
            () =>
            {
                var p = new Ping().Send("www.oreilly.com");
                res.Add(p.Address + $" coast {p.Status}ms : " + p.RoundtripTime);
            },
            () =>
            {
                var p = new Ping().Send("stackoverflow.com");
                res.Add(p.Address + $" coast {p.Status}ms : " + p.RoundtripTime);
            });

        foreach (var item in res)
        {
            _testOutputHelper.WriteLine(item);
        }
    }
}