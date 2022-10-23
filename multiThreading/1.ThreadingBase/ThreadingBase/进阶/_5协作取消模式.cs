using Xunit.Abstractions;

namespace ThreadingBase;

public class MyCanceler
{
    private readonly object _locker = new object();
    private bool _cancelRequest = false;

    public bool IsCancellationRequested
    {
        get
        {
            lock (_locker) return _cancelRequest;
        }
    }

    public void Cancel()
    {
        lock (_locker)
        {
            _cancelRequest = true;
        }
    }

    public void ThrowIfCancellationRequested()
    {
        if (IsCancellationRequested) throw new OperationCanceledException();
    }
}

public class _5协作取消模式
{
    private readonly ITestOutputHelper _testOutputHelper;

    public _5协作取消模式(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    void 自定义取消()
    {
        var canceler = new MyCanceler();
        new Thread(() =>
        {
            try
            {
                DoWork(canceler);
            }
            catch (OperationCanceledException e)
            {
                _testOutputHelper.WriteLine("任务已取消");
                throw;
            }
        }).Start();
        
        Thread.Sleep(2000);
        canceler.Cancel();
    }

    private void DoWork(MyCanceler canceler)
    {
        while (true)
        {
            canceler.ThrowIfCancellationRequested();
            
            try
            {
                // 干活
            }
            finally
            {
                // 清理
            }
        }
    }
}

