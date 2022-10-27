using System.ComponentModel;
using Xunit.Abstractions;

namespace ThreadingBase.进阶;

public class _3BackgroundWorker
{
    private readonly ITestOutputHelper _testOutputHelper;

    public _3BackgroundWorker(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void 基本用法()
    {
        BackgroundWorker worker = new BackgroundWorker();

        worker.DoWork += (sender, args) =>
        {
            _testOutputHelper.WriteLine(args.Argument.ToString());
            // working..
        };
        
        worker.RunWorkerAsync("hello");
    }

    private BackgroundWorker worker;
    [Fact]
    void 工作进度报告()
    {
        worker = new BackgroundWorker();
        worker.WorkerReportsProgress = true;  // 支持进度报告
        worker.WorkerSupportsCancellation = true;  // 支持取消
        worker.DoWork += DoWoker;
        worker.ProgressChanged += (_, args) => _testOutputHelper.WriteLine($"当前进度：{args.ProgressPercentage}%");
        worker.RunWorkerCompleted += (sender, args) =>
        {
            if (args.Cancelled) _testOutputHelper.WriteLine("工作线程已被取消");
            else if (args.Error != null) _testOutputHelper.WriteLine("工作线程发生异常: " + args.Error);
            else _testOutputHelper.WriteLine("任务完成，结果: " + args.Result); // Result来自DoWork
        };
        worker.RunWorkerAsync();        
    }

    private void DoWoker(object? sender, DoWorkEventArgs e)
    {
        for (int i = 0; i < 100; i+= 10)
        {
            if (worker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            worker.ReportProgress(i);  // 上报进度
            Thread.Sleep(1000);  // 模拟耗时任务
        }

        e.Result = int.MaxValue;  // 这个值会回传给RunWorkerCompleted
    }
}