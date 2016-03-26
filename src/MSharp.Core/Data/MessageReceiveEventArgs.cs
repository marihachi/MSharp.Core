using System;

namespace MSharp.Core.Data
{
	public class MessageReceiveEventArgs : EventArgs
	{
		public MessageReceiveEventArgs(string json)
		{
			JsonData = json;
		}

		/// <summary>
		/// 受信したJSONデータを取得します。
		/// </summary>
		public string JsonData { get; private set; }
	}
}
