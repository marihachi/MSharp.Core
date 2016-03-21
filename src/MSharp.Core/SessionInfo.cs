namespace MSharp.Core
{
	public class SessionInfo
	{
		public SessionInfo() { }

		public SessionInfo(string csrfToken, string sessionKey)
		{
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
	}
}
