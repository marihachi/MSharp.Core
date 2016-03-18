namespace MSharp.Core
{
	public class SessionInfo
	{
		public SessionInfo(Config config = null)
		{
			Config = config ?? new Config();
		}

		public SessionInfo(string csrfToken, string sessionKey, Config config = null)
		{
			Config = config ?? new Config();
			CsrfToken = csrfToken;
			SessionKey = sessionKey;
		}

		/// <summary>
		/// CSRFトークン
		/// </summary>
		public string CsrfToken { get; set; }

		/// <summary>
		/// セッションを確立するためのキー
		/// </summary>
		public string SessionKey { get; set; }

		/// <summary>
		/// 設定情報
		/// </summary>
		public Config Config { get; private set; }
	}
}
