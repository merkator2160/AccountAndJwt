using AccountAndJwt.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AccountAndJwt.Common.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder, IEntityMap<TEntity> entityConfiguration) where TEntity : class
        {
            modelBuilder.Entity<TEntity>(entityConfiguration.Configure);
        }
        public static void CollectMappings(this ModelBuilder modelBuilder, Assembly assemblyToScan)
        {
            var typesToRegister = assemblyToScan.GetTypes()
                .Where(type => !String.IsNullOrEmpty(type.Namespace))
                .Where(type =>
                {
                    var info = type.GetTypeInfo();
                    return info.ImplementedInterfaces.Any(e => e.Name == typeof(IEntityMap<>).Name);
                }).ToArray();

            foreach (var x in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(x);
                AddConfiguration(modelBuilder, configurationInstance);
            }
        }
    }
}