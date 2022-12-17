using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ParallelPrograming.Parallel类;

public class For案例
{
    private readonly ITestOutputHelper _testOutputHelper;

    public For案例(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void PINQ生成密钥对()
    {
        string[] keyPairs =
            ParallelEnumerable.Range(0, 6)
                .Select(i => RSA.Create().ToXmlString(true))
                .ToArray();

        foreach (var item in keyPairs)
        {
            _testOutputHelper.WriteLine(item);
        }
    }

    [Fact]
    void Parallel生成密钥对()
    {
        var keyPairs = new string[6];

        Parallel.For(0, keyPairs.Length,
            i => keyPairs[i] = RSA.Create().ToXmlString(true));

        foreach (var item in keyPairs)
        {
            _testOutputHelper.WriteLine(item);
        }
    }
}