using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ParallelPrograming.Task家族;

public class 异常
{
    private readonly ITestOutputHelper _testOutputHelper;

    public 异常(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void Task内部异常()
    {
        var task = Task.Run(() =>
        {
            _testOutputHelper.WriteLine("hh");
            throw new Exception();
        });

        Thread.Sleep(1000);
        _testOutputHelper.WriteLine("我ok啊");
        _testOutputHelper.WriteLine("task status：" + task.Status);  // task status：Faulted
    }

    [Fact]
    void Show()
    {
        // Create a cancellation token and cancel it.
        var source1 = new CancellationTokenSource();
        var token1 = source1.Token;
        source1.Cancel();
        // Create a cancellation token for later cancellation.
        var source2 = new CancellationTokenSource();
        var token2 = source2.Token;

        // Create a series of tasks that will complete, be cancelled, 
        // timeout, or throw an exception.
        Task[] tasks = new Task[12];
        for (int i = 0; i < 12; i++)
        {
            switch (i % 4)
            {
                // Task should run to completion.
                case 0:
                    tasks[i] = Task.Run(() => Thread.Sleep(2000));
                    break;
                // Task should be set to canceled state.
                case 1:
                    tasks[i] = Task.Run(() => Thread.Sleep(2000),
                        token1);
                    break;
                case 2:
                    // Task should throw an exception.
                    tasks[i] = Task.Run(() => { throw new NotSupportedException(); });
                    break;
                case 3:
                    // Task should examine cancellation token.
                    tasks[i] = Task.Run(() =>
                    {
                        Thread.Sleep(2000);
                        if (token2.IsCancellationRequested)
                            token2.ThrowIfCancellationRequested();
                        Thread.Sleep(500);
                    }, token2);
                    break;
            }
        }

        Thread.Sleep(250);
        source2.Cancel();

        try
        {
            Task.WaitAll(tasks);
        }
        catch (AggregateException ae)
        {
            _testOutputHelper.WriteLine("One or more exceptions occurred:");
            foreach (var ex in ae.InnerExceptions)
                _testOutputHelper.WriteLine("   {0}: {1}", ex.GetType().Name, ex.Message);
        }

        _testOutputHelper.WriteLine("\nStatus of tasks:");
        foreach (var t in tasks)
        {
            _testOutputHelper.WriteLine("   Task #{0}: {1}", t.Id, t.Status);
            if (t.Exception != null)
            {
                foreach (var ex in t.Exception.InnerExceptions)
                    _testOutputHelper.WriteLine("      {0}: {1}", ex.GetType().Name,
                        ex.Message);
            }
        }
    }
}