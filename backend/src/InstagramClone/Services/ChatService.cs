﻿using FluentResults;
using InstagramClone.Data;
using InstagramClone.Data.Entities;
using InstagramClone.Interfaces;
using Microsoft.AspNetCore.Authorization;
using InstagramClone.Utils;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using InstagramClone.Hubs;

namespace InstagramClone.Services
{
	public class ChatService(AppDbContext dbContext, IAuthorizationService authorizationService, IHubContext<ChatHub> chatHub) : IChatService
	{
		private readonly AppDbContext _dbContext = dbContext;
		private readonly IAuthorizationService _authorizationService = authorizationService;
		private readonly IHubContext<ChatHub> _chatHub = chatHub;

		public async Task<Result<ChatRoom>> GetOrCreateChatRoom(ClaimsPrincipal currentUserPrinciple, string[] usernames)
		{
			User currentUser = await _dbContext.Users.FirstAsync(u => u.Id == currentUserPrinciple.FindFirstValue("sub"));
			if (usernames.Length == 1)
			{
				var searchResult = await GetRoom(currentUserPrinciple, username: usernames[0]);
				if (searchResult.IsSuccess) return Result.Ok(searchResult.Value);
				// If error code is anything except NotFound, return the result
				else if (searchResult.Errors.FirstOrDefault()?.Metadata["code"].ToString() != Enum.GetName(ErrorCode.NotFound))
					return searchResult;
			}
			List<User> users = await _dbContext.Users.Where(u => usernames.Contains(u.UserName)).ToListAsync();
			if (users.Count != usernames.Length)
				return Result.Fail(new CodedError(ErrorCode.InvalidInput, $"The user/s: {string.Join(", ", usernames.Where(username => users.Any(u => u.UserName != username)))} were not found."));

			ChatRoom chatRoom = new() { Users = [currentUser, .. users] };
			await _dbContext.ChatRooms.AddAsync(chatRoom);
			await _dbContext.SaveChangesAsync();
			return Result.Ok(chatRoom);
		}

		public async Task<Result<ChatRoom>> GetRoom(ClaimsPrincipal currentUserPrinciple, string? username = null, string? roomID = null)
		{
			User currentUser = await _dbContext.Users.FirstAsync(u => u.Id == currentUserPrinciple.FindFirstValue("sub"));
			ChatRoom? chatRoom = default;
			if (roomID is not null)
				chatRoom = await _dbContext.ChatRooms.FindAsync(roomID);
			else
			{
				User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
				if (user is null)
					return Result.Fail(new CodedError(ErrorCode.InvalidInput, $"The user: {username} was not found."));

				chatRoom = await _dbContext.ChatRooms.FirstOrDefaultAsync(cr => cr.Users.Contains(currentUser) && cr.Users.Contains(user));
			}

			if (chatRoom is null)
				return Result.Fail(new CodedError(ErrorCode.NotFound, "Chat room between the two users doesn't exist."));

			chatRoom.Users = [.. chatRoom.Users.Where(u => u.UserName != currentUser.UserName)];
			return Result.Ok(chatRoom);
		}

		public async Task<Result<ICollection<ChatRoom>>> GetUserRooms(ClaimsPrincipal currentUserPrinciple)
		{
			User currentUser = await _dbContext.Users.FirstAsync(u => u.Id == currentUserPrinciple.FindFirstValue("sub"));
			var chatRooms = (await _dbContext.ChatRooms
				.Where(cr => cr.Users.Contains(currentUser))
				.Where(cr => cr.Messages.Count != 0)
				.ToListAsync())
				.OrderByDescending(cr => cr.LastUpdated)
				.ToList();
			chatRooms.ForEach(cr => cr.Users = [.. cr.Users.Where(u => u.UserName != currentUser.UserName)]);
			return Result.Ok((ICollection<ChatRoom>)chatRooms);
		}

		public async Task<Result<ICollection<Message>>> GetMessages(ClaimsPrincipal currentUserPrinciple, string roomID)
		{
			User currentUser = await _dbContext.Users.FirstAsync(u => u.Id == currentUserPrinciple.FindFirstValue("sub"));
			ChatRoom? chatRoom = await _dbContext.ChatRooms.FirstOrDefaultAsync(cr => cr.ID == roomID);
			if (chatRoom is null)
				return Result.Fail(new CodedError(ErrorCode.NotFound, "Chat room doesn't exist."));

			var authorizationResult = await _authorizationService.AuthorizeAsync(currentUserPrinciple, chatRoom, "CanAccessRoomMessages");
			if (!authorizationResult.Succeeded)
				return Result.Fail(new CodedError(ErrorCode.InsufficientPermissions, "User isn't part of chat room."));

			ICollection<Message> messages = chatRoom.SortedMessages.ToList();
			return Result.Ok(messages);
		}

		public async Task<Result<Message>> SendMessage(ClaimsPrincipal currentUserPrinciple, string roomID, string message, string fileDownloadEndpoint = "")
		{
			User currentUser = await _dbContext.Users.FirstAsync(u => u.Id == currentUserPrinciple.FindFirstValue("sub"));
			ChatRoom? chatRoom = await _dbContext.ChatRooms.FirstOrDefaultAsync(cr => cr.ID == roomID);
			if (chatRoom is null)
				return Result.Fail(new CodedError(ErrorCode.NotFound, "Chat room doesn't exist."));

			var authorizationResult = await _authorizationService.AuthorizeAsync(currentUserPrinciple, chatRoom, "CanAccessRoomMessages");
			if (!authorizationResult.Succeeded)
				return Result.Fail(new CodedError(ErrorCode.InsufficientPermissions, "User isn't part of chat room."));

			Message msg = new() { Content = message, User = currentUser };
			chatRoom.Messages.Add(msg);
			await _dbContext.SaveChangesAsync();
			await _chatHub.Clients.Group(roomID).SendAsync("ReceiveMessage", msg.ToViewDTO(fileDownloadEndpoint));
			await _chatHub.Clients.Users(chatRoom.Users.Select(u => u.Id)).SendAsync("UpdateRoom", chatRoom.ToViewDTO(fileDownloadEndpoint));
			return Result.Ok(msg);
		}
	}
}
