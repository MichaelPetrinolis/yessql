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

namespace YesSql.Samples.Performance.SqlServer
{
    [CoreJob]
    public class QueryBenchmarks : AbstractQueryBenchmarks
    {
        public QueryBenchmarks() : base(@"Data Source =localhost; Initial Catalog = yessql; Integrated Security = True;MultipleActiveResultSets=True") { }
    }
}
