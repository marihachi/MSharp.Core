using Codeplex.Data;
using MSharp.Core.Utility;
using MSharp.Data.Entity.Enum;
using MSharp.Core.Data.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSharp.Data.Entity
{
	/// <summary>
	/// ポストエンティティを表します。
	/// </summary>
	public abstract class PostEntity
	{
		public PostEntity(string jsonString)
		{
			try
			{
				var j = DynamicJson.Parse(jsonString);

				Id = j.id;
				ApplicationId = j.app() ? j.app : null;
				ChannelId = j.channel() ? j.channel : null;
				CreatedAt = DateTime.Parse(j.createdAt);
				Cursor = j.cursor() ? (int?)j.cursor : null;
				IsDeleted = j.isDeleted() ? j.isDeleted : null;
				IsLiked = j.isLiked() ? j.isLiked : null;
				IsReposted = j.isReposted() ? j.isReposted : null;
				LikesCount = j.likesCount() ? (int?)j.likesCount : null;
				NextPostId = j.nextPost() ? j.nextPost : null;
				PreviousPostId = j.prevPost() ? j.prevPost : null;
				RepliesCount = j.repliesCount() ? (int?)j.repliesCount : null;
				RepostsCount = j.repostsCount() ? (int?)j.repostsCount : null;
				User = j.user() ? new UserEntity(j.user.ToString()) : null;
			}
			catch (Exception ex)
			{
				throw new MSharpEntityException("JSONのパースに失敗しました。", ex);
			}
		}

		public static PostEntity ConvertPostEntity(string postJsonString)
		{
			var j = DynamicJson.Parse(postJsonString);

			PostEntity post;

			if (j.type == "status")
				post = new StatusEntity(j.ToString());
			else if (j.type == "reply")
				post = new ReplyEntity(j.ToString());
			else if (j.type == "repost")
				post = new RepostEntity(j.ToString());
			else
				throw new MSharpEntityException($"PostType '{j.type}'は不明です。");

			return post;
		}

		public PostType Type { get; set; } = PostType.Unknown;
		public string Id { get; set; }
		public string ApplicationId { get; set; }
		public string ChannelId { get; set; }
		public DateTime CreatedAt { get; set; }
		public int? Cursor { get; set; }
		public bool? IsDeleted { get; set; }
		public bool? IsLiked { get; set; }
		public bool? IsReposted { get; set; }
		public int? LikesCount { get; set; }
		public string NextPostId { get; set; }
		public string PreviousPostId { get; set; }
		public int? RepliesCount { get; set; }
		public int? RepostsCount { get; set; }
		public UserEntity User { get; set; }
	}

	/// <summary>
	/// ステータスのポストエンティティを表します。
	/// </summary>
	public class StatusEntity : PostEntity
	{
		public StatusEntity(string jsonString)
			: base(jsonString)
		{
			try
			{
				var j = DynamicJson.Parse(jsonString);

				if (j.type != "status")
					throw new ArgumentException("与えられたJSONデータがStatusではありません。");

				Type = PostType.Status;
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
				Text = j.text() ? j.text : null;
			}
			catch (Exception ex)
			{
				throw new MSharpEntityException("JSONのパースに失敗しました。", ex);
			}
		}

		public List<AlbumFileEntity> Files { get; set; }
		public List<string> Hashtags { get; set; }
		public string Text { get; set; }
	}

	/// <summary>
	/// リプライのポストエンティティを表します。
	/// </summary>
	public class ReplyEntity : StatusEntity
	{
		public ReplyEntity(string jsonString)
			: base(jsonString)
		{
			try
			{
				var j = DynamicJson.Parse(jsonString);

				if (j.type == "reply")
					throw new ArgumentException("与えられたJSONデータがReplyではありません。");

				Type = PostType.Reply;

				if (j.inReplyToPostId() && j.inReplyToPost() && j.inReplyToPostId != null)
				{
					// 中身がポストオブジェクトであれば
					if (j.inReplyToPost.ToString() != j.inReplyToPostId)
					{
						_InReplyToPost = ConvertPostEntity(j.inReplyToPost.ToString());
					}
					_InReplyToPostId = j.inReplyToPostId;
				}
			}
			catch (Exception ex)
			{
				throw new MSharpEntityException("JSONのパースに失敗しました。", ex);
			}
		}

		public Task<PostEntity> InReplyToPost
		{
			get
			{
				var t = Task.Run(async () =>
				{
					if (_InReplyToPost == null)
					{
						var post = await MisskeyRequest.POST(null, "posts/show", new Dictionary<string, string> { { "post-id", _InReplyToPostId } });
						_InReplyToPost = ConvertPostEntity(post.Content);
					}
					return _InReplyToPost;
				});
				return t;
			}
		}
		private PostEntity _InReplyToPost { get; set; }
		private string _InReplyToPostId { get; set; }
	}

	/// <summary>
	/// リポストのポストエンティティを表します。
	/// </summary>
	public class RepostEntity : PostEntity
	{
		public RepostEntity(string jsonString)
			: base(jsonString)
		{
			try
			{
				var j = DynamicJson.Parse(jsonString);

				if (j.type == "repost")
					throw new ArgumentException("与えられたJSONデータがRepostではありません。");

				Type = PostType.Repost;

				Post = j.post() ? ConvertPostEntity(j.post.ToString()) : null;
			}
			catch (Exception ex)
			{
				throw new MSharpEntityException("JSONのパースに失敗しました。", ex);
			}
		}

		public PostEntity Post { get; set; }
	}
}
