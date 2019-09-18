using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Reflection;

namespace AccountAndJwt.Common.Extensions
{
	public static class IQueryableExtensions
	{
		private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();
		private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");
		private static readonly FieldInfo QueryModelGeneratorField = QueryCompilerTypeInfo.DeclaredFields.First(x => x.Name == "_queryModelGenerator");
		private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");
		private static readonly PropertyInfo DatabaseDependenciesField = typeof(Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");


		// FUNCTIONS //////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// https://stackoverflow.com/questions/37527783/get-sql-code-from-an-ef-core-query
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="query"></param>
		/// <returns></returns>
		public static String ToSql<TEntity>(this IQueryable<TEntity> query)
		{
			var queryCompiler = (QueryCompiler)QueryCompilerField.GetValue(query.Provider);
			var modelGenerator = (QueryModelGenerator)QueryModelGeneratorField.GetValue(queryCompiler);
			var queryModel = modelGenerator.ParseQuery(query.Expression);
			var database = (IDatabase)DataBaseField.GetValue(queryCompiler);
			var databaseDependencies = (DatabaseDependencies)DatabaseDependenciesField.GetValue(database);
			var queryCompilationContext = databaseDependencies.QueryCompilationContextFactory.Create(false);
			var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();

			modelVisitor.CreateQueryExecutor<TEntity>(queryModel);

			return modelVisitor.Queries.First().ToString();
		}
	}
}