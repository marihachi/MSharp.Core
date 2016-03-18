using System;

namespace MSharp.Core
{
	public class Config
	{
		public string SessionKeyName { get; set; } = "hmsk";
		public Uri Url { get; set; } = new Uri("https://misskey.xyz");
		public Uri LoginUrl { get; set; } = new Uri("https://login.misskey.xyz");
		public Uri ApiUrl { get; set; } = new Uri("https://himasaku.misskey.xyz");
		public Uri StreamingApiUrl { get; set; } = new Uri("https://himasaku.misskey.xyz:3000");
	}
}
