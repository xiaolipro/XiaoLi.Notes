using System.Collections.Concurrent;
using Xunit.Abstractions;

namespace SynchronizationBase.EventWaitHandles;

public class PCQueue:IDisposable
{
    private EventWaitHandle _waitHandle = new AutoResetEvent(false);
    private Thread _worker;
    private readonly object _locker = new object();
    private Queue<string> _tasks = new Queue<string>();

    private readonly ITestOutputHelper _testOutputHelper;

    public PCQueue(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _worker = new Thread(Work);
        _worker.Start();
    }

    public void AddTask(string task)
    {
        lock (_locker)
        {
            _tasks.Enqueue(task);
        }

        _waitHandle.Set();
    }

    private void Work()
    {
        while (true)
        {
            string task = null;
            lock (_locker)
            {
                if (_tasks.Count > 0)
                {
                    task = _tasks.Dequeue();
                    if (task == null) return;  // null为退出任务
                }
            }

            if (task == null)
            {
                _waitHandle.WaitOne();  // 没有任务，进入阻塞，等待新的任务
            }
            else
            {
                _testOutputHelper.WriteLine("执行任务：" +task);
                Thread.Sleep(1000);  // 模拟耗时任务
            }
        }
    }

    public void Dispose()
    {
        AddTask(null);  // 通知消费线程退出
        _worker.Join();  // 等待消费线程执行完成
        _waitHandle.Close();  // 释放事件句柄
    }
}