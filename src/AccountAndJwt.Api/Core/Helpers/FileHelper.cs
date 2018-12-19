using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AccountAndJwt.Api.Core.Helpers
{
	public static class FileHelper
	{
		// READING ////////////////////////////////////////////////////////////////////////////////
		public static Task<T> GetFromJsonFileAsync<T>(String filePath)
		{
			return Task.Run(() => GetFromJsonFile<T>(filePath, Encoding.UTF8));
		}
		public static Task<T> GetFromJsonFileAsync<T>(String filePath, Encoding encoding)
		{
			return Task.Run(() => GetFromJsonFile<T>(filePath, encoding));
		}

		public static T GetFromJsonFile<T>(String filePath)
		{
			return GetFromJsonFile<T>(filePath, Encoding.UTF8);
		}
		public static T GetFromJsonFile<T>(String filePath, Encoding encoding)
		{
			if(!File.Exists(filePath))
				return default(T);

			using(var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read))
			{
				using(var streamReader = new StreamReader(stream, encoding))
				{
					using(var textReader = new JsonTextReader(streamReader))
					{
						return new JsonSerializer().Deserialize<T>(textReader);
					}
				}
			}
		}

		private static Task<String> GetFileTextAsync(String filePath)
		{
			return Task.Run(() => GetFileText(filePath));
		}
		private static Task<String> GetFileTextAsync(String filePath, Encoding encoding)
		{
			return Task.Run(() => GetFileText(filePath, encoding));
		}

		private static String GetFileText(String filePath)
		{
			return GetFileText(filePath, Encoding.UTF8);
		}
		private static String GetFileText(String filePath, Encoding encoding)
		{
			if(!File.Exists(filePath))
				return String.Empty;

			using(var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read))
			{
				using(var streamReader = new StreamReader(stream, encoding))
				{
					return streamReader.ReadToEnd();
				}
			}
		}


		// WRITING ////////////////////////////////////////////////////////////////////////////////
		public static Task SaveOnDiskAsJsonAsync<T>(this T items, String filePath, Encoding encoding)
		{
			return Task.Run(() => { SaveOnDiskAsJson(items, filePath, encoding); });
		}
		public static Task SaveOnDiskAsJsonAsync<T>(this T items, String filePath)
		{
			return Task.Run(() => { SaveOnDiskAsJson(items, filePath); });
		}

		public static void SaveOnDiskAsJson<T>(this T items, String filePath)
		{
			SaveOnDiskAsJson(items, filePath, Encoding.UTF8);
		}
		public static void SaveOnDiskAsJson<T>(this T items, String filePath, Encoding encoding)
		{
			using(var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
			{
				using(var streamWriter = new StreamWriter(stream, encoding))
				{
					using(var textWriter = new JsonTextWriter(streamWriter))
					{
						new JsonSerializer().Serialize(textWriter, items, typeof(T));
					}
				}
			}
		}

		public static async Task SaveOnDiskAsync(this String str, String filePath)
		{
			await SaveOnDiskAsync(str, filePath, Encoding.UTF8);
		}
		public static async Task SaveOnDiskAsync(this String str, String filePath, Encoding encoding)
		{
			using(var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
			{
				using(var streamWriter = new StreamWriter(stream, encoding))
				{
					await streamWriter.WriteAsync(str);
				}
			}
		}

		public static void SaveOnDisk(this String str, String filePath)
		{
			SaveOnDisk(str, filePath, Encoding.UTF8);
		}
		public static void SaveOnDisk(this String str, String filePath, Encoding encoding)
		{
			using(var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
			{
				using(var streamWriter = new StreamWriter(stream, encoding))
				{
					streamWriter.Write(str);
				}
			}
		}
	}
}