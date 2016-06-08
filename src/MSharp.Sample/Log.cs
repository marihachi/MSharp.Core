using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace MSharp.Sample
{
	public static class Log
	{
		public static string GetErrorMessageFromException(Exception ex)
		{
			var error = "";
			error += $"Time: {DateTime.Now.ToString()}\r\n";
			error += $"Name: {ex.GetType().Name}\r\n";
			error += $"Message: {ex.Message}\r\n";
			error += $"StackTrace: {ex.StackTrace}";
			return error;
		}

		public static async Task AddAsync(string error, string fileName = null)
		{
			var log = "";
			fileName = fileName ?? $"{DateTime.Now.ToString("yyyy-MM-dd")}.log";
			var path = $"logs/{fileName}";

			if(!Directory.Exists("logs"))
				Directory.CreateDirectory("logs");

			if (!File.Exists(path))
				using (var st = File.Create(path)) { }
			else
				using (var sr = new StreamReader(path))
					log = await sr.ReadToEndAsync();

			using (var sw = new StreamWriter(path))
			{
				await sw.WriteAsync(log);
				await sw.WriteLineAsync(error);
				await sw.WriteLineAsync("--------------------");
			}

			Debug.WriteLine(error);
			Debug.WriteLine("--------------------");
		}

		public static async Task AddAsync(Exception ex, string fileName = null)
		{
			var error = GetErrorMessageFromException(ex);
			await AddAsync(error, fileName);
		}
	}
}
