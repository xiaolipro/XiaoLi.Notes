namespace ThreadingAdvance.延迟初始化;

public class A
{
    public readonly Expensive Expensive = new Expensive();
    // ..
}

public class Expensive
{
    // 构造开销非常昂贵
}

public class B
{
    private Expensive _expensive;

    public Expensive GetExpensiveInstance()
    {
        if (_expensive == null) _expensive = new Expensive();

        return _expensive;
    }
}

public class C
{
    private readonly object _locker = new object();
    private Expensive _expensive;

    public Expensive GetExpensiveInstance()
    {
        lock (_locker)
        {
            if (_expensive == null) _expensive = new Expensive();
            return _expensive;
        }
    }
}

public class D
{
    private Lazy<Expensive> _expensive = new Lazy<Expensive>(() => new Expensive(), true);

    public Expensive GetExpensiveInstance() => _expensive.Value;
}


public class E
{
    private readonly object _locker = new object();
    private volatile Expensive _expensive;

    public Expensive GetExpensiveInstance()
    {
        // 额外的易失读（volatile read）
        if (_expensive == null)
        {
            lock (_locker)
            {
                if (_expensive == null) _expensive = new Expensive();
            }
        }
        
        return _expensive;
    }
}