using FluentMigrator.Runner.Generators.SQLite;
using FluentMigrator.Runner.Processors.SQLite;
using System.Data;

namespace FluentMigrator.Runner.ConnectionExecutor.Processors.SQLite
{
    public class SQLiteProcessorFactory : MigrationProcessorFactory
    {
        public override IMigrationProcessor Create(IDbConnection connection, IAnnouncer announcer, IMigrationProcessorOptions options)
        {
            var factory = new SQLiteDbFactory();
            return new SQLiteProcessor(connection, new SQLiteGenerator(), announcer, options, factory);
        }
    }
}
