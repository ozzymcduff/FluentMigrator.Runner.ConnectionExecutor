using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMigrator.Runner.ConnectionExecutor.Processors
{
    public abstract class MigrationProcessorFactory : IMigrationProcessorFactory
    {
        public abstract IMigrationProcessor Create(IDbConnection connection, IAnnouncer announcer, IMigrationProcessorOptions options);

        public virtual bool IsForProvider(string provider)
        {
            return provider.ToLower().Contains(Name.ToLower());
        }

        public virtual string Name
        {
            get { return GetType().Name.Replace("ProcessorFactory", string.Empty); }
        }

    }
}