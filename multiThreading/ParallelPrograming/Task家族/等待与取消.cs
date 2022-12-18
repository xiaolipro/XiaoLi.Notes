using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ParallelPrograming.Task家族;

public class 等待与取消
{
    private readonly ITestOutputHelper _testOutputHelper;

    public 等待与取消(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void Wait()
    {
        Task task = Task.Run(() => Thread.Sleep(2000));
        _testOutputHelper.WriteLine("task status: {0}", task.Status);
        task.Wait(); // 无期限的等待
        _testOutputHelper.WriteLine("task status: {0}", task.Status);
    }

    [Fact]
    void 超时()
    {
        Task task = Task.Run(() => Thread.Sleep(2000));
        _testOutputHelper.WriteLine("task status: {0}", task.Status);
        task.Wait(1000); // 超过1秒就不等了
        _testOutputHelper.WriteLine("task status: {0}", task.Status);
    }

    [Fact]
    void 取消()
    {
        var source = new CancellationTokenSource();
        source.CancelAfter(1000);
        Task task = Task.Run(() => Thread.Sleep(2000));
        _testOutputHelper.WriteLine("task status: {0}", task.Status);
        try
        {
            task.Wait(source.Token); // 令牌被取消就抛异常
        }
        catch (OperationCanceledException e)
        {
            _testOutputHelper.WriteLine("令牌被取消了");
            _testOutputHelper.WriteLine("task status: {0}", task.Status);
        }
    }


    [Fact]
    void WaitAny()
    {
        var task1 = Task.Run(() =>
        {
            Thread.Sleep(new Random().Next(1000, 5000));
            _testOutputHelper.WriteLine("任务1干完了");
        });
        var task2 = Task.Run(() =>
        {
            Thread.Sleep(new Random().Next(1000, 5000));
            _testOutputHelper.WriteLine("任务2干完了");
        });
        var task3 = Task.Run(() =>
        {
            Thread.Sleep(new Random().Next(1000, 5000));
            _testOutputHelper.WriteLine("任务3干完了");
        });

        Task.WaitAny(task1, task2, task3);
    }
}