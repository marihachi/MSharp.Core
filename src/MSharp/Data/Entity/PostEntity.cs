using Codeplex.Data;
using MSharp.Data.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MSharp.Data.Entity
{
	public class PostEntity : IStatus, IReply, IRepost
	{
		public PostEntity(string jsonString)
		{
			try
			{
				Debug.WriteLine("Post: "+jsonString);
				var j = DynamicJson.Parse(jsonString);

				if (j.type == "status")
					Type = PostType.Status;

				if (j.type == "repost")
					Type = PostType.Repost;

				if (j.type == "reply")
					Type = PostType.Reply;

				Id = j.id;
				ApplicationId = j.app() ? j.app : null;
				ChannelId = j.channel() ? j.channel : null;
				CreatedAt = DateTime.Parse(j.createdAt);
				Cursor = j.cursor() ? (int?)j.cursor : null;
				if (j.files() && j.files != null)
				{
					Files = new List<AlbumFileEntity>();
					foreach (var file in j.files)
						Files.Add(new AlbumFileEntity(file.ToString()));
				}
				if (j.hashtags() && j.files != null)
				{
					Hashtags = new List<string>();
					foreach (var hashtag in j.hashtags)
						Hashtags.Add(hashtag);
				}
				InReplyToPost = j.inReplyToPost() ? new PostEntity(j.inReplyToPost.ToString()) : null;
				IsDeleted = j.isDeleted() ? j.isDeleted : null;
				IsLiked = j.isLiked() ? j.isLiked : null;
				IsReposted = j.isReposted() ? j.isReposted : null;
				LikesCount = j.likesCount() ? (int?)j.likesCount : null;
				NextPostId = j.nextPost() ? j.nextPost : null;
				Post = j.post() ? new PostEntity(j.post.ToString()) : null;
				PreviousPostId = j.prevPost() ? j.prevPost : null; ;
				RepliesCount = j.repliesCount() ? (int?)j.repliesCount : null;
				RepostsCount = j.repostsCount() ? (int?)j.repostsCount : null;
				Text = j.text() ? j.text : null;
				User = j.user() ? new UserEntity(j.user.ToString()) : null;
			}
			catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		public PostType Type { get; set; } = PostType.Unknown;
		public string Id { get; set; }
		public string ApplicationId { get; set; }
		public string ChannelId { get; set; }
		public DateTime CreatedAt { get; set; }
		public int? Cursor { get; set; }
		public List<AlbumFileEntity> Files { get; set; }
		public List<string> Hashtags { get; set; }
		public PostEntity InReplyToPost { get; set; }
		public bool? IsDeleted { get; set; }
		public bool? IsLiked { get; set; }
		public bool? IsReposted { get; set; }
		public int? LikesCount { get; set; }
		public string NextPostId { get; set; }
		public PostEntity Post { get; set; }
		public string PreviousPostId { get; set; }
		public int? RepliesCount { get; set; }
		public int? RepostsCount { get; set; }
		public string Text { get; set; }
		public UserEntity User { get; set; }
	}

	public interface IPost
	{
		string Id { get; set; }
		string ApplicationId { get; set; }
		string ChannelId { get; set; }
		DateTime CreatedAt { get; set; }
		int? Cursor { get; set; }
		bool? IsDeleted { get; set; }
		bool? IsLiked { get; set; }
		bool? IsReposted { get; set; }
		int? LikesCount { get; set; }
		string NextPostId { get; set; }
		string PreviousPostId { get; set; }
		int? RepliesCount { get; set; }
		int? RepostsCount { get; set; }
		UserEntity User { get; set; }
	}

	public interface IStatus : IPost
	{
		List<AlbumFileEntity> Files { get; set; }
		List<string> Hashtags { get; set; }
		string Text { get; set; }
	}

	public interface IReply : IStatus
	{
		PostEntity InReplyToPost { get; set; }
	}

	public interface IRepost : IPost
	{
		PostEntity Post { get; set; }
	}
}
