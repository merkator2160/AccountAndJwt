using AccountAndJwt.Contracts.Const;
using AccountAndJwt.Ui.Clients.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AccountAndJwt.Ui.Clients
{
    public class TypedHttpClient : HttpClient, ITypedHttpClient
    {
        // ITypedHttpClient ///////////////////////////////////////////////////////////////////////
        public async Task<HttpResponseMessage> GetAsync(String uri, String accessToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                return await SendAsync(request);
            }
        }
        public async Task<HttpResponseMessage> PostAsync<T>(String uri, T obj, String accessToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, uri))
            {
                request.Content = new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, HttpMimeType.Application.Json);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                return await SendAsync(request);
            }
        }
    }
}