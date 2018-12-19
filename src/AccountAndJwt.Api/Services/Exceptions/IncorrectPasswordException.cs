using System;
using System.Runtime.Serialization;

namespace AccountAndJwt.Api.Services.Exceptions
{
	public class IncorrectPasswordException : ApplicationException
	{
		public IncorrectPasswordException()
		{

		}
		public IncorrectPasswordException(String message) : base(message)
		{

		}
		public IncorrectPasswordException(String message, Exception ex) : base(message)
		{

		}
		protected IncorrectPasswordException(SerializationInfo info, StreamingContext contex) : base(info, contex)
		{

		}
	}
}