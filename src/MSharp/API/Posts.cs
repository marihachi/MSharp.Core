using MSharp.Core.Utility;
using MSharp.Data;
using MSharp.Data.Entity;
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
		/// ポストを投稿します。
		/// </summary>
		/// <param name="text"></param>
		public async Task<PostEntity> Create(string text)
		{
			var body = new Dictionary<string, string>();
			body.Add("text", text);

			var res = await MisskeyRequest.POST(_Misskey.Session, "posts/create", body);
			return new PostEntity(res.Content);
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
			body.Add("files", string.Join(",", fileIds));

			var res = await MisskeyRequest.POST(_Misskey.Session, "posts/create", body);
			return new PostEntity(res.Content);
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

			foreach (var file in files)
			{
				var res = await _Misskey.Album.Upload(file);
				fileIds.Add(res.Id);
			}

			return await Create(text, fileIds);
		}

		/// <summary>
		/// ポストを投稿します。
		/// </summary>
		/// <param name="text"></param>
		/// <param name="imageFile"></param>
		public async Task<PostEntity> Create(string text, IFile file)
		{
			var res = await _Misskey.Album.Upload(file);
			return await Create(text, res.Id);
		}
	}
}
