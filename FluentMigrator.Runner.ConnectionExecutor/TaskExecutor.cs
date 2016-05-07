using FluentMigrator.Exceptions;
using FluentMigrator.Infrastructure;
using FluentMigrator.Runner.Generators;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Initialization.AssemblyLoader;
using FluentMigrator.Runner.Processors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace FluentMigrator.Runner.ConnectionExecutor
{
    using Processors;
    public class ConnectionTaskExecutor: Initialization.TaskExecutor
    {
        private readonly IDbConnection Connection;

        private IRunnerContext RunnerContext { get; set; }

        private AssemblyLoaderFactory AssemblyLoaderFactory { get; set; }
        private MigrationProcessorFactoryProvider ProcessorFactoryProvider { get; set; }

        public ConnectionTaskExecutor(IRunnerContext runnerContext, IDbConnection connection)
            : this(runnerContext, new AssemblyLoaderFactory(), new MigrationProcessorFactoryProvider(), connection)
        {
        }

        public ConnectionTaskExecutor(IRunnerContext runnerContext, AssemblyLoaderFactory assemblyLoaderFactory, MigrationProcessorFactoryProvider processorFactoryProvider, IDbConnection connection):base(runnerContext,assemblyLoaderFactory,null)
        {
            if (runnerContext == null) throw new ArgumentNullException("runnerContext");
            if (assemblyLoaderFactory == null) throw new ArgumentNullException("assemblyLoaderFactory");

            RunnerContext = runnerContext;
            AssemblyLoaderFactory = assemblyLoaderFactory;
            ProcessorFactoryProvider = processorFactoryProvider;
            Connection = connection;
        }

        protected virtual void Initialize(IDbConnection connection)
        {
            List<Assembly> assemblies = new List<Assembly>();

            foreach (var target in RunnerContext.Targets)
            {
                var assembly = AssemblyLoaderFactory.GetAssemblyLoader(target).Load();

                if (!assemblies.Contains(assembly))
                {
                    assemblies.Add(assembly);
                }
            }

            var assemblyCollection = new AssemblyCollection(assemblies);

            var processor = RunnerContext.NoConnection ? InitializeConnectionlessProcessor() : InitializeProcessor(assemblyCollection, connection);

            Runner = new MigrationRunner(assemblyCollection, RunnerContext, processor);
        }

        protected override void Initialize()
        {
            Initialize(Connection);
        }

        private IMigrationProcessor InitializeConnectionlessProcessor()
        {
            var options = new ProcessorOptions
            {
                PreviewOnly = RunnerContext.PreviewOnly,
                Timeout = RunnerContext.Timeout,
                ProviderSwitches = RunnerContext.ProviderSwitches
            };

            var generator = new MigrationGeneratorFactory().GetGenerator(RunnerContext.Database);

            var processor = new ConnectionlessProcessor(generator, RunnerContext, options);

            return processor;
        }

        private IMigrationProcessor InitializeProcessor(IAssemblyCollection assemblyCollection, IDbConnection connection)
        {

            if (RunnerContext.Timeout == 0)
            {
                RunnerContext.Timeout = 30; // Set default timeout for command
            }

            var processorFactory = ProcessorFactoryProvider.GetFactory(RunnerContext.Database);

            if (processorFactory == null)
                throw new ProcessorFactoryNotFoundException(string.Format("The provider or dbtype parameter is incorrect. Available choices are {0}: ", ProcessorFactoryProvider.ListAvailableProcessorTypes()));

            var processor = processorFactory.Create(connection, RunnerContext.Announcer, new ProcessorOptions
            {
                PreviewOnly = RunnerContext.PreviewOnly,
                Timeout = RunnerContext.Timeout,
                ProviderSwitches = RunnerContext.ProviderSwitches
            });

            return processor;
        }
    }
}
