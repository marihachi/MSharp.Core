using Codeplex.Data;
using MSharp.Core.Utility;
using MSharp.Data.Entity;
using MSharp.Data.EventArgs;
using System.Diagnostics;

namespace MSharp.API
{
	public class HomeStream : MisskeyStream
	{
		public HomeStream(Misskey misskey)
			: base(misskey.Session, "/streaming/home")
		{
			MessageRecieved += (s, ev) =>
			{
				var json = DynamicJson.Parse(ev.JsonData);

				if (json[0] == "post")
				{
					string post = json[1].ToString();
					OnPostRecieved(new PostRecieveEventArgs(new PostEntity(post)));
				}
				else if (json[0] == "notification")
				{
					string notification = json[1].ToString();
					OnNotificationRecieved(new NotificationRecieveEventArgs(new NotificationEntity(notification)));
				}
				else
				{
					Debug.WriteLine($"不明なイベントが発行されました。");
					Debug.WriteLine($"イベント名: {json[0]}");
					Debug.WriteLine($"内容: {json[1]}");
				}
			};
		}

		/// <summary>
		/// 投稿内容を受信したときに発生します。
		/// </summary>
		public event PostEventHandler PostRecieved;

		public delegate void PostEventHandler(object sender, PostRecieveEventArgs e);

		public void OnPostRecieved(PostRecieveEventArgs e)
		{
			if (PostRecieved != null)
				PostRecieved(this, e);
		}

		/// <summary>
		/// 通知内容を受信したときに発生します。
		/// </summary>
		public event NotificationEventHandler NotificationRecieved;

		public delegate void NotificationEventHandler(object sender, NotificationRecieveEventArgs e);

		public void OnNotificationRecieved(NotificationRecieveEventArgs e)
		{
			if (NotificationRecieved != null)
				NotificationRecieved(this, e);
		}
	}
}
