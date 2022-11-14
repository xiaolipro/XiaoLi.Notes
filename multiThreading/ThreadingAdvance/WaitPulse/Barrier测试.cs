using Xunit.Abstractions;

namespace ThreadingAdvance.WaitPulse;

public class Barrier测试
{
    private readonly ITestOutputHelper _testOutputHelper;
    private Barrier _barrier = new Barrier(3);

    public Barrier测试(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        //_barrier = new Barrier(3, barrier => _testOutputHelper.WriteLine("Mate! "));
    }

    [Fact]
    void Show()
    {
        new Thread(Speak).Start();
        new Thread(Speak).Start();
        new Thread(Speak).Start();
    }

    void Speak()
    {
        for (int i = 0; i < 5; i++)
        {
            _testOutputHelper.WriteLine(i.ToString());
            _barrier.SignalAndWait();
        }
    }
}