using AccountAndJwt.Database.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccountAndJwt.Database.Extensions
{
    internal static class ModelBuilderExtensions
    {
        public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder, IEntityMap<TEntity> entityConfiguration) where TEntity : class
        {
            modelBuilder.Entity<TEntity>(entityConfiguration.Configure);
        }
    }
}