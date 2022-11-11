using System.Collections.Concurrent;
using Xunit.Abstractions;

namespace ThreadingAdvance.线程局部存储;

public class ThreadLocal测试2
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ThreadLocal测试2(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void Show()
    {
        var threadName = new ThreadLocal<string>(() => "Thread" + Thread.CurrentThread.ManagedThreadId);

        Parallel.For(0, 13, x =>
        {
            bool repeat = threadName.IsValueCreated;
            _testOutputHelper.WriteLine($"ThreadName = {threadName.Value} {(repeat ? "(repeat)" : "")}");
        });

        threadName.Dispose(); // 释放资源
    }


    [Fact]
    void 线程安全的随机数()
    {
        var localRandom = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));
        var list = new ConcurrentBag<int>();
        int threadCnt = 1, times = 100000;
        Parallel.For(0, threadCnt, x =>
        {
            for (int i = 0; i < times; i++)
            {
                int n = localRandom.Value.Next();
                list.Add(n);
                //_testOutputHelper.WriteLine(n.ToString());
            }
        });
        var set = new HashSet<int>(list);
        Assert.Equal(threadCnt *times, set.Count);
    }

    [Fact]
    void GUID真唯一吗()
    {
        var list = new ConcurrentBag<Guid>();
        Parallel.For(0, 20, x =>
        {
            for (int i = 0; i < 100000; i++)
            {
                list.Add(Guid.NewGuid());
                //_testOutputHelper.WriteLine(n.ToString());
            }
        });
        _testOutputHelper.WriteLine(list.Count.ToString());
        var set = new HashSet<Guid>(list);
        Assert.Equal(2000000, set.Count);
    }


    [Fact]
    void GUID的hashcode唯一吗()
    {
        var list = new ConcurrentBag<int>();
        var set = new HashSet<int>();
        Parallel.For(0, 1, x =>
        {
            for (int i = 0; i < 100000; i++)
            {
                list.Add(Guid.NewGuid().GetHashCode());
                set.Add(Thread.CurrentThread.ManagedThreadId);
                //_testOutputHelper.WriteLine(n.ToString());
            }
        });
        _testOutputHelper.WriteLine("集合总数"+list.Count.ToString());
        _testOutputHelper.WriteLine("参与线程总数"+set.Count);
        var arr = new int[list.Count];
        int len = list.Count;
        for (int i = 0; i < len; i++)
        {
             list.TryTake(out arr[i]);
        }
        Array.Sort(arr);
         int cnt = 0;
         for (int i = 0; i < len; i++)
         {
             if (i + 1 < len && arr[i + 1] == arr[i])
             {
                 while (i + 1< len && arr[i + 1] == arr[i])
                 {
                     _testOutputHelper.WriteLine($"第{i + 1}:{arr[i + 1]}和第{i}:{arr[i]}发生了碰撞");
                     cnt++;
                     i++;
                 }
             }
             
         }
        _testOutputHelper.WriteLine("重复数量=" + cnt);
        
        var set3 = new HashSet<int>(arr);
        _testOutputHelper.WriteLine("arr去重后=" + set3.Count);
    }
}