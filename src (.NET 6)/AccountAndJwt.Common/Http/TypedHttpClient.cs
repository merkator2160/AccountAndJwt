using AccountAndJwt.Common.Exceptions;
using AccountAndJwt.Common.Http.Interfaces;
using AccountAndJwt.Contracts.Const;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace AccountAndJwt.Common.Http
{
    public class TypedHttpClient : HttpClient, ITypedHttpClient
    {
        private static readonly HttpMethod _patch = new HttpMethod("PATCH");


        public TypedHttpClient()
        {
            SerializerSettings = new JsonSerializerSettings();
        }
        public TypedHttpClient(JsonSerializerSettings serializerSettings)
        {
            SerializerSettings = serializerSettings;
        }
        public TypedHttpClient(JsonSerializerSettings serializerSettings, HttpMessageHandler handler) : base(handler)
        {
            SerializerSettings = serializerSettings;
        }
        public TypedHttpClient(JsonSerializerSettings serializerSettings, HttpMessageHandler handler, Boolean disposeHandler) : base(handler, disposeHandler)
        {
            SerializerSettings = serializerSettings;
        }


        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        public JsonSerializerSettings SerializerSettings { get; set; }


        // ITypedHttpClient ///////////////////////////////////////////////////////////////////////
        public async Task<T> GetObjectAsync<T>(String uri)
        {
            using (var response = await GetAsync(uri))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpServerException(HttpMethod.Get, response.StatusCode, response.Headers.Location!.AbsolutePath, await response.Content.ReadAsStringAsync());

                return await DeserializeAsync<T>(response);
            }
        }
        public async Task<HttpResponseMessage> PostObjectAsync<T>(String uri, T obj)
        {
            if (obj is HttpContent content)
                return await PostAsync(uri, content);

            return await PostAsync(uri, Serialize(obj));
        }
        public async Task<HttpResponseMessage> PutObjectAsync<T>(String uri, T obj)
        {
            if (obj is HttpContent content)
                return await PutAsync(uri, content);

            return await PutAsync(uri, Serialize(obj));
        }
        public async Task<HttpResponseMessage> PatchObjectAsync<T>(String uri, JsonPatchDocument<T> obj) where T : class
        {
            return await PatchAsync(uri, Serialize(obj));
        }

        Task<HttpResponseMessage> ITypedHttpClient.PatchAsync(String requestUri, HttpContent content)
        {
            return SendAsync(new HttpRequestMessage(_patch, requestUri)
            {
                Content = content
            });
        }
        Task<HttpResponseMessage> ITypedHttpClient.GetAsync(String requestUri)
        {
            return GetAsync(requestUri);
        }
        Task<HttpResponseMessage> ITypedHttpClient.PostAsync(String requestUri, HttpContent content)
        {
            return PostAsync(requestUri, content);
        }
        Task<HttpResponseMessage> ITypedHttpClient.PutAsync(String requestUri, HttpContent content)
        {
            return PutAsync(requestUri, content);
        }
        Task<HttpResponseMessage> ITypedHttpClient.DeleteAsync(String requestUri)
        {
            return DeleteAsync(requestUri);
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        protected HttpContent Serialize<T>(T obj)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(obj, SerializerSettings));
            stringContent.Headers.ContentType = new MediaTypeHeaderValue(HttpMimeType.Application.Json);
            return stringContent;
        }
        protected async Task<T> DeserializeAsync<T>(HttpResponseMessage response)
        {
            var reader = JsonSerializer.Create(SerializerSettings);
            using (var streamReader = new StreamReader(await response.Content.ReadAsStreamAsync()))
            {
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    return reader.Deserialize<T>(jsonTextReader);
                }
            }
        }
    }
}