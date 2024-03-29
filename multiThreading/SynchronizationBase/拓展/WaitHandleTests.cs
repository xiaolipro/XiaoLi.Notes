﻿using Xunit.Abstractions;

namespace SynchronizationBase.拓展;

public class WaitHandleTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    

    public WaitHandleTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void WaitAnyTest()
    {
        EventWaitHandle wh1 = new ManualResetEvent(false);
        EventWaitHandle wh2 = new AutoResetEvent(false);
        
        new Thread(() =>
        {
            Thread.Sleep(3000);
            wh1.Set();
        }).Start();
        
        new Thread(() =>
        {
            Thread.Sleep(1000);
            wh2.Set();
        }).Start();
        WaitHandle.WaitAny(new[] { wh1, wh2 });
        _testOutputHelper.WriteLine("end");
    }
    
    
    [Fact]
    void WaitAllTest()
    {
        EventWaitHandle wh1 = new ManualResetEvent(false);
        EventWaitHandle wh2 = new AutoResetEvent(false);
        
        new Thread(() =>
        {
            Thread.Sleep(2000);
            wh1.Set();
        }).Start();
        
        new Thread(() =>
        {
            Thread.Sleep(1000);
            wh2.Set();
        }).Start();
        WaitHandle.WaitAll(new WaitHandle[] { wh1, wh2 });
        _testOutputHelper.WriteLine("end");
    }
    
    [Fact]
    void SignalAndWaitTest()
    {
        EventWaitHandle wh1 = new ManualResetEvent(false);
        EventWaitHandle wh2 = new AutoResetEvent(false);
        
        var t1 = new Thread(() =>
        {
            Thread.Sleep(1000);
            _testOutputHelper.WriteLine("current thread id：" + Thread.CurrentThread.ManagedThreadId);
            WaitHandle.SignalAndWait (wh1, wh2);
            _testOutputHelper.WriteLine("current thread id：" + Thread.CurrentThread.ManagedThreadId);
            // wh1.Set(); wh2.WaitOne();
            _testOutputHelper.WriteLine("ok");
        });
        
        var t2 = new Thread(() =>
        {
            _testOutputHelper.WriteLine("the work is blocked");
            _testOutputHelper.WriteLine("current thread id：" + Thread.CurrentThread.ManagedThreadId);
            wh1.WaitOne(); //阻塞 -1 等待set
            _testOutputHelper.WriteLine("current thread id：" + Thread.CurrentThread.ManagedThreadId);
            _testOutputHelper.WriteLine("working..");
            Thread.Sleep(5000);
            wh2.Set();
        });

        // var t3 = new Thread(() =>
        // {
        //     Thread.Sleep(3000);
        //     wh2.Set();
        // });
        
        t1.Start();
        t2.Start();
        // t3.Start();
        t1.Join();
        t2.Join();
        // t3.Join();
    }
    
    EventWaitHandle wh1 = new ManualResetEvent(false);
    EventWaitHandle wh2 = new AutoResetEvent(false);
    [Fact]
    public void Show()
    {
        // 每个线程都睡眠一段随机时间
        Random r = new Random();
        new Thread(Mate).Start(r.Next(10000));
        Thread.Sleep(r.Next(10000));

        WaitHandle.SignalAndWait (wh1, wh2);

        _testOutputHelper.WriteLine("Mate! ");
    }
    
    void Mate(object delay)
    {
        Thread.Sleep((int)delay);

        WaitHandle.SignalAndWait (wh2, wh1);

        _testOutputHelper.WriteLine("Mate! ");
    }
}