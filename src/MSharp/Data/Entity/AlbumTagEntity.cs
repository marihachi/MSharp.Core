using Codeplex.Data;
using MSharp.Core;
using MSharp.Core.Data.Exceptions;
using System;

namespace MSharp.Data.Entity
{
	public class AlbumTagEntity
	{
		public AlbumTagEntity(string jsonString)
		{
			try
			{
				var j = DynamicJson.Parse(jsonString);

				Id = j.id;
				Color = j.color() ? j.color : null;
				Name = j.name() ? j.name : null;
				UserId = j.user() ? j.user : null;
			}
			catch (Exception ex)
			{
				throw new MSharpEntityException("JSONのパースに失敗しました。", ex);
			}
		}

		public string Id { get; set; }
		public string Color { get; set; }
		public string Name { get; set; }
		public string UserId { get; set; }
	}
}
