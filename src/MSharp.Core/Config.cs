using System;
using System.Net;

namespace MSharp.Core
{
	public static class Config
	{
		public static string SessionKeyName { get; set; } = "hmsk";
		public static Uri Url { get; set; } = new Uri("https://misskey.link");
		public static Uri LoginUrl { get; set; } = new Uri("https://login.misskey.link");
		public static Uri ApiUrl { get; set; } = new Uri("https://himasaku.misskey.link");
		public static Uri NonLoginApiUrl { get; set; } = new Uri("https://api.misskey.link");
		public static Uri StreamingApiUrl { get; set; } = new Uri("https://streaming.misskey.link:443");
		public static SecurityProtocolType UseSecurityProtocol { get; set; } = SecurityProtocolType.Tls12;
	}
}
