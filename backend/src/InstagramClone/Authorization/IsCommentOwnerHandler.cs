﻿using InstagramClone.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace InstagramClone.Authorization
{
	public class IsCommentOwnerHandler : AuthorizationHandler<IsCommentOrPostOwnerRequirement, Comment>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsCommentOrPostOwnerRequirement requirement, Comment resource)
		{
			string? userID = context.User.FindFirstValue("sub");
			if (userID is null) context.Fail();
			if (resource.User.Id == userID)
				context.Succeed(requirement);

			return Task.CompletedTask;
		}
	}
}
