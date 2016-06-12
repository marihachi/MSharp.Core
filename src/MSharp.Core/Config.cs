using System;

namespace MSharp.Core
{
	public static class Config
	{
		public static string SessionKeyName { get; set; } = "hmsk";
		public static Uri Url { get; set; } = new Uri("https://misskey.xyz");
		public static Uri LoginUrl { get; set; } = new Uri("https://login.misskey.xyz");
		public static Uri ApiUrl { get; set; } = new Uri("https://himasaku.misskey.xyz");
		public static Uri NonLoginApiUrl { get; set; } = new Uri("https://api.misskey.xyz");
		public static Uri StreamingApiUrl { get; set; } = new Uri("https://himasaku.misskey.xyz:3000");
	}
}
