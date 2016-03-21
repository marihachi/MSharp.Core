using MSharp.Core.Data;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MSharp.Core.Utility
{
	public static class MisskeyRequest
	{
		public static async Task<HttpResponse> GET(SessionInfo session, string endPoint, Dictionary<string, string> parameters = null)
		{
			parameters = parameters ?? new Dictionary<string, string>();
			parameters.Add("_csrf", session.CsrfToken);

			if (endPoint.Length >= 1)
				if (endPoint[0] == '/')
					endPoint = endPoint.Substring(1);

			var cookies = new Dictionary<string, string>();

			if (session != null)
				cookies.Add(Config.SessionKeyName, session.SessionKey);

			return await Request.GET($"{(session != null ? Config.ApiUrl.AbsoluteUri : Config.NonLoginApiUrl.AbsoluteUri)}", parameters, cookies);
        }

		public static async Task<HttpResponse> POST(SessionInfo session, string endPoint, HttpContent content, Dictionary<string, string> extendHeaders = null)
		{
			if (endPoint.Length >= 1)
				if (endPoint[0] == '/')
					endPoint = endPoint.Substring(1);

			var cookies = new Dictionary<string, string>();

			if(session != null)
				cookies.Add(Config.SessionKeyName, session.SessionKey);

			return await Request.POST($"{(session != null ? Config.ApiUrl.AbsoluteUri : Config.NonLoginApiUrl.AbsoluteUri)}{endPoint}", content, cookies, extendHeaders);
		}

		public static async Task<HttpResponse> POST(SessionInfo session, string endPoint, Dictionary<string, string> parameters = null)
		{
			parameters = parameters ?? new Dictionary<string, string>();
			parameters.Add("_csrf", session.CsrfToken);

			if (endPoint.Length >= 1)
				if (endPoint[0] == '/')
					endPoint = endPoint.Substring(1);

			var cookies = new Dictionary<string, string>();

			if (session != null)
				cookies.Add(Config.SessionKeyName, session.SessionKey);

			return await Request.POST($"{(session != null ? Config.ApiUrl.AbsoluteUri : Config.NonLoginApiUrl.AbsoluteUri)}{endPoint}", parameters, cookies);
		}

		public static async Task<HttpResponse> POST(SessionInfo session, string endPoint, List<HttpContent> contents, Dictionary<string, string> additionalHeaders = null)
		{
			additionalHeaders = additionalHeaders ?? new Dictionary<string, string>();
			additionalHeaders.Add("csrf-token", session.CsrfToken);

			if (endPoint.Length >= 1)
				if (endPoint[0] == '/')
					endPoint = endPoint.Substring(1);

			var cookies = new Dictionary<string, string>();

			if (session != null)
				cookies.Add(Config.SessionKeyName, session.SessionKey);

			return await Request.POST($"{(session != null ? Config.ApiUrl.AbsoluteUri : Config.NonLoginApiUrl.AbsoluteUri)}{endPoint}", contents, cookies, additionalHeaders);
		}
	}
}
