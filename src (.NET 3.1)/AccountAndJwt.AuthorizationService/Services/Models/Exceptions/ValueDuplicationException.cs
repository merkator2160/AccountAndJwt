using System;
using System.Runtime.Serialization;

namespace AccountAndJwt.AuthorizationService.Services.Models.Exceptions
{
	internal class ValueDuplicationException : ApplicationException
	{
		public ValueDuplicationException()
		{

		}
		public ValueDuplicationException(String message) : base(message)
		{

		}
		public ValueDuplicationException(String message, Exception ex) : base(message)
		{

		}
		protected ValueDuplicationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{

		}
	}
}