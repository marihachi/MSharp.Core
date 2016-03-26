using MSharp.Core.Utility;
using MSharp.Data;
using MSharp.Data.Entity;
using MSharp.Core.Data.Exceptions;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MSharp.API
{
	public class Album
	{
		public Album(Misskey misskey)
		{
			_Misskey = misskey;
		}
		private Misskey _Misskey { get; set; }

		public async Task<AlbumFileEntity> Upload(Data.File file)
		{
			var contents = new List<HttpContent>();

			// ファイルの種類を判断

			if (file is ImageFile)
			{
				using (var ms = new MemoryStream())
				{
					var imageFile = file as ImageFile;

					var image = imageFile.Data;
					var fileName = imageFile.FileName;

					image.Save(ms, image.RawFormat);

					var fileContent = new ByteArrayContent(ms.ToArray());

					string mediaType;
					if (image.RawFormat == ImageFormat.Jpeg)
						mediaType = "image/jpeg";
					else if (image.RawFormat == ImageFormat.Png)
						mediaType = "image/png";
					else if (image.RawFormat == ImageFormat.Bmp)
						mediaType = "image/bmp";
					else if (image.RawFormat == ImageFormat.Gif)
						mediaType = "image/gif";
					else
						mediaType = "image/jpeg";

					fileContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

					// 自分でContent-Dispositionヘッダーを組み立てる
					var bytes = Encoding.UTF8.GetBytes(fileName);
					fileName = "";
					foreach (var b in bytes)
						fileName += (char)b;
					fileContent.Headers.Add("Content-Disposition", $"form-data; filename=\"{fileName}\"; name=file");

					contents.Add(fileContent);
				}
			}
			else
			{
				throw new MSharpApiException("ファイルの種類が不明です。");
			}

			var folder = new StringContent("null");
			folder.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
			{
				Name = "folder"
			};
			contents.Add(folder);

			var res = await MisskeyRequest.POST(_Misskey.Session, "web/album/upload", contents);
			return new AlbumFileEntity(res.Content);
		}
	}
}
