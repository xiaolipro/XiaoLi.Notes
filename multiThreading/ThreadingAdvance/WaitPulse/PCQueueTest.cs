﻿using Xunit.Abstractions;

namespace ThreadingAdvance.WaitPulse;

public class PCQueueTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public PCQueueTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }


    [Fact]
    void 生产消费队列()
    {
        using (var queue = new PCQueue(_testOutputHelper, 2))
        {
            for (int i = 0; i < 10; i++)
            {
                int num = i;
                queue.AddTask(() =>
                {
                    _testOutputHelper.WriteLine("say " + num);
                    Thread.Sleep(1000);
                });
            }
        }
    }
}