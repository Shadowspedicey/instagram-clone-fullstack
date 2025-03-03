﻿using System.ComponentModel.DataAnnotations;

namespace InstagramClone.DTOs.Authentication
{
	public class EmailChangeDTO
	{
		[EmailAddress]
		public required string Email { get; set; }
		public required string Password { get; set; }
	}
}
