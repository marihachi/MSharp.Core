using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using MSharp.Core.Data;

namespace MSharp.Core.Utility
{
	/// <summary>
	/// HTTPの簡易なリクエストを表します。
	/// </summary>
	public static class HttpRequest
	{
		/// <summary>
		/// POSTリクエストを送信します。
		/// </summary>
		/// <param name="url"></param>
		/// <param name="content"></param>
		/// <param name="cookies"></param>
		/// <param name="additionalHeaders"></param>
		public static async Task<HttpResponse> POST(string url, HttpContent content, Dictionary<string, string> cookies = null, Dictionary<string, string> additionalHeaders = null)
		{
			using (var handler = new HttpClientHandler { ClientCertificateOptions = ClientCertificateOption.Automatic, AutomaticDecompression = DecompressionMethods.GZip })
			using (var client = new HttpClient(handler))
			{
				foreach (var i in cookies ?? new Dictionary<string, string>())
					handler.CookieContainer.Add(new Uri(url), new Cookie(i.Key, i.Value));

				foreach (var additionalHeader in additionalHeaders ?? new Dictionary<string, string>())
					client.DefaultRequestHeaders.Add(additionalHeader.Key, additionalHeader.Value);

				try
				{
					var res = await client.PostAsync(url, content);
					var result = await res.Content.ReadAsStringAsync();

					string value;
					cookies = new Dictionary<string, string>();
					foreach (Cookie i in handler.CookieContainer.GetCookies(new Uri(url)))
						if (!cookies.TryGetValue(i.Name, out value))
							cookies.Add(i.Name, i.Value);

					return new HttpResponse(result, cookies);
				}
				catch (Exception ex)
				{
					throw new Exception("リクエスト時にエラーが発生しました。", ex);
				}
			}
		}

		/// <summary>
		/// Multipart/FormDataの形式でPOSTリクエストを送信します。
		/// </summary>
		/// <param name="url"></param>
		/// <param name="contents"></param>
		/// <param name="cookies"></param>
		/// <param name="additionalHeaders"></param>
		public static async Task<HttpResponse> POST(string url, List<HttpContent> contents, Dictionary<string, string> cookies = null, Dictionary<string, string> additionalHeaders = null)
		{
			using (var formData = new MultipartFormDataContent())
			{
				foreach (var content in contents)
					formData.Add(content);

				return await POST(url, formData, cookies, additionalHeaders);
			}
		}

		/// <summary>
		/// bodyパラメータを指定してPOSTリクエストを送信します。
		/// </summary>
		/// <param name="url"></param>
		/// <param name="body"></param>
		/// <param name="cookies"></param>
		/// <param name="headers"></param>
		public static async Task<HttpResponse> POST(string url, Dictionary<string, string> body, Dictionary<string, string> cookies = null, Dictionary<string, string> headers = null)
		{
			var content = new FormUrlEncodedContent(body);
			return await POST(url, content, cookies, headers);
		}

		/// <summary>
		/// GETリクエストを送信します。
		/// </summary>
		public static async Task<HttpResponse> GET(string url, Dictionary<string, string> param = null, Dictionary<string, string> cookies = null, Dictionary<string, string> headers = null)
		{
			param = param ?? new Dictionary<string, string>();

			if (param.Count != 0)
			{
				var pers = new List<string>();
				foreach (var item in param)
					pers.Add(item.Key + "=" + item.Value);
				url += "?" + string.Join("&", pers);
			}

			using (var handle = new HttpClientHandler { ClientCertificateOptions = ClientCertificateOption.Automatic })
			using (var client = new HttpClient(handle))
			{
				foreach (var i in cookies ?? new Dictionary<string, string>())
					handle.CookieContainer.Add(new Uri(url), new Cookie(i.Key, i.Value));

				foreach (var item in headers ?? new Dictionary<string, string>())
					client.DefaultRequestHeaders.Add(item.Key, item.Value);

				string content = null;

				try
				{
					var res = await client.GetAsync(url);
					content = await res.Content.ReadAsStringAsync();
				}
				catch (Exception ex)
				{
					throw new Exception("リクエスト時にエラーが発生しました。", ex);
				}

				string a;

				cookies = new Dictionary<string, string>();
				foreach (Cookie i in handle.CookieContainer.GetCookies(new Uri(url)))
					if (!cookies.TryGetValue(i.Name, out a))
						cookies.Add(i.Name, i.Value);

				return new HttpResponse(content, cookies);
			}
		}
	}
}
