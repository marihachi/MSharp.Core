using Codeplex.Data;
using System;
using System.Diagnostics;

namespace MSharp.Data.Entity
{
	public class AlbumTagEntity
	{
		public AlbumTagEntity(string jsonString)
		{
			try
			{
				Debug.WriteLine("AlbumTag: " + jsonString);
				var j = DynamicJson.Parse(jsonString);

				Id = j.id;
				Color = j.color() ? j.color : null;
				Name = j.name() ? j.name : null;
				UserId = j.user() ? j.user : null;
			}
			catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		public string Id { get; set; }
		public string Color { get; set; }
		public string Name { get; set; }
		public string UserId { get; set; }
	}
}
