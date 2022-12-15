using Xunit.Abstractions;

namespace ThreadingAdvance.WaitPulse;

public class 线程会和测试
{
    private readonly ITestOutputHelper _testOutputHelper;
    private 模拟CountdownEvent _countdown = new 模拟CountdownEvent(2);

    public 线程会和测试(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Show()
    {
        // 每个线程都睡眠一段随机时间
        Random r = new Random();
        new Thread(Mate).Start(r.Next(10000));
        Thread.Sleep(r.Next(10000));

        _countdown.Signal();
        _countdown.Wait();

        _testOutputHelper.WriteLine("Mate! ");
    }


    void Mate(object delay)
    {
        Thread.Sleep((int)delay);

        _countdown.Signal(); //+1
        _countdown.Wait();

        _testOutputHelper.WriteLine("Mate! ");
    }
}