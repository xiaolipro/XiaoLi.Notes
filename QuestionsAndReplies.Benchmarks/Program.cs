// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using QuestionsAndReplies.Benchmarks.Span与Range;

BenchmarkRunner.Run<Span与RangeBenchmarks>();

Console.ReadKey();