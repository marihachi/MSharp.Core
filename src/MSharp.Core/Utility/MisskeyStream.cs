using Codeplex.Data;
using MSharp.Core.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;

namespace MSharp.Core.Utility
{
	/// <summary>
	/// Misskeyとのストリーミング通信をサポートします。
	/// </summary>
	public class MisskeyStream
	{
		/// <summary>
		/// 新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="misskey"></param>
		/// <param name="endPoint"></param>
		public MisskeyStream(SessionInfo session, string endPoint)
		{
			Session = session;
			EndPoint = endPoint;
		}

		/// <summary>
		/// セッション情報のインスタンスを取得します。
		/// </summary>
		public SessionInfo Session { get; private set; }

		/// <summary>
		/// エンドポイントを取得します。
		/// </summary>
		public string EndPoint { get; private set; }

		/// <summary>
		/// 接続されているかどうかを示す値を取得します。
		/// </summary>
		public bool IsConnecting { get; private set; }

		private int ReconnectDelay { get; set; } = 1000;

		/// <summary>
		/// ストリームに接続したときに発生します。
		/// </summary>
		public event StreamConnectEventHandler StreamConnected;

		public delegate void StreamConnectEventHandler(object sender, EventArgs e);

		public void OnStreamConnected()
		{
			if (StreamConnected != null)
				StreamConnected(this, new EventArgs());
		}

		/// <summary>
		/// ストリームから切断されたときに発生します。
		/// </summary>
		public event StreamDisconnectEventHandler StreamDisconnected;

		public delegate void StreamDisconnectEventHandler(object sender, EventArgs e);

		public void OnStreamDisconnected()
		{
			if (StreamDisconnected != null)
				StreamDisconnected(this, new EventArgs());
		}

		/// <summary>
		/// メッセージを受信したときに発生します。
		/// </summary>
		public event MessageRecieveEventHandler MessageRecieved;

		public delegate void MessageRecieveEventHandler(object sender, MessageRecieveEventArgs e);

		public void OnMessageRecieved(string json)
		{
			if (MessageRecieved != null)
				MessageRecieved(this, new MessageRecieveEventArgs(json));
		}

		private CancellationTokenSource _Canceller { get; set; }

		protected TaskScheduler SyncTaskScheduler { get; set; }

		/// <summary>
		/// ストリームに接続します。
		/// </summary>
		/// <returns></returns>
		public virtual async Task ConnectAsync(bool isReconnect)
		{
			SyncTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
			_Canceller = new CancellationTokenSource();

			connectStart:

			try
			{
				await Task.Run(async () =>
				{
					var cookie = new Dictionary<string, string>();
					cookie.Add(Session.Config.SessionKeyName, Session.SessionKey);

					var streamUrl = Session.Config.StreamingApiUrl;

					var res = await Request.GET($"{streamUrl.Scheme}://{streamUrl.Host}:{streamUrl.Port}/socket.io/?EIO=3&transport=polling", null, cookie);

					var configSrc = res.Content.Substring(5);
					var config = DynamicJson.Parse(configSrc);

					var sid = config.sid;
					var pingInterval = (int)config.pingInterval;

					var content = new StringContent("17:40" + EndPoint);
					res = await Request.POST($"{streamUrl.Scheme}://{streamUrl.Host}:{streamUrl.Port}/socket.io/?EIO=3&transport=polling&sid={sid}", content, cookie);

					using (var ws = new WebSocket($"{(streamUrl.Scheme == "https" ? "wss" : "ws")}://{streamUrl.Host}:{streamUrl.Port}/socket.io/?EIO=3&transport=websocket&sid={sid}"))
					{
						ws.OnMessage += async(s, ev) =>
						{
							if (ev.Data == "3probe")
							{
								ws.Send("5");

								await Task.Factory.StartNew(() =>
								{
									OnStreamConnected();
								}, CancellationToken.None, TaskCreationOptions.None, SyncTaskScheduler);
							}
							else if (ev.Data[0] != '3')
							{
								var code = ev.Data.Substring(0, 2);

								if (code == "42")
								{
									var source = ev.Data.Substring(2);
									var json = source.Substring(source.IndexOf(',') + 1);

									OnMessageRecieved(json);
								}
							}
						};

						ws.Connect();
						ws.Send("2probe");

						IsConnecting = true;

						while (ws.IsAlive && IsConnecting)
						{
							Task.Delay(pingInterval, _Canceller.Token).Wait();
							ws.Send("2");
						}
					}
				}, _Canceller.Token);
			}
			catch (AggregateException ex)
			{
				var exs = ex.InnerExceptions;
				foreach (var i in exs)
				{
					if (!(i is TaskCanceledException))
					{
						Debug.WriteLine($"----------");
						Debug.WriteLine($"Type:{i.GetType().Name}");
						Debug.WriteLine($"Message:{i.Message}");
						Debug.WriteLine($"StackTrace:{i.StackTrace}");
						Debug.WriteLine($"----------");
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"----------");
				Debug.WriteLine($"Type:{ex.GetType().Name}");
				Debug.WriteLine($"Message:{ex.Message}");
				Debug.WriteLine($"StackTrace:{ex.StackTrace}");
				Debug.WriteLine($"----------");
			}

			IsConnecting = false;

			await Task.Factory.StartNew(() =>
			{
				OnStreamDisconnected();
			}, CancellationToken.None, TaskCreationOptions.None, SyncTaskScheduler);

			if (isReconnect && !_Canceller.IsCancellationRequested)
			{
				await Task.Delay(ReconnectDelay);
				ReconnectDelay += 500;

				goto connectStart;
			}
		}

		/// <summary>
		/// ストリームを切断します。
		/// </summary>
		public void Disconnect()
		{
			IsConnecting = false;
			_Canceller.Cancel();
		}
	}
}
