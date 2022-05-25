using System.Net;

namespace AccountAndJwt.Contracts.Models.Exceptions
{
    public class HttpServerException : ApplicationException
    {
        public HttpServerException(HttpMethod verb, HttpStatusCode statusCode, String uri, String message) : base(message)
        {
            StatusCode = statusCode;
            Uri = uri;
            Verb = verb;
        }


        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        public HttpStatusCode StatusCode { get; }
        public String Uri { get; }
        public HttpMethod Verb { get; }
    }
}