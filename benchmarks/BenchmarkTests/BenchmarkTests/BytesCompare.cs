using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;

namespace BenchmarkTests;

public static class BytesCompare
{
    [DllImport("msvcrt.dll")]	// 需要使用的dll名称
    private static extern unsafe int memcmp(byte* b1, byte* b2, int count);
    
    // 由于指针使用是内存不安全的操作，所以需要使用unsafe关键字
    // 项目文件中也要加入<AllowUnsafeBlocks>true</AllowUnsafeBlocks>来允许unsafe代码
    public static unsafe bool MemcmpCompare(byte[]? x,byte[]? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        if (x.Length != y.Length) return false;
        
        // 在.NET程序的运行中，垃圾回收器可能会整理和压缩内存，这样会导致数组地址变动
        // 所以，我们需要使用fixed关键字，将x和y数组'固定'在内存中，让GC不移动它
        // 更多详情请看 https://docs.microsoft.com/zh-cn/dotnet/csharp/language-reference/keywords/fixed-statement
        fixed (byte* xPtr = x, yPtr = y)	
        {
            return memcmp(xPtr, yPtr, x.Length) == 0;
        }
    }
    
    
    public static unsafe bool UlongCompare(byte[]? x, byte[]? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        if (x.Length != y.Length) return false;
    
        fixed (byte* xPtr = x, yPtr = y)
        {
            return UlongCompareInternal(xPtr, yPtr, x.Length);
        }
    }

    private static unsafe bool UlongCompareInternal(byte* xPtr, byte* yPtr, int length)
    {
        // 指针+偏移量计算出数组最后一个元素地址
        byte* lastAddr = xPtr + length;
        byte* lastAddrMinus32 = lastAddr - 32;
        while (xPtr < lastAddrMinus32) // 我们一次循环比较32字节，也就是256位
        {
            // 一次判断比较前64位
            if (*(ulong*) xPtr != *(ulong*) yPtr) return false;
            // 第二次从64为开始，比较接下来的64位，需要指针偏移64位，一个byte指针是8为，所以需要偏移8个位置才能到下一轮起始位置
            // 所以代码就是xPtr+8
            if (*(ulong*) (xPtr + 8) != *(ulong*) (yPtr + 8)) return false;
            // 同上面一样，第三次从第128位开始比较64位
            if (*(ulong*) (xPtr + 16) != *(ulong*) (yPtr + 16)) return false;
            // 第四次从第192位开始比较64位
            if (*(ulong*) (xPtr + 24) != *(ulong*) (yPtr + 24)) return false;
            // 一轮总共比较了256位，让指针偏移256位
            xPtr += 32;
            yPtr += 32;
        }
        // 因为上面是一次性比较32字节(256位)，可能数组不能为32整除，最后只留下比如30字节，20字节
        // 最后的几个字节，我们用循环来逐字节比较
        while (xPtr < lastAddr)
        {
            if (*xPtr != *yPtr) return false;
            xPtr++;
            yPtr++;
        }
        return true;
    }
    
    
    public static unsafe bool Sse2Compare(byte[]? x, byte[]? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        if (x.Length != y.Length) return false;
        
        fixed (byte* xPtr = x, yPtr = y)
        {
            return Sse2CompareInternal(xPtr, yPtr, x.Length);
        }
    }
    
    private static unsafe bool Sse2CompareInternal(byte* xPtr, byte* yPtr, int length)
    {
        // 这里的算法与64位大体一样，只是位数变成了128位
        byte* lastAddr = xPtr + length;
        byte* lastAddrMinus64 = lastAddr - 64;
        const int mask = 0xFFFF;
        while (xPtr < lastAddrMinus64)
        {
            // 使用Sse2.LoadVector128()各加载x和y的128位数据
            // 再使用Sse2.CompareEqual()比较是否相等，它的返回值是一个128位向量，如果相等，该位置返回0xffff，否则返回0x0
            // CompareEqual的结果是128位的，我们可以通过Sse2.MoveMask()来重新排列成32位，最终看是否等于0xffff就好
            if (Sse2.MoveMask(Sse2.CompareEqual(Sse2.LoadVector128(xPtr), Sse2.LoadVector128(yPtr))) != mask)
            {
                return false;
            }

            if (Sse2.MoveMask(Sse2.CompareEqual(Sse2.LoadVector128(xPtr + 16), Sse2.LoadVector128(yPtr + 16))) != mask)
            {
                return false;
            }

            if (Sse2.MoveMask(Sse2.CompareEqual(Sse2.LoadVector128(xPtr + 32), Sse2.LoadVector128(yPtr + 32))) != mask)
            {
                return false;
            }

            if (Sse2.MoveMask(Sse2.CompareEqual(Sse2.LoadVector128(xPtr + 48), Sse2.LoadVector128(yPtr + 48))) != mask)
            {
                return false;
            }

            xPtr += 64;
            yPtr += 64;
        }

        while (xPtr < lastAddr)
        {
            if (*xPtr != *yPtr) return false;
            xPtr++;
            yPtr++;
        }

        return true;
    }
}