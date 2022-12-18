using Xunit.Abstractions;

namespace ThreadingAdvance.线程局部存储;

public class LocalDataStoreSlot测试
{
    private readonly ITestOutputHelper _testOutputHelper;

    public LocalDataStoreSlot测试(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void 命名槽()
    {
        // 同一个 LocalDataStoreSlot 对象可以跨线程使用。
        LocalDataStoreSlot _slot = Thread.AllocateNamedDataSlot("mySlot");
        void Work()
        {
            for (int i = 0; i < 100000; i++)
            {
                int num = (int)(Thread.GetData(_slot)??0);
                Thread.SetData(_slot, num + 1);
            }
            _testOutputHelper.WriteLine(((int)(Thread.GetData(_slot)??0)).ToString());
        }

        var t1 = new Thread(Work);
        var t2 = new Thread(Work);
        
        Thread.FreeNamedDataSlot("mySlot");

        t1.Start();
        t2.Start();
        t1.Join();
        t2.Join();

        _testOutputHelper.WriteLine(((int)(Thread.GetData(_slot)??0)).ToString());
    }

    public async Task F()
    {
        Console.WriteLine(); // 29
        await Task.Delay(1000);  // ? 1 2 218937
        Console.WriteLine();  // 29
    }
    
    [Fact]
    void 无名槽()
    {
        // 同一个 LocalDataStoreSlot 对象可以跨线程使用。
        LocalDataStoreSlot _slot = Thread.AllocateDataSlot();
        void Work()
        {
            for (int i = 0; i < 100000; i++)
            {
                int num = (int)(Thread.GetData(_slot)??0);
                Thread.SetData(_slot, num + 1);
            }
            _testOutputHelper.WriteLine(((int)(Thread.GetData(_slot)??0)).ToString());
        }

        var t1 = new Thread(Work);
        var t2 = new Thread(Work);

        t1.Start();
        t2.Start();
        t1.Join();
        t2.Join();

        _testOutputHelper.WriteLine(((int)(Thread.GetData(_slot)??0)).ToString());
    }


    #region 封装

    LocalDataStoreSlot _secSlot = Thread.GetNamedDataSlot ("securityLevel");
    int Num
    {
        get
        {
            object data = Thread.GetData(_secSlot);
            return data == null ? 0 : (int) data;    // null 相当于未初始化。
        }
        set { Thread.SetData (_secSlot, value); }
    }

    [Fact]
    void 封装()
    {
        void Work()
        {
            for (int i = 0; i < 100000; i++)
            {
                Num++;
            }
            _testOutputHelper.WriteLine(Num.ToString());
        }

        var t1 = new Thread(Work);
        var t2 = new Thread(Work);
        
        t1.Start();
        t2.Start();
        t1.Join();
        t2.Join();

        _testOutputHelper.WriteLine(Num.ToString());
    }

    #endregion
}