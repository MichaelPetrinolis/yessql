using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YesSql.Provider.SqlServer;
using YesSql.Services;
using YesSql.Sql;

namespace YesSql.Samples.Performance
{
    [SimpleJob(RunStrategy.Monitoring, launchCount: 1, warmupCount: 3, targetCount: 3)]
    public abstract class InsertBenchmarksAbstract : YesSqlBenchmarks
    {
        public InsertBenchmarksAbstract(string connectionString) : base(connectionString) { }

        [IterationSetup]
        public void InsertIterationSetup()
        {
            Init();
        }

        [Benchmark]
        public async Task Insert128Users()
        {
            await CreateUsersAsync(-128);
        }

        [Benchmark]
        public async Task Insert256Users()
        {
            await CreateUsersAsync(-256);
        }

        [Benchmark]
        public async Task Insert512Users()
        {
            await CreateUsersAsync(-512);
        }

        [Benchmark]
        public async Task Insert1024Users()
        {
            await CreateUsersAsync(-1024);
        }

        [Benchmark]
        public async Task Insert2048Users()
        {
            await CreateUsersAsync(-2048);
        }


        [Benchmark]
        public async Task InsertMulti128Users()
        {
            await CreateUsersAsync(128);
        }

        [Benchmark]
        public async Task InsertMulti256Users()
        {
            await CreateUsersAsync(256);
        }

        [Benchmark]
        public async Task InsertMulti512Users()
        {
            await CreateUsersAsync(512);
        }

        [Benchmark]
        public async Task InsertMulti1024Users()
        {
            await CreateUsersAsync(1024);
        }

        [Benchmark]
        public async Task InsertMulti2048Users()
        {
            await CreateUsersAsync(2048);
        }

    }
}
