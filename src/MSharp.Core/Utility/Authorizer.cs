using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MSharp.Core.Utility
{
	public static class Authorizer
	{
		/// <summary>
		/// Misskeyへの認証を試みます。
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public static async Task<SessionInfo> Authorize(SessionInfo session, string username, string password)
		{
			var res = await Request.GET(Config.LoginUrl.AbsoluteUri);

			var cookies = res.Cookies;
			var csrfToken = Regex.Match(res.Content, "<meta name=\"csrf-token\" content=\"([a-zA-Z0-9_-]+)\">").Groups[1].ToString();

			var param = new Dictionary<string, string>();
			param.Add("_csrf", csrfToken);
			param.Add("screen-name", username);
			param.Add("password", password);

			res = await Request.POST(Config.LoginUrl.AbsoluteUri, param, cookies);

			return res.Content == "OK" ? new SessionInfo(csrfToken, cookies[Config.SessionKeyName]) : null;
		}
	}
}
