using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace AccountAndJwt.Database.Interceptors
{
	public class HintCommandInterceptor : DbCommandInterceptor
	{
		public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
		{
			// Manipulate the command text, etc. here...
			//command.CommandText += " OPTION (OPTIMIZE FOR UNKNOWN)";
			//return result;

			var commandText = command.CommandText;

			return base.ReaderExecuting(command, eventData, result);
		}
	}
}