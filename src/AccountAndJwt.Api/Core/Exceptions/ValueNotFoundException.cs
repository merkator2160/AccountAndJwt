using System;
using System.Runtime.Serialization;

namespace AccountAndJwt.Api.Core.Exceptions
{
	public class ValueNotFoundException : ApplicationException
	{
		public ValueNotFoundException()
		{

		}
		public ValueNotFoundException(String message) : base(message)
		{

		}
		public ValueNotFoundException(String message, Exception ex) : base(message)
		{

		}
		protected ValueNotFoundException(SerializationInfo info, StreamingContext contex) : base(info, contex)
		{

		}
	}
}