using AccountAndJwt.Database;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace AccountAndJwt.AuthorizationService.Controllers.Odata
{
	public class DataController : ODataController
	{
		private readonly DataContext _context;


		public DataController(DataContext context)
		{
			_context = context;
		}


		// ACTIONS ////////////////////////////////////////////////////////////////////////////////
		[EnableQuery]
		public IActionResult Get()
		{
			return Ok(_context.Values);
		}

		[EnableQuery]
		[ODataRoute("Data({id})")]
		public IActionResult Get(Int32 id)
		{
			return Ok(_context.Values.FirstOrDefault(p => p.Id == id));
		}
	}
}