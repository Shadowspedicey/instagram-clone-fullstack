﻿using InstagramClone.Data;
using InstagramClone.Data.Entities;
using InstagramClone.DTOs.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace InstagramClone.Tests.IntegrationTests
{
	public class AuthTests : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly WebApplicationFactory<Program> _fixture;
		public AuthTests(WebApplicationFactory<Program> fixture)
		{
			_fixture = fixture;
		}

		[Fact]
		public async Task RegisterUser_ValidData_ShouldCreateUser()
		{
			var client = _fixture.CreateClient();
			var newUser = new UserRegisterDTO
			{
				Email = "example1@domain.com",
				Username = "exampleusername1",
				Password = "password",
				RealName = "Example Name"
			};

			var content = new StringContent(JsonSerializer.Serialize(newUser), Encoding.UTF8, "application/json");
			await client.PostAsync("auth/register", content);

			using var scope = _fixture.Services.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
			bool userExists = dbContext.Users.Any(u => u.Email == "example@domain.com");
		}

		[Fact]
		public async Task Login_EmailNotVerified_ShouldReturn401WithEmailNotVerifiedError()
		{
			var client = _fixture.CreateClient();
			using var scope = _fixture.Services.CreateScope();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
			var newUser = new User
			{
				Email = "example2@domain.com",
				EmailConfirmed = false,
				UserName = "exampleusername2",
				RealName = "Example Name",
				IsVerified = false,
				Followers = [],
				Following = [],
				RecentSearches = [],
				CreatedAt = DateTime.UtcNow,
				LastLogin = DateTime.UtcNow
			};
			await userManager.CreateAsync(newUser, "password");

			var loginInfo = new
			{
				newUser.Email,
				Password = "password"
			};
			var response = await client.PostAsJsonAsync("auth/login", loginInfo);

			var responseCode = response.StatusCode;
			Assert.Equal(HttpStatusCode.Unauthorized, responseCode);
		}

		[Fact]
		public async Task Login_EmailVerified_ShouldReturn200WithTokenInResponse()
		{
			var client = _fixture.CreateClient();
			using var scope = _fixture.Services.CreateScope();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
			var newUser = new User
			{
				Email = "example3@domain.com",
				EmailConfirmed = false,
				UserName = "exampleusername3",
				RealName = "Example Name",
				IsVerified = false,
				Followers = [],
				Following = [],
				RecentSearches = [],
				CreatedAt = DateTime.UtcNow,
				LastLogin = DateTime.UtcNow
			};
			await userManager.CreateAsync(newUser, "password");
			var token = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
			await userManager.ConfirmEmailAsync(newUser, token);

			var loginInfo = new
			{
				newUser.Email,
				Password = "password"
			};
			var response = await client.PostAsJsonAsync("auth/login", loginInfo);

			var body = await response.Content.ReadAsStreamAsync();
			var bodyJson = JsonSerializer.Deserialize<JsonNode>(body);
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.NotNull(bodyJson?["token"]?.GetValue<string>());
		}

		[Fact]
		public async Task Login_InvalidCredentials_ShouldReturn400()
		{
			var client = _fixture.CreateClient();
			using var scope = _fixture.Services.CreateScope();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
			var newUser = new User
			{
				Email = "example3@domain.com",
				EmailConfirmed = false,
				UserName = "exampleusername3",
				RealName = "Example Name",
				IsVerified = false,
				Followers = [],
				Following = [],
				RecentSearches = [],
				CreatedAt = DateTime.UtcNow,
				LastLogin = DateTime.UtcNow
			};
			await userManager.CreateAsync(newUser, "password");
			var token = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
			await userManager.ConfirmEmailAsync(newUser, token);

			var loginInfo = new
			{
				newUser.Email,
				Password = "wrong-password"
			};
			var response = await client.PostAsJsonAsync("auth/login", loginInfo);

			var body = await response.Content.ReadAsStreamAsync();
			var bodyJson = JsonSerializer.Deserialize<JsonNode>(body);
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
			Assert.Equal(bodyJson?["errors"][0]["code"]?.GetValue<string>(), "InvalidCredentials");
		}

		[Fact]
		public async Task ConfirmEmail_ShouldReturn204_WhenValidToken()
		{
			var client = _fixture.CreateClient();
			using var scope = _fixture.Services.CreateScope();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
			var newUser = new User
			{
				Email = "example4@domain.com",
				EmailConfirmed = false,
				UserName = "exampleusername4",
				RealName = "Example Name",
				IsVerified = false,
				Followers = [],
				Following = [],
				RecentSearches = [],
				CreatedAt = DateTime.UtcNow,
				LastLogin = DateTime.UtcNow
			};
			await userManager.CreateAsync(newUser, "password");
			var user = await userManager.FindByEmailAsync(newUser.Email);
			var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

			var encodedEmail = WebUtility.UrlEncode(user.Email);
			var encodedToken = WebUtility.UrlEncode(token);
			var response = await client.PostAsync($"auth/confirm-email?encodedEmail={encodedEmail}&code={encodedToken}", null);


			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public async Task ConfirmEmail_ShouldReturn400_WhenInvalidToken()
		{
			var client = _fixture.CreateClient();
			using var scope = _fixture.Services.CreateScope();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
			var newUser = new User
			{
				Email = "example4@domain.com",
				EmailConfirmed = false,
				UserName = "exampleusername4",
				RealName = "Example Name",
				IsVerified = false,
				Followers = [],
				Following = [],
				RecentSearches = [],
				CreatedAt = DateTime.UtcNow,
				LastLogin = DateTime.UtcNow
			};
			await userManager.CreateAsync(newUser, "password");

			var encodedEmail = WebUtility.UrlEncode(newUser.Email);
			var response = await client.PostAsync($"auth/confirm-email?encodedEmail={encodedEmail}&code=InvalidToken", null);


			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}
	}
	
	public class AuthTestsWebApplicationFactory : WebApplicationFactory<Program>
	{
		private SqliteConnection _connection;
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			_connection = new SqliteConnection("DataSource=:memory:");
			_connection.Open();

			builder.ConfigureTestServices(services =>
			{
				services.RemoveAll<DbContextOptions<AppDbContext>>();
				services.AddDbContext<AppDbContext>(options => options.UseSqlite(_connection));
			});
		}

		protected override IHost CreateHost(IHostBuilder builder)
		{
			var host = base.CreateHost(builder);

			using var scope = host.Services.CreateScope();
			var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
			db.Database.EnsureCreated();

			return host;
		}
	}
}
