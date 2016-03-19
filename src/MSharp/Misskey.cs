using MSharp.API;
using MSharp.Core;
using MSharp.Core.Utility;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MSharp
{
	public class Misskey
	{
		public Misskey(SessionInfo session = null)
		{
			Session = session;

			if (Session != null)
			{
				Album = new Album(this);
				Posts = new Posts(this);
				HomeStream = new HomeStream(this);
			}
		}

		/// <summary>
		/// セッション情報
		/// </summary>
		public SessionInfo Session { get; set; }

		/// <summary>
		/// Misskeyへの認証を試みます。
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		public async Task<Misskey> Authorize(string username, string password)
		{
			var session = await Authorizer.Authorize(new SessionInfo(), username, password);
			return session != null ? new Misskey(session) : null;
		}

		/// <summary>
		/// このインスタンスのアルバムに関するAPIを表します。
		/// </summary>
		public Album Album { get; private set; }

		/// <summary>
		/// このインスタンスの投稿に関するAPIを表します。
		/// </summary>
		public Posts Posts { get; private set; }

		/// <summary>
		/// このインスタンスのホームストリームを表します。
		/// </summary>
		public HomeStream HomeStream { get; private set; }

		/// <summary>
		/// Misskeyに汎用のGETリクエストを送信します。
		/// </summary>
		/// <param name="method"></param>
		/// <param name="endPoint"></param>
		/// <param name="parameters"></param>
		public async Task<string> GET(string endPoint, Dictionary<string, string> parameters = null)
		{
			var req = await MisskeyRequest.GET(Session, endPoint, parameters);
			return req.Content;
		}

		/// <summary>
		/// Misskeyに汎用のPOSTリクエストを送信します。
		/// </summary>
		/// <param name="method"></param>
		/// <param name="endPoint"></param>
		/// <param name="parameters"></param>
		public async Task<string> POST(string endPoint, Dictionary<string, string> parameters = null)
		{
			var req = await MisskeyRequest.POST(Session, endPoint, parameters);
			return req.Content;
		}

		/// <summary>
		/// Misskeyに汎用のPOSTリクエストを送信します。
		/// </summary>
		/// <param name="method"></param>
		/// <param name="endPoint"></param>
		/// <param name="parameters"></param>
		public async Task<string> POST(string endPoint, HttpContent content, Dictionary<string, string> extendHeaders)
		{
			var req = await MisskeyRequest.POST(Session, endPoint, content, extendHeaders);
			return req.Content;
		}
	}
}
