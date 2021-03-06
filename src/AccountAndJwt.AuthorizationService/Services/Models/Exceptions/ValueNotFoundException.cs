﻿using System;
using System.Runtime.Serialization;

namespace AccountAndJwt.AuthorizationService.Services.Models.Exceptions
{
	internal class ValueNotFoundException : ApplicationException
	{
		public ValueNotFoundException()
		{

		}
		public ValueNotFoundException(String message) : base(message)
		{

		}
		public ValueNotFoundException(String message, Exception ex) : base(message)
		{

		}
		protected ValueNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{

		}
	}
}