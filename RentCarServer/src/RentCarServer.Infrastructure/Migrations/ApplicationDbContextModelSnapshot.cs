﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RentCarServer.Infrastructure.Context;

#nullable disable

namespace RentCarServer.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RentCarServer.Domain.Branches.Branch", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Branches", (string)null);
                });

            modelBuilder.Entity("RentCarServer.Domain.LoginTokens.LoginToken", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("LoginTokens", (string)null);
                });

            modelBuilder.Entity("RentCarServer.Domain.Roles.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("RentCarServer.Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("RentCarServer.Domain.Branches.Branch", b =>
                {
                    b.OwnsOne("RentCarServer.Domain.Branchs.ValueObjects.Name", "Name", b1 =>
                        {
                            b1.Property<Guid>("BranchId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("varchar(MAX)");

                            b1.HasKey("BranchId");

                            b1.ToTable("Branches");

                            b1.WithOwner()
                                .HasForeignKey("BranchId");
                        });

                    b.OwnsOne("RentCarServer.Domain.Branchs.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("BranchId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("varchar(MAX)");

                            b1.Property<string>("District")
                                .IsRequired()
                                .HasColumnType("varchar(MAX)");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasColumnType("varchar(MAX)");

                            b1.Property<string>("FullAddress")
                                .IsRequired()
                                .HasColumnType("varchar(MAX)");

                            b1.Property<string>("PhoneNumber1")
                                .IsRequired()
                                .HasColumnType("varchar(MAX)");

                            b1.Property<string>("PhoneNumber2")
                                .IsRequired()
                                .HasColumnType("varchar(MAX)");

                            b1.HasKey("BranchId");

                            b1.ToTable("Branches");

                            b1.WithOwner()
                                .HasForeignKey("BranchId");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("RentCarServer.Domain.LoginTokens.LoginToken", b =>
                {
                    b.OwnsOne("RentCarServer.Domain.LoginTokens.ValueObjects.ExpiresDate", "ExpiresDate", b1 =>
                        {
                            b1.Property<Guid>("LoginTokenId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTimeOffset>("Value")
                                .HasColumnType("datetimeoffset");

                            b1.HasKey("LoginTokenId");

                            b1.ToTable("LoginTokens");

                            b1.WithOwner()
                                .HasForeignKey("LoginTokenId");
                        });

                    b.OwnsOne("RentCarServer.Domain.LoginTokens.ValueObjects.IsActive", "IsActive", b1 =>
                        {
                            b1.Property<Guid>("LoginTokenId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool>("Value")
                                .HasColumnType("bit");

                            b1.HasKey("LoginTokenId");

                            b1.ToTable("LoginTokens");

                            b1.WithOwner()
                                .HasForeignKey("LoginTokenId");
                        });

                    b.OwnsOne("RentCarServer.Domain.LoginTokens.ValueObjects.Token", "Token", b1 =>
                        {
                            b1.Property<Guid>("LoginTokenId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("varchar(MAX)");

                            b1.HasKey("LoginTokenId");

                            b1.ToTable("LoginTokens");

                            b1.WithOwner()
                                .HasForeignKey("LoginTokenId");
                        });

                    b.Navigation("ExpiresDate")
                        .IsRequired();

                    b.Navigation("IsActive")
                        .IsRequired();

                    b.Navigation("Token")
                        .IsRequired();
                });

            modelBuilder.Entity("RentCarServer.Domain.Roles.Role", b =>
                {
                    b.OwnsMany("RentCarServer.Domain.Roles.Permission", "Permissions", b1 =>
                        {
                            b1.Property<Guid>("RoleId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("varchar(MAX)");

                            b1.HasKey("RoleId", "Id");

                            b1.ToTable("Permission");

                            b1.WithOwner()
                                .HasForeignKey("RoleId");
                        });

                    b.OwnsOne("RentCarServer.Domain.Branchs.ValueObjects.Name", "Name", b1 =>
                        {
                            b1.Property<Guid>("RoleId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("varchar(MAX)");

                            b1.HasKey("RoleId");

                            b1.ToTable("Roles");

                            b1.WithOwner()
                                .HasForeignKey("RoleId");
                        });

                    b.Navigation("Name")
                        .IsRequired();

                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("RentCarServer.Domain.Users.User", b =>
                {
                    b.OwnsOne("RentCarServer.Domain.Users.TFACode", "TFACode", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("varchar(MAX)");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("RentCarServer.Domain.Users.TFAConfirmCode", "TFAConfirmCode", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("varchar(MAX)");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("RentCarServer.Domain.Users.TFAExpiresDate", "TFAExpiresDate", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTimeOffset>("Value")
                                .HasColumnType("datetimeoffset");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("RentCarServer.Domain.Users.TFAIsCompleted", "TFAIsCompleted", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool>("Value")
                                .HasColumnType("bit");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("RentCarServer.Domain.Users.TFAStatus", "TFAStatus", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool>("Value")
                                .HasColumnType("bit");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("RentCarServer.Domain.Users.ValueObjects.Email", "Email", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("varchar(MAX)");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("RentCarServer.Domain.Users.ValueObjects.FirstName", "FirstName", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("varchar(MAX)");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("RentCarServer.Domain.Users.ValueObjects.ForgotPasswordCode", "ForgotPasswordCode", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("RentCarServer.Domain.Users.ValueObjects.ForgotPasswordDate", "ForgotPasswordDate", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTimeOffset>("Value")
                                .HasColumnType("datetimeoffset");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("RentCarServer.Domain.Users.ValueObjects.FullName", "FullName", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("varchar(MAX)");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("RentCarServer.Domain.Users.ValueObjects.IsForgotPasswordCompleted", "IsForgotPasswordCompleted", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool>("Value")
                                .HasColumnType("bit");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("RentCarServer.Domain.Users.ValueObjects.LastName", "LastName", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("varchar(MAX)");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("RentCarServer.Domain.Users.ValueObjects.Password", "Password", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<byte[]>("PasswordHash")
                                .IsRequired()
                                .HasColumnType("varbinary(max)");

                            b1.Property<byte[]>("PasswordSalt")
                                .IsRequired()
                                .HasColumnType("varbinary(max)");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("RentCarServer.Domain.Users.ValueObjects.UserName", "UserName", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("varchar(MAX)");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Email")
                        .IsRequired();

                    b.Navigation("FirstName")
                        .IsRequired();

                    b.Navigation("ForgotPasswordCode");

                    b.Navigation("ForgotPasswordDate");

                    b.Navigation("FullName")
                        .IsRequired();

                    b.Navigation("IsForgotPasswordCompleted")
                        .IsRequired();

                    b.Navigation("LastName")
                        .IsRequired();

                    b.Navigation("Password")
                        .IsRequired();

                    b.Navigation("TFACode");

                    b.Navigation("TFAConfirmCode");

                    b.Navigation("TFAExpiresDate");

                    b.Navigation("TFAIsCompleted");

                    b.Navigation("TFAStatus")
                        .IsRequired();

                    b.Navigation("UserName")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
