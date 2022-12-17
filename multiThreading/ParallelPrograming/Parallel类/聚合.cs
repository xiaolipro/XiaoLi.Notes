using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ParallelPrograming.Parallel类;

public class 聚合
{
    private readonly ITestOutputHelper _testOutputHelper;

    public 聚合(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void 普通聚合()
    {
        var locker = new object();
        double total = 0;
        Parallel.For(1, 1000_0000, x =>
        {
            lock (locker)
            {
                total += Math.Sqrt(x);
            }
        });

        _testOutputHelper.WriteLine(total.ToString());
    }


    [Fact]
    void 局部聚合()
    {
        var locker = new object();
        double total = 0;
        Parallel.For(1, 1000_0000,
            () => 0.0,
            (x, state, local) => local + Math.Sqrt(x),
            local =>
            {
                lock (locker)
                {
                    total += local;
                }
            });

        _testOutputHelper.WriteLine(total.ToString());
    }


    [Fact]
    void PLINQ()
    {
        var res = Enumerable.Range(1, 1000_0000).AsParallel()
            .Sum(x => Math.Sqrt(x));

        _testOutputHelper.WriteLine(res.ToString());
    }
    
    [Fact]
    void PLINQ_Range()
    {
        var res = ParallelEnumerable.Range(1, 1000_0000)
            .Sum(x => Math.Sqrt(x));

        _testOutputHelper.WriteLine(res.ToString());
    }
}