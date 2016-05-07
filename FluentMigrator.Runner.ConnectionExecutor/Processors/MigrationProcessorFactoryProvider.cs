using FluentMigrator.Runner.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentMigrator.Runner.ConnectionExecutor.Processors
{
    public class MigrationProcessorFactoryProvider
    {
        private static readonly IDictionary<string, IMigrationProcessorFactory> MigrationProcessorFactories;

        static MigrationProcessorFactoryProvider()
        {
            Assembly assembly = typeof(IMigrationProcessorFactory).Assembly;

            List<Type> types = assembly
                .GetExportedTypes()
                .Where(type => type.IsConcrete() && type.Is<IMigrationProcessorFactory>())
                .ToList();

            var availableMigrationProcessorFactories = new SortedDictionary<string, IMigrationProcessorFactory>();
            foreach (Type type in types)
            {
                var factory = (IMigrationProcessorFactory)Activator.CreateInstance(type);
                availableMigrationProcessorFactories.Add(factory.Name, factory);
            }

            MigrationProcessorFactories = availableMigrationProcessorFactories;
        }

        public virtual IMigrationProcessorFactory GetFactory(string name)
        {
            return MigrationProcessorFactories
                .Where(pair => pair.Key.Equals(name, StringComparison.OrdinalIgnoreCase))
                .Select(pair => pair.Value)
                .FirstOrDefault();
        }

        public string ListAvailableProcessorTypes()
        {
            return string.Join(", ", MigrationProcessorFactories.Keys.ToArray());
        }
    }
}
