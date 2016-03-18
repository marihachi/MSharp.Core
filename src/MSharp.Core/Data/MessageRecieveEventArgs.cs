using System;

namespace MSharp.Core.Data
{
	public class MessageRecieveEventArgs : EventArgs
	{
		public MessageRecieveEventArgs(string json)
		{
			JsonData = json;
		}

		/// <summary>
		/// 受信したJSONデータを取得します。
		/// </summary>
		public string JsonData { get; private set; }
	}
}
