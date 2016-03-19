using MSharp.Data.Entity;

namespace MSharp.Data.EventArgs
{
	public class NotificationRecieveEventArgs : System.EventArgs
	{
		public NotificationRecieveEventArgs(NotificationEntity notification)
		{
			Data = notification;
		}

		public NotificationEntity Data { get; private set; }
	}
}
