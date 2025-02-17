﻿using InstagramClone.DTOs.Posts;
using InstagramClone.DTOs.Profile;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace InstagramClone.Data.Entities
{
	public class Post : ICommentable
	{
		[Key]
		[StringLength(26)]
		public string ID { get; set; } = Ulid.NewUlid().ToString();
		[StringLength(1000)]
		public string? Caption { get; set; }
		public required string Photo { get; set; }
		public virtual required User User { get; set; }
		public virtual ICollection<Comment> Comments { get; set; } = [];
		public virtual ICollection<User> Likes { get; set; } = [];
		public required DateTime CreatedAt { get; set; }

		public void AddComment(Comment comment) => Comments.Add(comment);
		[IgnoreDataMember]
		public IOrderedEnumerable<Comment> SortedComments => Comments.OrderByDescending(x => x.CreatedAt);

		public PostViewDTO ToDTO(string fileDownloadEndpoint)
		{
			return new PostViewDTO
			{
				ID = ID,
				Photo = $"{fileDownloadEndpoint}{Photo}",
				Caption = Caption,
				Comments = SortedComments.Select(c => c.ToDTO(fileDownloadEndpoint)).ToList(),
				User = new UserMinimalProfileDTO(User.UserName!, User.ProfilePic!, fileDownloadEndpoint),
				CreatedAt = CreatedAt
			};
		}
		public PostMinimalViewDTO ToMinimalDTO(string fileDownloadEndpoint) => new()
		{
			ID = ID,
			Photo = $"{fileDownloadEndpoint}{Photo}",
			LikesCount = Likes.Count,
			CommentsCount = Comments.Count
		};
	}
}
