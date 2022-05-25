using Microsoft.AspNetCore.JsonPatch;

namespace AccountAndJwt.Common.Http.Interfaces
{
    public interface ITypedHttpClient
    {
        Task<T> GetObjectAsync<T>(String uri);
        Task<HttpResponseMessage> PostObjectAsync<T>(String uri, T obj);
        Task<HttpResponseMessage> PutObjectAsync<T>(String uri, T obj);
        Task<HttpResponseMessage> PatchObjectAsync<T>(String uri, JsonPatchDocument<T> obj) where T : class;
        Task<HttpResponseMessage> GetAsync(String requestUri);
        Task<HttpResponseMessage> PostAsync(String requestUri, HttpContent content);
        Task<HttpResponseMessage> PutAsync(String requestUri, HttpContent content);
        Task<HttpResponseMessage> PatchAsync(String requestUri, HttpContent content);
        Task<HttpResponseMessage> DeleteAsync(String requestUri);
    }
}