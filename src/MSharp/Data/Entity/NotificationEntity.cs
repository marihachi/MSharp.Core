using Codeplex.Data;
using MSharp.Core.Data.Exceptions;
using MSharp.Data.Entity.Enum;
using System;
using System.Diagnostics;

namespace MSharp.Data.Entity
{
	public abstract class NotificationEntity
	{
		public NotificationEntity(DynamicJson json)
		{
			try
			{
				dynamic j = json;

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

		public static NotificationEntity Create(string jsonString)
		{
			NotificationEntity notify = null;
			var j = DynamicJson.Parse(jsonString);

			if (j.type == "self-notification")
				notify = new SelfNotification(j);
			else if (j.type == "like")
				notify = new LikeNotification(j);
			else if (j.type == "repost")
				notify = new RepostNotification(j);
			else if (j.type == "follow")
				notify = new FollowNotification(j);
			else if (j.type == "mention")
				notify = new MentionNotification(j);
			else
				throw new MSharpEntityException($"NotificationType '{j.type}'は不明です。");

			return notify;
		}

		public NotificationType Type { get; set; } = NotificationType.Unknown;
		public string Id { get; set; }
		public string ApplicationId { get; set; }
		public DateTime CreatedAt { get; set; }
		public int? Cursor { get; set; }
		public bool? IsRead { get; set; }
		public string UserId { get; set; }
	}

	public class SelfNotification : NotificationEntity
	{
		public SelfNotification(DynamicJson json) : base(json) { }
	}

	public class LikeNotification : NotificationEntity
	{
		public LikeNotification(DynamicJson json) : base(json) { }
	}

	public class RepostNotification : NotificationEntity
	{
		public RepostNotification(DynamicJson json) : base(json) { }
	}

	public class FollowNotification : NotificationEntity
	{
		public FollowNotification(DynamicJson json) : base(json) { }
	}

	public class MentionNotification : NotificationEntity
	{
		public MentionNotification(DynamicJson json) : base(json) { }
	}
}
