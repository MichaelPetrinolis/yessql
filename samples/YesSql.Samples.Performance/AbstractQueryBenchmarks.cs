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
    [CoreJob]
    public class AbstractQueryBenchmarks : YesSqlBenchmarks
    {
        public AbstractQueryBenchmarks(string connectionString) : base(connectionString)
        {
            Init();
            for (int i = 0; i < 10; i++)
            {
                CreateUsers(Names.Length);
            }
        }

        [Benchmark]
        public async Task<IEnumerable<UserByName>> QueryIndexByFullName1()
        {
            var rnd = new Random();
            var names = Enumerable.Range(1, 1).Select(x => Names[rnd.Next(Names.Length - 1)]).ToArray();

            using (var session = _store.CreateSession())
            {
                return await session.QueryIndex<UserByName>(x => x.Name.IsIn(names)).ListAsync();
            }
        }

        [Benchmark]
        public async Task<IEnumerable<UserByName>> QueryIndexByFullName10()
        {
            var rnd = new Random();
            var names = Enumerable.Range(1, 10).Select(x => Names[rnd.Next(Names.Length - 1)]).ToArray();

            using (var session = _store.CreateSession())
            {
                return await session.QueryIndex<UserByName>(x => x.Name.IsIn(names)).ListAsync();
            }
        }

        [Benchmark]
        public async Task<IEnumerable<UserByName>> QueryIndexByFullName100()
        {
            var rnd = new Random();
            var names = Enumerable.Range(1, 100).Select(x => Names[rnd.Next(Names.Length - 1)]).ToArray();

            using (var session = _store.CreateSession())
            {
                return await session.QueryIndex<UserByName>(x => x.Name.IsIn(names)).ListAsync();
            }
        }

        [Benchmark]
        public async Task<IEnumerable<User>> QueryByFullName1()
        {
            var rnd = new Random();
            var names = Enumerable.Range(1, 1).Select(x => Names[rnd.Next(Names.Length - 1)]).ToArray();

            using (var session = _store.CreateSession())
            {
                return await session.Query<User, UserByName>(x => x.Name.IsIn(names)).ListAsync();
            }
        }

        [Benchmark]
        public async Task<IEnumerable<User>> QueryByFullName10()
        {
            var rnd = new Random();
            var names = Enumerable.Range(1, 10).Select(x => Names[rnd.Next(Names.Length - 1)]).ToArray();

            using (var session = _store.CreateSession())
            {
                return await session.Query<User, UserByName>(x => x.Name.IsIn(names)).ListAsync();
            }
        }

        [Benchmark]
        public async Task<IEnumerable<User>> QueryByFullName100()
        {
            var rnd = new Random();
            var names = Enumerable.Range(1, 100).Select(x => Names[rnd.Next(Names.Length - 1)]).ToArray();

            using (var session = _store.CreateSession())
            {
                return await session.Query<User, UserByName>(x => x.Name.IsIn(names)).ListAsync();
            }
        }

        [Benchmark]
        public ISession CreateSession()
        {
            using (var session = _store.CreateSession())
            {
                return session;
            }
        }

        [Benchmark]
        public async Task<IEnumerable<UserByName>> QuerySql()
        {
            var rnd = new Random();
            var names = Enumerable.Range(1, 1).Select(x => Names[rnd.Next(Names.Length - 1)]).ToArray();

            using (var session = _store.CreateSession())
            {
                return await session.QueryIndex<UserByName>().Where("Name = '" + names[0] + "'").ListAsync();
            }
        }

        [Benchmark]
        public async Task<IEnumerable<UserByName>> QueryParameterizedSql()
        {
            var rnd = new Random();
            var names = Enumerable.Range(1, 1).Select(x => Names[rnd.Next(Names.Length - 1)]).ToArray();

            using (var session = _store.CreateSession())
            {
                return await session.QueryIndex<UserByName>().Where("Name = @Name").WithParameter("Name", names[0]).ListAsync();
            }
        }

        [Benchmark]
        public async Task<IEnumerable<UserByName>> QueryLinq()
        {
            var rnd = new Random();
            var names = Enumerable.Range(1, 1).Select(x => Names[rnd.Next(Names.Length - 1)]).ToArray();

            using (var session = _store.CreateSession())
            {
                return await session.QueryIndex<UserByName>(x => x.Name == names[0]).ListAsync();
            }
        }

    }
}
