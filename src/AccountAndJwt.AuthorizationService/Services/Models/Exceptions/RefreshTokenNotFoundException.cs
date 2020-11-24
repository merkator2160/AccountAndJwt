using System;
using System.Runtime.Serialization;

namespace AccountAndJwt.AuthorizationService.Services.Models.Exceptions
{
	internal class RefreshTokenNotFoundException : ApplicationException
	{
		public RefreshTokenNotFoundException()
		{

		}
		public RefreshTokenNotFoundException(String message) : base(message)
		{

		}
		public RefreshTokenNotFoundException(String message, Exception ex) : base(message)
		{

		}
		protected RefreshTokenNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{

		}
	}
}