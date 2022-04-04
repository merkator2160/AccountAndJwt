using System.Runtime.Serialization;

namespace AccountAndJwt.Common.Exceptions
{
    public class EnvironmentVariableNotFoundException : ApplicationException
    {
        public EnvironmentVariableNotFoundException()
        {

        }
        public EnvironmentVariableNotFoundException(String message) : base(message)
        {

        }
        public EnvironmentVariableNotFoundException(String message, Exception ex) : base(message)
        {

        }
        protected EnvironmentVariableNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}