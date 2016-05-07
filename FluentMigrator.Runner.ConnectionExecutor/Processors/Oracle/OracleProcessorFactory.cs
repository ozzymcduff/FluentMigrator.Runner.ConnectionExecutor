using FluentMigrator.Runner.Generators.Oracle;
using FluentMigrator.Runner.Processors.Oracle;
using System.Data;

namespace FluentMigrator.Runner.ConnectionExecutor.Processors.Oracle
{
    public class OracleProcessorFactory : MigrationProcessorFactory
    {
        public override IMigrationProcessor Create(IDbConnection connection, IAnnouncer announcer, IMigrationProcessorOptions options)
        {
            var factory = new OracleDbFactory();
            return new OracleProcessor(connection, new OracleGenerator(Quoted(options.ProviderSwitches)), announcer, options, factory);
        }

        private bool Quoted(string options)
        {
            return !string.IsNullOrEmpty(options) &&
                options.ToUpper().Contains("QUOTEDIDENTIFIERS=TRUE");
        }
    }
}
