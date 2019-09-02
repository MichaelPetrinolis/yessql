using BenchmarkDotNet.Running;
using System;
using System.Diagnostics;

namespace YesSql.Samples.Performance
{
    public class Program
    {
        static void Main(string[] args)
        {
            InsertBenchmarksAbstract b;

            b = new SqlServer.InsertBenchmarks();
            MeasureTests("Without Pipelining", b);

            b = new SqlServer.InsertPipelinedBenchmarks();
            MeasureTests("With Pipelining", b);

            Console.ReadLine();
            //BenchmarkRunner.Run<SqlServer.InsertBenchmarks>();
            //BenchmarkRunner.Run<SqlServer.InsertPipelinedBenchmarks>();

            //BenchmarkRunner.Run<SqlServer.QueryBenchmarks>();
        }

        static void MeasureTests(string title,InsertBenchmarksAbstract b)
        {
            Console.WriteLine("***** " + title);           

            b.InsertIterationSetup();
            b.InsertMulti2048Users().GetAwaiter().GetResult();
            b.InsertIterationSetup();
            b.Insert2048Users().GetAwaiter().GetResult();

            var sp = new Stopwatch();

            b.InsertIterationSetup();
            sp.Start();
            b.InsertMulti2048Users().GetAwaiter().GetResult();
            sp.Stop();
            Console.WriteLine($"InsertMulti2048Users Time: {sp.ElapsedMilliseconds} ms");

            b.InsertIterationSetup();
            sp.Restart();
            b.Insert2048Users().GetAwaiter().GetResult();
            sp.Stop();
            Console.WriteLine($"Insert2048Users Time: {sp.ElapsedMilliseconds} ms");

        }
    }
}
