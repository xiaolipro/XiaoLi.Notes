using Xunit.Abstractions;

namespace ThreadingBase;

public class _4中断与中止
{
    private readonly ITestOutputHelper _testOutputHelper;

    public _4中断与中止(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void 中断()
    {
        var t = new Thread(delegate()
        {
            try
            {
                Thread.Sleep(Timeout.Infinite); // 无期限休眠
            }
            catch (ThreadInterruptedException)
            {
                _testOutputHelper.WriteLine("收到中断信号");
            }

            _testOutputHelper.WriteLine("溜溜球~");
        });
        t.Start();
        Thread.Sleep(3000); // 睡3s后中断线程t
        t.Interrupt();
    }


    [Fact]
    void 中断不处理()
    {
        var t = new Thread(delegate()
        {
            Thread.Sleep(Timeout.Infinite); // 无期限休眠
            _testOutputHelper.WriteLine("收到中断信号");

            _testOutputHelper.WriteLine("溜溜球~");
        });
        t.Start();
        Thread.Sleep(3000); // 睡3s后中断线程t
        t.Interrupt();
    }


    [Fact]
    void 中止()
    {
        Thread t = new Thread(delegate()
        {
            try
            {
                while (true)
                {
                }
            }
            catch (ThreadAbortException)
            {
                _testOutputHelper.WriteLine("收到中止信号");
            }
            // 这里仍然会继续抛出ThreadAbortException，以保证此线程真正中止
        });

        _testOutputHelper.WriteLine(t.ThreadState.ToString()); // Unstarted 状态

        t.Start();
        Thread.Sleep(1000);
        _testOutputHelper.WriteLine(t.ThreadState.ToString()); // Running 状态

        t.Abort();
        _testOutputHelper.WriteLine(t.ThreadState.ToString()); // AbortRequested 状态

        t.Join();
        _testOutputHelper.WriteLine(t.ThreadState.ToString()); // Stopped 状态
    }
}