using FluentMigrator.Runner.Generators.Postgres;
using FluentMigrator.Runner.Processors.Postgres;
using System.Data;

namespace FluentMigrator.Runner.ConnectionExecutor.Processors.Postgres
{
    public class PostgresProcessorFactory : MigrationProcessorFactory
    {
        public override IMigrationProcessor Create(IDbConnection connection, IAnnouncer announcer, IMigrationProcessorOptions options)
        {
            var factory = new PostgresDbFactory();
            return new PostgresProcessor(connection, new PostgresGenerator(), announcer, options, factory);
        }
    }
}
