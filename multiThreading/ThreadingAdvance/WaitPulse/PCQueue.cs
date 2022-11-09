using Xunit.Abstractions;

namespace ThreadingAdvance.WaitPulse;

public class PCQueue: IDisposable
{
    private readonly object _locker = new object();
    private Thread[] _workers;
    private Queue<Action> _queue = new Queue<Action>();
    
    private readonly ITestOutputHelper _testOutputHelper;

    public PCQueue(ITestOutputHelper testOutputHelper, int workerCount)
    {
        _testOutputHelper = testOutputHelper;

        _workers = new Thread[workerCount];
        for (int i = 0; i < workerCount; i++)
        {
            _workers[i] = new Thread(Consume);
            _workers[i].Start();
        }
    }

    public void AddTask(Action task)
    {
        lock (_locker)
        {
            _queue.Enqueue(task);
            Monitor.Pulse(_locker);
        }
    }

    private void Consume()
    {
        while (true)
        {
            Action task;
            lock (_locker)
            {
                while (_queue.Count == 0)
                {
                    Monitor.Wait(_locker);  // 队列里没任务，释放锁，进入等待
                }
                // 获取新任务，重新持有锁
                task = _queue.Dequeue();
            }
            
            if (task == null) return;  // 空任务代表退出
            task();  // 执行任务
        }
    }

    public void Dispose()
    {
        foreach (var worker in _workers)
        {
            AddTask(null);
        }

        foreach (var worker in _workers)
        {
            worker.Join();
        }
    }
}