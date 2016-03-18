using System.Collections.Generic;

namespace MSharp.Core.Data
{
	/// <summary>
	/// HTTPの簡易なレスポンスを表します。
	/// </summary>
	public class HttpResponse
	{
		public HttpResponse(string content, Dictionary<string, string> cookies)
		{
			Content = content;
			Cookies = cookies;
		}

		/// <summary>
		/// 結果として返ってきた文字列を取得します。
		/// </summary>
		public string Content { get; private set; }

		/// <summary>
		/// クッキーを取得します。
		/// </summary>
		public Dictionary<string, string> Cookies { get; private set; }
	}
}
