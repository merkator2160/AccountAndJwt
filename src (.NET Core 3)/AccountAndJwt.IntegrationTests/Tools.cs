using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AccountAndJwt.IntegrationTests
{
	internal static class Tools
	{
		public static async Task<T> DeserializeAsync<T>(this HttpResponseMessage response)
		{
			return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings());
		}
		public static async Task<T> DeserializeAsync<T>(this HttpResponseMessage response, JsonSerializerSettings settings)
		{
			return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(), settings);
		}

		public static HttpContent ToHttpContent<T>(this T obj)
		{
			return obj.ToHttpContent(new JsonSerializerSettings());
		}
		public static HttpContent ToHttpContent<T>(this T obj, JsonSerializerSettings settings)
		{
			var content = obj as HttpContent;
			if(content == null)
				throw new NullReferenceException("Content is null!");

			return obj.Serialize(settings);
		}
		public static HttpContent Serialize<T>(this T obj, JsonSerializerSettings settings)
		{
			var stringContent = new StringContent(JsonConvert.SerializeObject(obj, settings));
			stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			return stringContent;
		}
	}
}