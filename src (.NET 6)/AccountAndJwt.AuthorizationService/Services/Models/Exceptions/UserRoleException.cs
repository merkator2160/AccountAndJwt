using System;
using System.Runtime.Serialization;

namespace AccountAndJwt.AuthorizationService.Services.Models.Exceptions
{
	internal class UserRoleException : ApplicationException
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
		protected UserRoleException(SerializationInfo info, StreamingContext context) : base(info, context)
		{

		}
	}
}