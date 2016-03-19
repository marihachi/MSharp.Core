using Codeplex.Data;
using MSharp.Data.Entity.Enum;
using System;
using System.Diagnostics;

namespace MSharp.Data.Entity
{
	public class NotificationEntity :
		INotification,
		ISelfNotification,
		ILikeNotification,
		IRepostNotification,
		IFollowNotification,
		IMentionNotification
	{
		public NotificationEntity(string jsonString)
		{
			try
			{
				var j = DynamicJson.Parse(jsonString);

				if (j.type == "self-notification")
					Type = NotificationType.SelfNotification;

				if (j.type == "like")
					Type = NotificationType.Like;

				if (j.type == "repost")
					Type = NotificationType.Repost;

				if (j.type == "follow")
					Type = NotificationType.Follow;

				if (j.type == "mention")
					Type = NotificationType.Mention;

				Id = j.id;
				ApplicationId = j.app() ? j.app : null;
				CreatedAt = DateTime.Parse(j.createdAt);
				Cursor = j.cursor() ? (int?)j.cursor : null;
				IsRead = j.isRead() ? j.isRead : null;
				UserId = j.user() ? j.user : null;
			}
			catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		public NotificationType Type { get; set; } = NotificationType.Unknown;
		public string Id { get; set; }
		public string ApplicationId { get; set; }
		public DateTime CreatedAt { get; set; }
		public int? Cursor { get; set; }
		public bool? IsRead { get; set; }
		public string UserId { get; set; }
	}

	public interface INotification
	{
		string Id { get; set; }
		string ApplicationId { get; set; }
		DateTime CreatedAt { get; set; }
		int? Cursor { get; set; }
		bool? IsRead { get; set; }
		string UserId { get; set; }
	}

	public interface ISelfNotification : INotification
	{
	}

	public interface ILikeNotification : INotification
	{
	}

	public interface IRepostNotification : INotification
	{
	}

	public interface IFollowNotification : INotification
	{
	}

	public interface IMentionNotification : INotification
	{
	}
}
