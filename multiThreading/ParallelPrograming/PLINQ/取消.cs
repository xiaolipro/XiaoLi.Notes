using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ParallelPrograming.PLINQ;

public class 取消
{
    private readonly ITestOutputHelper _testOutputHelper;

    public 取消(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void Show()
    {
        IEnumerable<int> numbers = Enumerable.Range(3, 1000000 - 3);

        var cancelSource = new CancellationTokenSource();

        var parallelQuery = numbers
            .AsParallel()
            //.WithMergeOptions(ParallelMergeOptions.FullyBuffered)
            .WithCancellation(cancelSource.Token)
            .Where(x => Enumerable.Range(2, (int)Math.Sqrt(x)).All(i => x % i > 0));

        Task.Run(() => { Thread.Sleep(2); cancelSource.Cancel(); });

        try
        {
            int cnt = 0;
            foreach (var prime in parallelQuery)
            {
                if (cnt % 500 == 0) _testOutputHelper.WriteLine(prime.ToString());
                cnt++;
            }
        }
        catch (OperationCanceledException e)
        {
            _testOutputHelper.WriteLine("工作已经被取消");
        }
    }
}