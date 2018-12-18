using AccountAndJwt.Api.Contracts.Models;
using AccountAndJwt.Client.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AccountAndJwt.Client
{
	internal class Program
	{
		private static JsonSerializerSettings _serializerSettings;
		private static Token _token;


		static void Main(String[] args)
		{
			_serializerSettings = CreateSerializerSettings();

			if(!TryAuthorize(out var tokenResponse))
			{
				Console.WriteLine("Authorization failed");

				Console.ReadKey();
				return;
			}

			_token = new Token
			{
				AccessToken = tokenResponse.AccessToken,
				AccessTokenLifeTime = tokenResponse.AccessTokenLifeTime,
				RefreshToken = tokenResponse.RefreshToken
			};

			Console.WriteLine("Authorization succeed");
			Console.WriteLine($"{nameof(_token.AccessToken)}: {_token.AccessToken}");
			Console.WriteLine($"{nameof(_token.RefreshToken)}: {_token.RefreshToken}");

			var valueId = 1;
			if(!TryGetValue(out var value, _token.AccessToken, valueId))
			{
				Console.WriteLine($"Get the {valueId} value failed");

				Console.ReadKey();
				return;
			}

			Console.WriteLine();
			Console.WriteLine($"Value[{valueId}]: {value}");

			Console.WriteLine();
			Console.WriteLine("Refreshing the access token");
			if(!TryRefreshToken(out var refreshTokenResponse, _token.RefreshToken))
			{
				Console.WriteLine("Refreshing failed");

				Console.ReadKey();
				return;
			}

			_token.AccessToken = refreshTokenResponse.AccessToken;
			_token.AccessTokenLifeTime = refreshTokenResponse.AccessTokenLifeTime;

			Console.WriteLine();
			Console.WriteLine("Token successfully refreshed");
			Console.WriteLine($"{nameof(_token.AccessToken)}: {_token.AccessToken}");
			Console.WriteLine($"{nameof(_token.RefreshToken)}: {_token.RefreshToken}");

			valueId = 2;
			if(!TryGetValue(out value, _token.AccessToken, valueId))
			{
				Console.WriteLine($"Get the {valueId} value failed");

				Console.ReadKey();
				return;
			}

			Console.WriteLine();
			Console.WriteLine("All done!");
			Console.ReadKey();
		}


		// SUPPORT FUNCTIONS //////////////////////////////////////////////////////////////////////
		private static Boolean TryAuthorize(out AuthorizeByCredentialsResponseAm requestedToken)
		{
			using(var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Clear();
				var firstResponse = client.PostAsync("http://localhost:58751/api/Token/AuthorizeByCredentials", Serialize(new AuthorizeByCredentialsRequestAm()
				{
					Login = "Member1",
					Password = "123"
				}, _serializerSettings)).Result;
				if(!firstResponse.IsSuccessStatusCode)
				{
					requestedToken = new AuthorizeByCredentialsResponseAm();
					return false;
				}

				var firstResult = firstResponse.Content.ReadAsStringAsync().Result;
				requestedToken = JsonConvert.DeserializeObject<AuthorizeByCredentialsResponseAm>(firstResult);

				return true;
			}
		}
		private static Boolean TryGetValue(out String value, String accessToken, Int32 valueId)
		{
			using(var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

				var secondResponse = client.GetAsync($"http://localhost:58751/api/values/{valueId}").Result;
				if(!secondResponse.IsSuccessStatusCode)
				{
					value = String.Empty;
					return false;
				}

				var result = secondResponse.Content.ReadAsStringAsync().Result;
				value = JsonConvert.DeserializeObject<ValueAm>(result).Value;
				return true;
			}
		}
		private static Boolean TryRefreshToken(out RefreshTokenResponseAm newTokenResponse, String refreshToken)
		{
			using(var client = new HttpClient())
			{
				var refreshResponse = client.GetAsync($"http://localhost:58751/api/token/RefreshToken?refreshToken={refreshToken}").Result;
				if(!refreshResponse.IsSuccessStatusCode)
				{
					newTokenResponse = new RefreshTokenResponseAm();
					return false;
				}

				var refreshresult = refreshResponse.Content.ReadAsStringAsync().Result;
				newTokenResponse = JsonConvert.DeserializeObject<RefreshTokenResponseAm>(refreshresult);
				return true;
			}
		}
		private static HttpContent Serialize<T>(T obj, JsonSerializerSettings settings)
		{
			var stringContent = new StringContent(JsonConvert.SerializeObject(obj, settings));
			stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			return stringContent;
		}
		private static JsonSerializerSettings CreateSerializerSettings()
		{
			return new JsonSerializerSettings()
			{
				NullValueHandling = NullValueHandling.Ignore,
				MissingMemberHandling = MissingMemberHandling.Ignore,
				TypeNameHandling = TypeNameHandling.None,
				TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
			};
		}
	}
}
