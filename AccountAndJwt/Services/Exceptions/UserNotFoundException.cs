using System;
using System.Runtime.Serialization;

namespace AccountAndJwt.Services.Exceptions
{
    public class UserNotFoundException : ApplicationException
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
        protected UserNotFoundException(SerializationInfo info, StreamingContext contex) : base(info, contex)
        {

        }
    }
}