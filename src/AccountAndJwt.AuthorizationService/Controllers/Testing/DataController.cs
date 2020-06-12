using AccountAndJwt.Database;
using AccountAndJwt.Database.Models.Storage;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AccountAndJwt.AuthorizationService.Controllers.Testing
{
	public class DataController : ODataController
	{
		private readonly DataContext _context;


		public DataController(DataContext context)
		{
			_context = context;
		}


		// ACTIONS ////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// 
		/// </summary>
		[HttpGet]
		[EnableQuery]
		public ActionResult<IQueryable<ValueDb>> Get()
		{
			return _context.Values;
		}
	}
}