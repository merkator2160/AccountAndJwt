using AccountAndJwt.Api.Database.Extensions;
using AccountAndJwt.Api.Database.Interfaces;
using AccountAndJwt.Api.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace AccountAndJwt.Api.Database
{
	internal class DataContext : DbContext
	{
		public DataContext(DbContextOptions options) : base(options)
		{
			Initialize();
		}


		// ENTITIES ///////////////////////////////////////////////////////////////////////////////
		public DbSet<ValueDb> Values { get; set; }
		public DbSet<UserDb> Users { get; set; }
		public DbSet<RoleDb> Roles { get; set; }
		public DbSet<UserRoleDb> UserRoles { get; set; }


		// FUNCTIONS //////////////////////////////////////////////////////////////////////////////
		private void Initialize()
		{
			ChangeTracker.AutoDetectChangesEnabled = false;     // increasing working speed 4x times
			ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
			Database.AutoTransactionsEnabled = true;
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Dynamically load all configurations

			var typesToRegister = typeof(DataContext).GetTypeInfo().Assembly.GetTypes()
				.Where(type => !String.IsNullOrEmpty(type.Namespace))
				.Where(type =>
				{
					var info = type.GetTypeInfo();
					return info.ImplementedInterfaces.Any(e => e.Name == typeof(IEntityMap<>).Name);
				});

			foreach(var x in typesToRegister)
			{
				dynamic configurationInstance = Activator.CreateInstance(x);
				ModelBuilderExtensions.AddConfiguration(modelBuilder, configurationInstance);
			}

			// ...or do it manually below. For example,
			// modelBuilder.AddConfiguration(new ParkingTerrioryMap());

			base.OnModelCreating(modelBuilder);
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if(!optionsBuilder.IsConfigured) // This prevents multiple configurations
			{
				base.OnConfiguring(optionsBuilder);
			}
		}
	}
}