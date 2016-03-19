using MSharp.Core.Utility;
using MSharp.Data;
using MSharp.Data.Entity;
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
			if(string.IsNullOrEmpty(postId))
				throw new ArgumentNullException("postId");

			var body = new Dictionary<string, string>();

			body.Add("post-id", postId);

			var res = (await MisskeyRequest.POST(_Misskey.Session, "posts/show", body)).Content;

			if (res == "not-found")
				throw new ArgumentException("指定されたpostIdのポストは見つかりませんでした。");

			return new PostEntity(res);
		}

		/// <summary>
		/// ポストを投稿します。
		/// </summary>
		/// <param name="text"></param>
		/// <param name="fileIds"></param>
		public async Task<PostEntity> Create(string text, List<string> fileIds)
		{
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
				throw new Exception("投稿内容が重複しています。");

			return new PostEntity(res);
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
		public async Task<PostEntity> Create(string text, List<IFile> files)
		{
			var fileIds = new List<string>();

			if (files != null)
			{
				files = files.FindAll(i => i != null);

				foreach(var i in files)
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
		public async Task<PostEntity> Create(string text, IFile file = null)
		{
			return await Create(text, new List<IFile> { file });
		}
	}
}
