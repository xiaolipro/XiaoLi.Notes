using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace ParallelPrograming.PLINQ;

public class 拼写检查
{
    private readonly ITestOutputHelper _testOutputHelper;

    public 拼写检查(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    void 普通检查()
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

        var random = new Random();
        string[] wordList = wordLookup.ToArray();

        string[] wordsToTest = Enumerable.Range(0, 100_0000)
            .Select(i => wordList[random.Next(0, wordList.Length)])
            .ToArray();

        wordsToTest[12345] = "woozsh"; // 引入两个拼写错误
        wordsToTest[23456] = "wubsie";

        var query = wordsToTest
            .Select((word, index) => new IndexedWord { Word = word, Index = index })
            .Where(iword => !wordLookup.Contains(iword.Word))
            .OrderBy(iword => iword.Index);

        foreach (var item in query)
        {
            _testOutputHelper.WriteLine($"单词：{item.Word} 拼写错误，索引：{item.Index}");
        }
    }

    [Fact]
    void 并行检查()
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

        var random = new Random();
        string[] wordList = wordLookup.ToArray();

        string[] wordsToTest = Enumerable.Range(0, 100_0000)
            .Select(i => wordList[random.Next(0, wordList.Length)])
            .ToArray();

        wordsToTest[12345] = "woozsh"; // 引入两个拼写错误
        wordsToTest[23456] = "wubsie";

        var query = wordsToTest
            .AsParallel()
            .Select((word, index) => new IndexedWord { Word = word, Index = index })
            .Where(iword => !wordLookup.Contains(iword.Word))
            .OrderBy(iword => iword.Index);

        //query.Dump();     // 在 LINQPad 中显示输出

        foreach (var item in query)
        {
            _testOutputHelper.WriteLine($"单词：{item.Word} 拼写错误，索引：{item.Index}");
        }
    }


    struct IndexedWord
    {
        public string Word;
        public int Index;
    }
}