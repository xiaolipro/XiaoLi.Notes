using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ParallelPrograming.Parallel类;

public class ForEachAsync案例
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ForEachAsync案例(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void 执行100个任务()
    {
        var tasks = Enumerable.Range(1, 100).Select(async x =>
        {
            await Task.Delay(1000);
            _testOutputHelper.WriteLine("线程 " + Thread.CurrentThread.ManagedThreadId + " 干了活" + x);
            return Task.CompletedTask;
        });
        Task.WaitAll(tasks.ToArray());
    }
    [Fact]
    void 信号量控制并发()
    {
        using var semaphore = new SemaphoreSlim(20, 20);
        var tasks = Enumerable.Range(1, 100).Select(async x =>
        {
            try
            {
                await semaphore.WaitAsync();
                await Task.Delay(1000);
                _testOutputHelper.WriteLine("线程 " + Thread.CurrentThread.ManagedThreadId + " 干了活" + x);
            }
            finally
            {
                semaphore.Release();
            }
        });

        Task.WaitAll(tasks.ToArray());
    }

    [Fact]
    async Task Parallel控制并发()
    {
        await Parallel.ForEachAsync(Enumerable.Range(1, 100), new ParallelOptions()
        {
            MaxDegreeOfParallelism = 10
        }, async (x, _) =>
        {
            await Task.Delay(1000);
            _testOutputHelper.WriteLine("线程 " + Thread.CurrentThread.ManagedThreadId + " 干了活" + x);
        });
    }
}