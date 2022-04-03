using System;
using System.Collections.Generic;
using System.Net;

namespace AccountAndJwt.Contracts.Models.Errors
{
	public class ModelStateAm
	{
		public Dictionary<String, String[]> Errors { get; set; }
		public String Type { get; set; }
		public String Title { get; set; }
		public HttpStatusCode Status { get; set; }
		public String TraceId { get; set; }
	}
}