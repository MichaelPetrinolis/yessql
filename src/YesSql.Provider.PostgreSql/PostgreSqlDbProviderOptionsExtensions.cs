using Npgsql;
using System;
using System.Data;

namespace YesSql.Provider.PostgreSql
{
    public static class PostgreSqlDbProviderOptionsExtensions
    {
        public static IConfiguration RegisterPostgreSql(this IConfiguration configuration, bool useJson = false)
        {
            SqlDialectFactory.SqlDialects["npgsqlconnection"] = new PostgreSqlDialect(useJson);
            CommandInterpreterFactory.CommandInterpreters["npgsqlconnection"] = d => new PostgreSqlCommandInterpreter(d);

            return configuration;
        }

        public static IConfiguration UsePostgreSql(
            this IConfiguration configuration,
            string connectionString,
            bool useJson = false)
        {
            return UsePostgreSql(configuration, connectionString, IsolationLevel.ReadUncommitted, useJson);
        }

        public static IConfiguration UsePostgreSql(
            this IConfiguration configuration,
            string connectionString,
            IsolationLevel isolationLevel,
            bool useJson = false)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException(nameof(connectionString));
            }

            RegisterPostgreSql(configuration, useJson);
            configuration.ConnectionFactory = new DbConnectionFactory<NpgsqlConnection>(connectionString);
            configuration.IsolationLevel = isolationLevel;

            return configuration;
        }
    }
}
