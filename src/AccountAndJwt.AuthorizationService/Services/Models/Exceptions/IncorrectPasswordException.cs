using System;
using System.Runtime.Serialization;

namespace AccountAndJwt.AuthorizationService.Services.Models.Exceptions
{
	internal class IncorrectPasswordException : ApplicationException
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
		protected IncorrectPasswordException(SerializationInfo info, StreamingContext context) : base(info, context)
		{

		}
	}
}