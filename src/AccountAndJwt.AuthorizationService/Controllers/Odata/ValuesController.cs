using AccountAndJwt.Database;
using AccountAndJwt.Database.Models.Storage;
using Microsoft.AspNet.OData;
using System;
using System.Linq;

namespace AccountAndJwt.AuthorizationService.Controllers.Odata
{
	public class ValuesController : ODataController
	{
		private readonly DataContext _context;


		public ValuesController(DataContext context)
		{
			_context = context;
		}


		// ACTIONS ////////////////////////////////////////////////////////////////////////////////
		[EnableQuery]
		public IQueryable<ValueDb> GetValues()
		{
			return _context.Values;
		}

		[EnableQuery]
		public ValueDb GetValue([FromODataUri] Int32 key)
		{
			return _context.Values.FirstOrDefault(p => p.Id == key);
		}
	}
}