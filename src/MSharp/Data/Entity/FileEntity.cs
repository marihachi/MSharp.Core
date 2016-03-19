using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MSharp.Data.Entity
{
	public class FileEntity
	{
		public FileEntity(string jsonString)
		{
			try
			{
				var j = DynamicJson.Parse(jsonString);

				Id = j.id;
				Cursor = j.cursor() ? (int?)j.cursor : null;
				UserId = j.user() ? j.user : null;
				DataSize = j.dataSize() ? (int?)j.dataSize : null;
				MimeType = j.mimeType() ? j.mimeType : null;
				Name = j.name() ? j.name : null;
				if (j.tags() && j.tags != null)
				{
					Tags = new List<string>();
					foreach (var i in j.tags)
						Tags.Add(i);
				}
				if (j.properties())
				{
					if (j.properties.width() && j.properties.width != null && j.properties.height() && j.properties.height != null)
					{
						ImageSize = new Size((int)j.properties.width, (int)j.properties.height);
					}
				}
				IsPrivate = j.isPrivate() ? j.isPrivate : null;
				IsHidden = j.isHidden() ? j.isHidden : null;
				IsDeleted = j.isDeleted() ? j.isDeleted : null;
				Hash = j.hash() ? j.hash : null;
				FolderId = j.folder() ? j.folder : null;
				CreatedAt = DateTime.Parse(j.createdAt);
				ApplicationId = j.app() ? j.app : null;
				Url = j.url() && !string.IsNullOrEmpty(j.url) ? new Uri(j.url) : null;
				ThumbnailUrl = j.thumbnailUrl() && !string.IsNullOrEmpty(j.thumbnailUrl) ? new Uri(j.thumbnailUrl) : null;
			}
			catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public string Id { get; set; }
		public int? Cursor { get; set; }
		public string UserId { get; set; }
		public int? DataSize { get; set; }
		public string MimeType { get; set; }
		public string Name { get; set; }
		public List<string> Tags { get; set; }
		public Size? ImageSize { get; set; }
		public bool? IsPrivate { get; set; }
		public bool? IsHidden { get; set; }
		public bool? IsDeleted { get; set; }
		public string Hash { get; set; }
		public string FolderId { get; set; }
		public DateTime CreatedAt { get; set; }
		public object ApplicationId { get; set; }
		public Uri Url { get; set; }
		public Uri ThumbnailUrl { get; set; }
	}
}
