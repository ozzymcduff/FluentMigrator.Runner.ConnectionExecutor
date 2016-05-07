using FluentMigrator.Runner.Generators.Firebird;
using FluentMigrator.Runner.Processors.Firebird;
using System;
using System.Data;

namespace FluentMigrator.Runner.ConnectionExecutor.Processors.Firebird
{
    public class FirebirdProcessorFactory : MigrationProcessorFactory
    {
        public FirebirdOptions FBOptions { get; set; }

        public FirebirdProcessorFactory() : this(FirebirdOptions.AutoCommitBehaviour()) { }
        public FirebirdProcessorFactory(FirebirdOptions fbOptions)
            : base()
        {
            if (fbOptions == null)
                throw new ArgumentNullException("fbOptions");
            FBOptions = fbOptions;
        }

        public override IMigrationProcessor Create(IDbConnection connection, IAnnouncer announcer, IMigrationProcessorOptions options)
        {
            var factory = new FirebirdDbFactory();
            return new FirebirdProcessor(connection, new FirebirdGenerator(FBOptions), announcer, options, factory, FBOptions);
        }
    }
}
