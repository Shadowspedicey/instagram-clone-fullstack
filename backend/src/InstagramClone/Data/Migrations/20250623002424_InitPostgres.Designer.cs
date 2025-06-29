﻿// <auto-generated />
using System;
using InstagramClone.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InstagramClone.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250623002424_InitPostgres")]
    partial class InitPostgres
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ChatRoomUser", b =>
                {
                    b.Property<string>("ChatRoomID")
                        .HasColumnType("text");

                    b.Property<string>("UsersId")
                        .HasColumnType("text")
                        .HasColumnName("UserID");

                    b.HasKey("ChatRoomID", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("ChatRoomUsers", (string)null);
                });

            modelBuilder.Entity("CommentUser", b =>
                {
                    b.Property<string>("CommentID")
                        .HasColumnType("character varying(26)");

                    b.Property<string>("LikesId")
                        .HasColumnType("text")
                        .HasColumnName("UserID");

                    b.HasKey("CommentID", "LikesId");

                    b.HasIndex("LikesId");

                    b.ToTable("CommentsLikes", (string)null);
                });

            modelBuilder.Entity("InstagramClone.Data.Entities.ChatRoom", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("ChatRooms");
                });

            modelBuilder.Entity("InstagramClone.Data.Entities.Comment", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(26)
                        .HasColumnType("character varying(26)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PostID")
                        .IsRequired()
                        .HasColumnType("character varying(26)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("PostID");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("InstagramClone.Data.Entities.Message", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("text");

                    b.Property<string>("ChatRoomID")
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("ChatRoomID");

                    b.HasIndex("UserId");

                    b.ToTable("Messages", (string)null);
                });

            modelBuilder.Entity("InstagramClone.Data.Entities.Post", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(26)
                        .HasColumnType("character varying(26)");

                    b.Property<string>("Caption")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Photo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("InstagramClone.Data.Entities.RefreshToken", b =>
                {
                    b.Property<string>("UserID")
                        .HasColumnType("text");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)");

                    b.HasKey("UserID");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("InstagramClone.Data.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("Bio")
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsVerified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("ProfilePic")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("jGLe0Lx39svW1MiJObFkD6w1S-gigSJuJ0WfcA0BpE4");

                    b.Property<string>("RealName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("InstagramClone.Data.Entities.UserSearch", b =>
                {
                    b.Property<string>("UserID")
                        .HasColumnType("text");

                    b.Property<string>("SearchedUserID")
                        .HasColumnType("text");

                    b.Property<DateTime>("SearchedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("UserID", "SearchedUserID");

                    b.HasIndex("SearchedUserID");

                    b.ToTable("UserSearches");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("PostUser", b =>
                {
                    b.Property<string>("LikedPostsID")
                        .HasColumnType("character varying(26)")
                        .HasColumnName("LikedPostID");

                    b.Property<string>("LikesId")
                        .HasColumnType("text")
                        .HasColumnName("UserID");

                    b.HasKey("LikedPostsID", "LikesId");

                    b.HasIndex("LikesId");

                    b.ToTable("PostsLikes", (string)null);
                });

            modelBuilder.Entity("PostUser1", b =>
                {
                    b.Property<string>("SavedPostsID")
                        .HasColumnType("character varying(26)")
                        .HasColumnName("SavedPostID");

                    b.Property<string>("User1Id")
                        .HasColumnType("text")
                        .HasColumnName("UserID");

                    b.Property<DateTime>("SavedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("SavedPostsID", "User1Id");

                    b.HasIndex("User1Id");

                    b.ToTable("PostsSaves", (string)null);
                });

            modelBuilder.Entity("UserUser", b =>
                {
                    b.Property<string>("FollowersId")
                        .HasColumnType("text")
                        .HasColumnName("UserID");

                    b.Property<string>("FollowingId")
                        .HasColumnType("text")
                        .HasColumnName("FollowedUserID");

                    b.HasKey("FollowersId", "FollowingId");

                    b.HasIndex("FollowingId");

                    b.ToTable("UserFollowing", (string)null);
                });

            modelBuilder.Entity("ChatRoomUser", b =>
                {
                    b.HasOne("InstagramClone.Data.Entities.ChatRoom", null)
                        .WithMany()
                        .HasForeignKey("ChatRoomID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InstagramClone.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CommentUser", b =>
                {
                    b.HasOne("InstagramClone.Data.Entities.Comment", null)
                        .WithMany()
                        .HasForeignKey("CommentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InstagramClone.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("LikesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("InstagramClone.Data.Entities.Comment", b =>
                {
                    b.HasOne("InstagramClone.Data.Entities.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostID")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("InstagramClone.Data.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("InstagramClone.Data.Entities.Message", b =>
                {
                    b.HasOne("InstagramClone.Data.Entities.ChatRoom", null)
                        .WithMany("Messages")
                        .HasForeignKey("ChatRoomID");

                    b.HasOne("InstagramClone.Data.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("InstagramClone.Data.Entities.Post", b =>
                {
                    b.HasOne("InstagramClone.Data.Entities.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("InstagramClone.Data.Entities.RefreshToken", b =>
                {
                    b.HasOne("InstagramClone.Data.Entities.User", "User")
                        .WithOne()
                        .HasForeignKey("InstagramClone.Data.Entities.RefreshToken", "UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("InstagramClone.Data.Entities.UserSearch", b =>
                {
                    b.HasOne("InstagramClone.Data.Entities.User", "SearchedUser")
                        .WithMany()
                        .HasForeignKey("SearchedUserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InstagramClone.Data.Entities.User", "User")
                        .WithMany("RecentSearches")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("SearchedUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("InstagramClone.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("InstagramClone.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InstagramClone.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("InstagramClone.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PostUser", b =>
                {
                    b.HasOne("InstagramClone.Data.Entities.Post", null)
                        .WithMany()
                        .HasForeignKey("LikedPostsID")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("InstagramClone.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("LikesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PostUser1", b =>
                {
                    b.HasOne("InstagramClone.Data.Entities.Post", null)
                        .WithMany()
                        .HasForeignKey("SavedPostsID")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("InstagramClone.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("User1Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UserUser", b =>
                {
                    b.HasOne("InstagramClone.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("FollowersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InstagramClone.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("InstagramClone.Data.Entities.ChatRoom", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("InstagramClone.Data.Entities.Post", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("InstagramClone.Data.Entities.User", b =>
                {
                    b.Navigation("Posts");

                    b.Navigation("RecentSearches");
                });
#pragma warning restore 612, 618
        }
    }
}
