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
		public static async Task<PostEntity> Create(string postJsonString)
		{
			return await Task.Run(async() =>
			{
				PostEntity post = null;
				var j = DynamicJson.Parse(postJsonString);

				if (j.type == "status")
					post = StatusEntity.Create(j);
				else if (j.type == "reply")
					post = await ReplyEntity.Create(j);
				else if (j.type == "repost")
					post = await RepostEntity.Create(j);
				else
					throw new MSharpEntityException($"PostType '{j.type}'は不明です。");
				return post;
			});
		}

		protected void Initialize(DynamicJson json)
		{
			try
			{
				dynamic j = json;

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
		public static StatusEntity Create(DynamicJson json)
		{
			var entity = new StatusEntity();
			entity.Initialize(json);
			return entity;
		}

		protected StatusEntity() { }

		protected new void Initialize(DynamicJson json)
		{
			base.Initialize(json);

			try
			{
				dynamic j = json;

				if (!(j.type == "status" || j.type == "reply"))
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
		public static async new Task<ReplyEntity> Create(DynamicJson json)
		{
			var entity = new ReplyEntity();
			await entity.Initialize(json);
			return entity;
		}

		protected ReplyEntity() { }

		public async new Task Initialize(DynamicJson json)
		{
			base.Initialize(json);

			dynamic j = json;

			if (j.type != "reply")
				throw new ArgumentException("与えられたJSONデータがReplyではありません。");

			if (!j.inReplyToPostId() || !j.inReplyToPost() || j.inReplyToPost == null || j.inReplyToPostId == null)
				throw new MSharpEntityException("Replyとして判断されたオブジェクトにinReplyToPostId要素またはinReplyToPost要素の少なくともどちらかの要素が存在しませんでした。");

			Type = PostType.Reply;

			try
			{
				// 中身がポストオブジェクトであれば
				if (j.inReplyToPost.ToString() != j.inReplyToPostId)
					_InReplyToPost = await Create((string)j.inReplyToPost.ToString());

				_InReplyToPostId = j.inReplyToPostId;
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
						_InReplyToPost = await Create(post.Content);
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
		public static async Task<RepostEntity> Create(DynamicJson json)
		{
			var entity = new RepostEntity();
			await entity.Initialize(json);
			return entity;
		}

		protected RepostEntity() { }

		public async new Task Initialize(DynamicJson json)
		{
			base.Initialize(json);

			dynamic j = json;

			if (j.type != "repost")
				throw new ArgumentException("与えられたJSONデータがRepostではありません。");

			Type = PostType.Repost;

			try
			{
				TargetPost = j.post() ? await PostEntity.Create((string)j.post.ToString()) : null;
			}
			catch (Exception ex)
			{
				throw new MSharpEntityException("JSONのパースに失敗しました。", ex);
			}
		}

		public PostEntity TargetPost { get; set; }
	}
}
