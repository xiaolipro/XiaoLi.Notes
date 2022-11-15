using Xunit.Abstractions;

namespace ThreadingAdvance.拓展;

public class 读写锁
{
    private readonly ITestOutputHelper _testOutputHelper;
    private ReaderWriterLockSlim _rw;
    private ThreadLocal<Random> _rand;
    private List<int> _items;

    public 读写锁(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _rw = new ReaderWriterLockSlim();
        _rand = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));
        _items = new List<int>();
    }

    [Fact]
    void Show()
    {
        new Thread(Read).Start();
        new Thread(Read).Start();
        new Thread(Read).Start();

        new Thread(Write).Start();
        new Thread(Write).Start();
        
        Thread.Sleep(1000);
    }

    void Read()
    {
        while (true)
        {
            _rw.EnterReadLock();
            foreach (int number in _items)
            {
                _testOutputHelper.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " reading " + number);
                Thread.Sleep(100);
            }
            _rw.ExitReadLock();
        }
    }

    void Write()
    {
        while (true)
        {
            _testOutputHelper.WriteLine(_rw.CurrentReadCount + " concurrent readers");
            int number = _rand.Value.Next(100);
            _rw.EnterWriteLock();
            _items.Add(number);
            _rw.ExitWriteLock();
            _testOutputHelper.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " added " + number);
            Thread.Sleep(100);
        }
    }
}