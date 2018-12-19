using System;
using System.Runtime.Serialization;

namespace AccountAndJwt.Api.Services.Exceptions
{
	public class RefreshTokenNotFoundException : ApplicationException
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
		protected RefreshTokenNotFoundException(SerializationInfo info, StreamingContext contex) : base(info, contex)
		{

		}
	}
}