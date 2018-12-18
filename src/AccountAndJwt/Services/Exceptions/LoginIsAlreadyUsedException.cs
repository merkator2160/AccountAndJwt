using System;
using System.Runtime.Serialization;

namespace AccountAndJwt.Api.Services.Exceptions
{
	public class LoginIsAlreadyUsedException : ApplicationException
	{
		public LoginIsAlreadyUsedException()
		{

		}
		public LoginIsAlreadyUsedException(String message) : base(message)
		{

		}
		public LoginIsAlreadyUsedException(String message, Exception ex) : base(message)
		{

		}
		protected LoginIsAlreadyUsedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{

		}
	}
}