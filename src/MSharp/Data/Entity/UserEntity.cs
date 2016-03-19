using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MSharp.Data.Entity
{
	public class UserEntity
	{
		public UserEntity(string jsonString)
		{
			try
			{
				var j = DynamicJson.Parse(jsonString);

				Id = j.id;
				ScreenName = j.screenName() ? j.screenName : null;
				Name = j.name() ? j.name : null;
				Lang = j.lang() ? j.lang : null;
				Credit = j.credit() ? (int?)j.credit : null;
				if (j.url())
					if (Regex.IsMatch(j.url, "https?://(.+)"))
						Url = new Uri(j.url);
				TimelineReadCursor = j.timelineReadCursor() ? (int?)j.timelineReadCursor : null;
				if (j.tags())
				{
					Tags = new List<string>();
					foreach (var tag in j.tags)
						Tags.Add(tag);
				}
				PostsCount = j.postsCount() ? (int?)j.postsCount : null;
				PinnedPostId = j.pinnedPost() ? j.pinnedPost : null;
				Location = j.location() ? j.location : null;
				LikesCount = j.likesCount() ? (int?)j.likesCount : null;
				LikedCount = j.likedCount() ? (int?)j.likedCount : null;
				LatestPostId = j.latestPost() ? j.latestPost : null;
				IsVerified = j.isVerified() ? j.isVerified : null;
				IsSuspended = j.isSuspended() ? j.isSuspended : null;
				IsStaff = j.isStaff() ? j.isStaff : null;
				IsPro = j.isPro() ? j.isPro : null;
				IsPrivate = j.isPrivate() ? j.isPrivate : null;
				IsEmailVerified = j.isEmailVerified() ? j.isEmailVerified : null;
				IsDeleted = j.isDeleted() ? j.isDeleted : null;
				FollowingCount = j.followingCount() ? (int?)j.followingCount : null;
				FollowersCount = j.followersCount() ? (int?)j.followersCount : null;
				Description = j.description() ? j.description : null;
				CreatedAt = DateTime.Parse(j.createdAt);
				Comment = j.comment() ? j.comment : null;
				Color = j.color() ? j.color : null;
				Birthday = j.birthday() ? j.birthday : null;
				AvatarUrl = j.avatarUrl() && !string.IsNullOrEmpty(j.avatarUrl) ? new Uri(j.avatarUrl) : null;
				AvatarThumbnailUrl = j.avatarThumbnailUrl() && !string.IsNullOrEmpty(j.avatarThumbnailUrl) ? new Uri(j.avatarThumbnailUrl) : null;
				BannerUrl = j.bannerUrl() && !string.IsNullOrEmpty(j.bannerUrl) ? new Uri(j.bannerUrl) : null;
				BannerThumbnailUrl = j.bannerThumbnailUrl() && !string.IsNullOrEmpty(j.bannerThumbnailUrl) ? new Uri(j.bannerThumbnailUrl) : null;
				WallpaperUrl = j.wallpaperUrl() && !string.IsNullOrEmpty(j.wallpaperUrl) ? new Uri(j.wallpaperUrl) : null;
				WallpaperThumbnailUrl = j.wallpaperThumbnailUrl() && !string.IsNullOrEmpty(j.wallpaperThumbnailUrl) ? new Uri(j.wallpaperThumbnailUrl) : null;
			}
			catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public string Id { get; set; }
		public string ScreenName { get; set; }
		public string Name { get; set; }
		public string Lang { get; set; }
		public int? Credit { get; set; }
		public Uri Url { get; set; }
		public int? TimelineReadCursor { get; set; }
		public List<string> Tags { get; set; }
		public int? PostsCount { get; set; }
		public string PinnedPostId { get; set; }
		public string Location { get; set; }
		public int? LikesCount { get; set; }
		public int? LikedCount { get; set; }
		public string LatestPostId { get; set; }
		public bool? IsVerified { get; set; }
		public bool? IsSuspended { get; set; }
		public bool? IsStaff { get; set; }
		public bool? IsPro { get; set; }
		public bool? IsPrivate { get; set; }
		public bool? IsEmailVerified { get; set; }
		public bool? IsDeleted { get; set; }
		public int? FollowingCount { get; set; }
		public int? FollowersCount { get; set; }
		public string Description { get; set; }
		public DateTime CreatedAt { get; set; }
		public string Comment { get; set; }
		public string Color { get; set; }
		public string Birthday { get; set; }
		public Uri AvatarUrl { get; set; }
		public Uri AvatarThumbnailUrl { get; set; }
		public Uri BannerUrl { get; set; }
		public Uri BannerThumbnailUrl { get; set; }
		public Uri WallpaperUrl { get; set; }
		public Uri WallpaperThumbnailUrl { get; set; }
	}
}
