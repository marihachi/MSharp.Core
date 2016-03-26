using Codeplex.Data;
using MSharp.Core.Utility;
using MSharp.Data;
using MSharp.Data.Entity;
using MSharp.Data.Entity.Enum;
using MSharp.Core.Data.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSharp.API
{
	/// <summary>
	/// ポストに関するAPIをサポートします。
	/// </summary>
	public class Posts
	{
		public Posts(Misskey misskey)
		{
			_Misskey = misskey;
		}
		private Misskey _Misskey { get; set; }

		/// <summary>
		/// ポストの内容を取得します。
		/// </summary>
		/// <param name="postId"></param>
		/// <returns></returns>
		public async Task<PostEntity> Show(string postId)
		{
			if (string.IsNullOrEmpty(postId))
				throw new ArgumentNullException("postId");

			var body = new Dictionary<string, string>();

			body.Add("post-id", postId);

			var res = (await MisskeyRequest.POST(_Misskey.Session, "posts/show", body)).Content;

			if (res == "not-found")
				throw new ArgumentException("指定されたpostIdのポストは見つかりませんでした。");

			return await PostEntity.Create(res);
		}

		/// <summary>
		/// ポストを投稿します。
		/// </summary>
		/// <param name="text"></param>
		/// <param name="fileIds"></param>
		public async Task<PostEntity> Create(string text, List<string> fileIds)
		{
			if (string.IsNullOrEmpty(text))
				throw new ArgumentNullException("text");

			var body = new Dictionary<string, string>();
			body.Add("text", text);

			if (fileIds != null)
			{
				fileIds.RemoveAll(i => i == null);

				if (fileIds.Count != 0)
					body.Add("files", string.Join(",", fileIds));
			}

			var res = (await MisskeyRequest.POST(_Misskey.Session, "posts/create", body)).Content;

			if (res == "content-duplicate")
				throw new MSharpApiException("投稿内容が重複しています。");

			return await PostEntity.Create(res);
		}

		/// <summary>
		/// ポストを投稿します。
		/// </summary>
		/// <param name="text"></param>
		/// <param name="fileId"></param>
		public async Task<PostEntity> Create(string text, string fileId)
		{
			return await Create(text, new List<string> { fileId });
		}

		/// <summary>
		/// ポストを投稿します。
		/// </summary>
		/// <param name="text"></param>
		/// <param name="files"></param>
		public async Task<PostEntity> Create(string text, List<File> files)
		{
			var fileIds = new List<string>();

			if (files != null)
			{
				files = files.FindAll(i => i != null);

				foreach (var i in files)
				{
					var res = await _Misskey.Album.Upload(i);
					fileIds.Add(res.Id);
				}
			}

			return await Create(text, fileIds);
		}

		/// <summary>
		/// ポストを投稿します。
		/// </summary>
		/// <param name="text"></param>
		/// <param name="imageFile"></param>
		public async Task<PostEntity> Create(string text, File file = null)
		{
			return await Create(text, new List<File> { file });
		}

		/// <summary>
		/// ホームタイムラインの内容を取得します。
		/// </summary>
		/// <returns></returns>
		public async Task<List<PostEntity>> HomeTimeLine(int? limit = null, int? sinceCursor = null, int? maxCursor = null)
		{
			var body = new Dictionary<string, string>();

			if (limit != null)
			{
				if (limit >= 1 && limit <= 100)
					body.Add("limit", limit.ToString());
				else
					throw new ArgumentOutOfRangeException("limit は1～100の範囲内である必要があります。");
			}

			if (sinceCursor != null)
				body.Add("since-cursor", sinceCursor.ToString());

			if (maxCursor != null)
				body.Add("max-cursor", maxCursor.ToString());

			var res = (await MisskeyRequest.POST(_Misskey.Session, "posts/timeline", body)).Content;

			var postList = new List<PostEntity>();

			foreach (var post in DynamicJson.Parse(res))
				postList.Add(await PostEntity.Create(post.ToString()));

			return postList;
		}

		/// <summary>
		/// 指定ユーザーのポストを複数取得します。
		/// </summary>
		/// <returns></returns>
		public async Task<List<PostEntity>> UserTimeLine(string userId, int? limit = null, HashSet<PostType> types = null, int? sinceCursor = null, int? maxCursor = null)
		{
			var body = new Dictionary<string, string>();

			if (string.IsNullOrEmpty(userId))
				throw new ArgumentNullException("userId");

			body.Add("since-cursor", userId);

			if (limit != null)
			{
				if (limit >= 1 && limit <= 100)
					body.Add("limit", limit.ToString());
				else
					throw new ArgumentOutOfRangeException("limit は1～100の範囲内である必要があります。");
			}

			if (types != null)
			{
				foreach (var type in types)
					if (type == PostType.Unknown)
						throw new ArgumentException("引数'types'の項目に PostType.Unknown は指定できません。");

				body.Add("types", string.Join(",", types));
			}

			if (sinceCursor != null)
				body.Add("since-cursor", sinceCursor.ToString());

			if (maxCursor != null)
				body.Add("max-cursor", maxCursor.ToString());

			var res = (await MisskeyRequest.POST(_Misskey.Session, "posts/user-timeline", body)).Content;

			var postList = new List<PostEntity>();

			foreach (var post in DynamicJson.Parse(res))
				postList.Add(await PostEntity.Create(post.ToString()));

			return postList;
		}
	}
}
