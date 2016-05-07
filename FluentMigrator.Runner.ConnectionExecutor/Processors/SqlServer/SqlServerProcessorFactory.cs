using FluentMigrator.Runner.Generators.SqlServer;
using FluentMigrator.Runner.Processors.SqlServer;
using System.Data;

namespace FluentMigrator.Runner.ConnectionExecutor.Processors.SqlServer
{
    public class SqlServerProcessorFactory : MigrationProcessorFactory
        {
            public override IMigrationProcessor Create(IDbConnection connection, IAnnouncer announcer, IMigrationProcessorOptions options)
            {
                var factory = new SqlServerDbFactory();
                return new SqlServerProcessor(connection, new SqlServer2008Generator(), announcer, options, factory);
            }

            public override bool IsForProvider(string provider)
            {
                return provider.ToLower().Contains("sqlclient");
            }
        }
    
}
