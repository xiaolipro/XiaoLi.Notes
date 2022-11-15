namespace ThreadingAdvance.拓展;

public class 可递归
{
    [Fact]
    void NoRecursion()
    {
        Assert.Throws<LockRecursionException>(() =>
        {
            var rw = new ReaderWriterLockSlim();
            rw.EnterReadLock();
            rw.EnterReadLock();
            rw.ExitReadLock();
            rw.ExitReadLock();
        });
    }

    [Fact]
    void SupportsRecursion()
    {
        var rw = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        rw.EnterReadLock();
        rw.EnterReadLock();
        rw.ExitReadLock();
        rw.ExitReadLock();
    }

    [Fact]
    void BadRecursion()
    {
        void F()
        {
            var rw = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
            rw.EnterReadLock();
            rw.EnterWriteLock();
            rw.EnterWriteLock();
            rw.ExitReadLock();
        }

        Assert.Throws<LockRecursionException>(F);
    }

    [Fact]
    void SpecialRecursion()
    {
        var rw = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        rw.EnterUpgradeableReadLock();
        rw.EnterWriteLock();
        rw.ExitWriteLock();
        rw.ExitUpgradeableReadLock();
    }
}