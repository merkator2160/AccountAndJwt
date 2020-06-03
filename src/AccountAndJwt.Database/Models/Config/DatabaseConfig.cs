using System;

namespace AccountAndJwt.Database.Models.Config
{
	public class DatabaseConfig
	{
		public Int32 CommandTimeout { get; set; }
		public Int32 MigrationTimeout { get; set; }
	}
}