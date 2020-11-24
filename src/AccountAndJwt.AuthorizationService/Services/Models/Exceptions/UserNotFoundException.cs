using System;
using System.Runtime.Serialization;

namespace AccountAndJwt.AuthorizationService.Services.Models.Exceptions
{
	internal class UserNotFoundException : ApplicationException
	{
		public UserNotFoundException()
		{

		}
		public UserNotFoundException(String message) : base(message)
		{

		}
		public UserNotFoundException(String message, Exception ex) : base(message)
		{

		}
		protected UserNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{

		}
	}
}