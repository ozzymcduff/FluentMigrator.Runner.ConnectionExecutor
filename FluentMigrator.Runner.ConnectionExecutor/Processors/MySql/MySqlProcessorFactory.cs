using FluentMigrator.Runner.Generators.MySql;
using FluentMigrator.Runner.Processors.MySql;
using System.Data;

namespace FluentMigrator.Runner.ConnectionExecutor.Processors.MySql
{
    public class MySqlProcessorFactory : MigrationProcessorFactory
    {
        public override IMigrationProcessor Create(IDbConnection connection, IAnnouncer announcer, IMigrationProcessorOptions options)
        {
            var factory = new MySqlDbFactory();
            return new MySqlProcessor(connection, new MySqlGenerator(), announcer, options, factory);
        }
    }
}
