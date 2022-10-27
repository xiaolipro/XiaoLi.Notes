namespace ThreadingBase;

public class _6取消令牌
{
    [Fact]
    void 取消令牌()
    {
        var cancelSource = new CancellationTokenSource();
        cancelSource.CancelAfter(3000);
        var t = new Thread(() => Work(cancelSource.Token));
        t.Start();
        t.Join();
    }

    void Work(CancellationToken cancelToken)
    {
        while (true)
        {
            cancelToken.ThrowIfCancellationRequested();
            // ...
            Thread.Sleep(1000);
        }
    }
}