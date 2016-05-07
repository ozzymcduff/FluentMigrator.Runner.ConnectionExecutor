using System.Data;

namespace FluentMigrator.Runner.ConnectionExecutor.Processors
{
    public interface IMigrationProcessorFactory
    {
        IMigrationProcessor Create(IDbConnection connection, IAnnouncer announcer, IMigrationProcessorOptions options);
        string Name { get; }
    }
}
