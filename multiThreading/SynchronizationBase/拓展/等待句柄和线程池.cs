﻿using Xunit.Abstractions;

namespace SynchronizationBase.拓展;

public class 等待句柄和线程池
{
    private readonly ITestOutputHelper _testOutputHelper;

    public 等待句柄和线程池(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void Show()
    {
        var _waitHandle = new ManualResetEvent(false);
        var reg = ThreadPool.RegisterWaitForSingleObject(_waitHandle, Work, "hahah", -1, true);
        Thread.Sleep(3000);
        _testOutputHelper.WriteLine("发送复位信号");
        _waitHandle.Set();
        reg.Unregister(_waitHandle);
    }
    
    private void Work (object data, bool timedOut)
    {
        _testOutputHelper.WriteLine ("Say - " + data);
        // 执行任务 ....
    }
}