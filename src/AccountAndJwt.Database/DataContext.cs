﻿using AccountAndJwt.Database.Extensions;
using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Database.Models.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace AccountAndJwt.Database
{
	public class DataContext : DbContext
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
			ChangeTracker.AutoDetectChangesEnabled = false;                             // manual changes tracking, increasing working speed 4x times
			ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;     // Equivalent of .AsNoTracking() for each select query
			Database.AutoTransactionsEnabled = true;                                    // Required for "Unit of work pattern"
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