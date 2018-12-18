using System;
using System.Runtime.Serialization;

namespace AccountAndJwt.Api.Core.Exceptions
{
	public class PosixDateTimeException : ApplicationException
	{
		public PosixDateTimeException()
		{

		}
		public PosixDateTimeException(String message) : base(message)
		{

		}
		public PosixDateTimeException(String message, Exception ex) : base(message)
		{

		}
		protected PosixDateTimeException(SerializationInfo info, StreamingContext contex) : base(info, contex)
		{

		}
	}
}