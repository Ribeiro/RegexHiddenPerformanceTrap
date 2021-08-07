using BenchmarkDotNet.Running;
using System;

namespace RegexPerformanceTrap
{
    public class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchy>();
        }
    }
}
