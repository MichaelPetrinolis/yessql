using Npgsql;
using System;
using System.Data;

namespace YesSql.Provider.PostgresJson
{
    public static class PostgresJsonDbProviderOptionsExtensions
    {
        public static IConfiguration RegisterPostgresJson(this IConfiguration configuration)
        {
            SqlDialectFactory.SqlDialects["npgsqlconnection"] = new PostgresJsonDialect();
            CommandInterpreterFactory.CommandInterpreters["npgsqlconnection"] = d => new PostgresJsonCommandInterpreter(d);

            return configuration;
        }

        public static IConfiguration UsePostgresJson(
            this IConfiguration configuration,
            string connectionString)
        {
            return UsePostgresJson(configuration, connectionString, IsolationLevel.ReadUncommitted);
        }

        public static IConfiguration UsePostgresJson(
            this IConfiguration configuration,
            string connectionString,
            IsolationLevel isolationLevel)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException(nameof(connectionString));
            }

            RegisterPostgresJson(configuration);
            configuration.ConnectionFactory = new DbConnectionFactory<NpgsqlConnection>(connectionString);
            configuration.IsolationLevel = isolationLevel;

            return configuration;
        }
    }
}
