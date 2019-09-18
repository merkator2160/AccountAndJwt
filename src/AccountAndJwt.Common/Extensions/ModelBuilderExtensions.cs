using AccountAndJwt.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AccountAndJwt.Common.Extensions
{
	public static class ModelBuilderExtensions
	{
		public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder, IEntityMap<TEntity> entityConfiguration) where TEntity : class
		{
			modelBuilder.Entity<TEntity>(entityConfiguration.Configure);
		}
		public static void NamesToSnakeCase(this ModelBuilder modelBuilder)
		{
			var entities = modelBuilder.Model.GetEntityTypes().ToArray();
			foreach(var entity in entities)
			{
				entity.Relational().TableName = entity.Relational().TableName.ToSnakeCase();

				foreach(var property in entity.GetProperties())
				{
					property.Relational().ColumnName = property.Name.ToSnakeCase();
				}

				foreach(var key in entity.GetKeys())
				{
					key.Relational().Name = key.Relational().Name.ToSnakeCase();
				}

				foreach(var key in entity.GetForeignKeys())
				{
					key.Relational().Name = key.Relational().Name.ToSnakeCase();
				}

				foreach(var index in entity.GetIndexes())
				{
					index.Relational().Name = index.Relational().Name.ToSnakeCase();
				}
			}
		}
		public static String ToSnakeCase(this String input)
		{
			if(String.IsNullOrEmpty(input))
				return input;

			var startUnderscores = Regex.Match(input, @"^_+");
			return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
		}
		public static void CollectMappings(this ModelBuilder modelBuilder, Assembly assemblyToScan)
		{
			var typesToRegister = assemblyToScan.GetTypes()
				.Where(type => !string.IsNullOrEmpty(type.Namespace))
				.Where(type =>
				{
					var info = type.GetTypeInfo();
					return info.ImplementedInterfaces.Any(e => e.Name == typeof(IEntityMap<>).Name);
				}).ToArray();

			foreach(var x in typesToRegister)
			{
				dynamic configurationInstance = Activator.CreateInstance(x);
				AddConfiguration(modelBuilder, configurationInstance);
			}
		}
	}
}