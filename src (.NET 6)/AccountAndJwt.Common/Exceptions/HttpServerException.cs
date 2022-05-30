using System.Net;
using System.Text;

namespace AccountAndJwt.Common.Exceptions
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



        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public override String ToString()
        {
            var sb = new StringBuilder();
            ParseException(sb, this);

            return sb.ToString();
        }
        private void ParseException(StringBuilder sb, Exception ex)
        {
            if (ex is HttpServerException exception)
                sb.Append($"[{exception.Verb}] {exception.Uri}\n");

            sb.Append($"Message: {ex.Message}");

            if (!String.IsNullOrEmpty(ex.StackTrace))
                sb.Append($"\nStackTrace:\n{ex.StackTrace}");

            if (ex.InnerException == null)
                return;

            sb.Append("\n\nInnerException:\n");
            ParseException(sb, ex.InnerException);
        }
    }
}