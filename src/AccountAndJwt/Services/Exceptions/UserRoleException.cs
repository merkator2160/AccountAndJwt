using System;
using System.Runtime.Serialization;

namespace AccountAndJwt.Api.Services.Exceptions
{
	public class UserRoleException : ApplicationException
	{
		public UserRoleException()
		{

		}
		public UserRoleException(String message) : base(message)
		{

		}
		public UserRoleException(String message, Exception ex) : base(message)
		{

		}
		protected UserRoleException(SerializationInfo info, StreamingContext contex) : base(info, contex)
		{

		}
	}
}