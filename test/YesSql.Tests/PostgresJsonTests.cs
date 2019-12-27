using System;
using System.Data.Common;
using System.Threading.Tasks;
using Xunit;
using YesSql.Provider.PostgreSql;
using YesSql.Sql;

namespace YesSql.Tests
{
    public class PostgresJsonTests : PostgreSqlTests
    {

        protected override IConfiguration CreateConfiguration()
        {
            return new Configuration()
                .UsePostgreSql(ConnectionString, true)
                .SetTablePrefix(TablePrefix)
                .UseBlockIdGenerator()
                ;
        }

    }
}
