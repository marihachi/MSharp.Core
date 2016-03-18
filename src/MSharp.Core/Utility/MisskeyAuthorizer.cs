using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MSharp.Core.Utility
{
	public static class MisskeyAuthorizer
	{
		/// <summary>
		/// Misskeyへの認証を試みます。
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public static async Task<SessionInfo> Authorize(SessionInfo session, string username, string password)
		{
			var res = await HttpRequest.GET(session.Config.LoginUrl.AbsoluteUri);

			var cookies = res.Cookies;
			var csrfToken = Regex.Match(res.Content, "<meta name=\"csrf-token\" content=\"([a-zA-Z0-9_-]+)\">").Groups[1].ToString();

			var param = new Dictionary<string, string>();
			param.Add("_csrf", csrfToken);
			param.Add("screen-name", username);
			param.Add("password", password);

			res = await HttpRequest.POST(session.Config.LoginUrl.AbsoluteUri, param, cookies);

			return res.Content == "OK" ? new SessionInfo(csrfToken, cookies[session.Config.SessionKeyName], session.Config) : null;
		}
	}
}
