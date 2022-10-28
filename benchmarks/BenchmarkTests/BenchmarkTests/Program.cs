// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using BenchmarkTests;

BenchmarkRunner.Run<CompareBenchmark>();

Console.ReadKey();