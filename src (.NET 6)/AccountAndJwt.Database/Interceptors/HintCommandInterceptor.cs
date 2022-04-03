using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Data.Common;

namespace AccountAndJwt.Database.Interceptors
{
	public class HintCommandInterceptor : DbCommandInterceptor
	{
		public override DbCommand CommandCreated(CommandEndEventData eventData, DbCommand result)
		{
			var commandText = result.CommandText;

			return base.CommandCreated(eventData, result);
		}
		public override void CommandFailed(DbCommand command, CommandErrorEventData eventData)
		{
			base.CommandFailed(command, eventData);
		}
		public override InterceptionResult DataReaderDisposing(DbCommand command, DataReaderDisposingEventData eventData, InterceptionResult result)
		{
			return base.DataReaderDisposing(command, eventData, result);
		}
		public override Int32 NonQueryExecuted(DbCommand command, CommandExecutedEventData eventData, Int32 result)
		{
			return base.NonQueryExecuted(command, eventData, result);
		}
		public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
		{
			return base.ReaderExecuted(command, eventData, result);
		}
		public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
		{
			// Manipulate the command text, etc. here...
			//command.CommandText += " OPTION (OPTIMIZE FOR UNKNOWN)";
			//return result;

			var commandText = command.CommandText;

			return base.ReaderExecuting(command, eventData, result);
		}
		public override Object ScalarExecuted(DbCommand command, CommandExecutedEventData eventData, Object result)
		{
			return base.ScalarExecuted(command, eventData, result);
		}
		public override InterceptionResult<Object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<Object> result)
		{
			return base.ScalarExecuting(command, eventData, result);
		}
	}
}