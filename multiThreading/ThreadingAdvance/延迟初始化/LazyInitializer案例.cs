namespace ThreadingAdvance.延迟初始化;

public class F
{
    private Expensive _expensive;

    public Expensive GetExpensiveInstance()
    {
        LazyInitializer.EnsureInitialized(ref _expensive,
            () => new Expensive());
        return _expensive;
    }
}

public class G
{
    private volatile Expensive _expensive;
    public Expensive Expensive
    {
        get
        {
            if (_expensive == null)
            {
                var instance = new Expensive();
                Interlocked.CompareExchange (ref _expensive, instance, null);
            }
            return _expensive;
        }
    }
}