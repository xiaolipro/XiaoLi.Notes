using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ParallelPrograming.Parallel类;

public class 拼写检查
{
    private readonly ITestOutputHelper _testOutputHelper;

    public 拼写检查(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void Show()
    {
        if (!File.Exists("WordLookup.txt")) // 包含约 150,000 个单词
        {
            var res = new HttpClient().GetByteArrayAsync(new Uri("http://www.albahari.com/ispell/allwords.txt"))
                .GetAwaiter().GetResult();
            File.WriteAllBytes("WordLookup.txt", res);
        }

        var wordLookup = new HashSet<string>(
            File.ReadAllLines("WordLookup.txt"),
            StringComparer.InvariantCultureIgnoreCase);

        string[] wordList = wordLookup.ToArray();

        var localRandom = new ThreadLocal<Random>(() => new Random());

        string[] wordsToTest = Enumerable.Range(0, 100_0000).AsParallel()
            .Select(i => wordList[localRandom.Value.Next(0, wordList.Length)])
            .ToArray();

        wordsToTest[12345] = "woozsh"; // 引入两个拼写错误
        wordsToTest[23456] = "wubsie";

        var errors = new ConcurrentBag<(long Index, string Word)>();

        Parallel.ForEach(wordsToTest, (word, state, i) =>
        {
            if (!wordLookup.Contains(word)) errors.Add((i, word));
        });

        foreach (var item in errors)
        {
            _testOutputHelper.WriteLine($"单词：{item.Word} 拼写错误，索引：{item.Index}");
        }
    }
}