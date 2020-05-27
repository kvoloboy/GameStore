﻿// <auto-generated />
using System;
using GameStore.DataAccess.Sql.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GameStore.DataAccess.Sql.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200407203021_AddIdentitySeeding")]
    partial class AddIdentitySeeding
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GameStore.Core.Models.Comment", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GameRootId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("QuoteText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("GameRootId");

                    b.HasIndex("ParentId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("GameStore.Core.Models.GameDetails", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Discount")
                        .HasColumnType("decimal");

                    b.Property<string>("GameRootId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsDiscontinued")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("money");

                    b.Property<string>("QuantityPerUnit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<short?>("UnitsInStock")
                        .HasColumnType("smallint");

                    b.Property<int>("UnitsOnOrder")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameRootId")
                        .IsUnique();

                    b.ToTable("GameDetails");
                });

            modelBuilder.Entity("GameStore.Core.Models.GameGenre", b =>
                {
                    b.Property<string>("GameRootId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("GenreId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("GameRootId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("GameGenre");
                });

            modelBuilder.Entity("GameStore.Core.Models.GamePlatform", b =>
                {
                    b.Property<string>("GameRootId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PlatformId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("GameRootId", "PlatformId");

                    b.HasIndex("PlatformId");

                    b.ToTable("GamePlatform");
                });

            modelBuilder.Entity("GameStore.Core.Models.GameRoot", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PublisherEntityId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique();

                    b.ToTable("GameRoots");
                });

            modelBuilder.Entity("GameStore.Core.Models.Genre", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("GameStore.Core.Models.Identity.Permission", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Value")
                        .IsUnique();

                    b.ToTable("Permissions");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Value = "Read deleted games"
                        },
                        new
                        {
                            Id = "2",
                            Value = "Create game"
                        },
                        new
                        {
                            Id = "3",
                            Value = "Update game"
                        },
                        new
                        {
                            Id = "4",
                            Value = "Delete game"
                        },
                        new
                        {
                            Id = "5",
                            Value = "Create genre"
                        },
                        new
                        {
                            Id = "6",
                            Value = "Update genre"
                        },
                        new
                        {
                            Id = "7",
                            Value = "Delete genre"
                        },
                        new
                        {
                            Id = "8",
                            Value = "Create platform"
                        },
                        new
                        {
                            Id = "9",
                            Value = "Update platform"
                        },
                        new
                        {
                            Id = "10",
                            Value = "Delete platform"
                        },
                        new
                        {
                            Id = "11",
                            Value = "Create publisher"
                        },
                        new
                        {
                            Id = "12",
                            Value = "Update publisher"
                        },
                        new
                        {
                            Id = "13",
                            Value = "Delete publisher"
                        },
                        new
                        {
                            Id = "14",
                            Value = "Create comment"
                        },
                        new
                        {
                            Id = "15",
                            Value = "Delete comment"
                        },
                        new
                        {
                            Id = "16",
                            Value = "Ban user"
                        },
                        new
                        {
                            Id = "17",
                            Value = "Read orders"
                        },
                        new
                        {
                            Id = "18",
                            Value = "Update order"
                        },
                        new
                        {
                            Id = "19",
                            Value = "Make order"
                        },
                        new
                        {
                            Id = "20",
                            Value = "Create role"
                        },
                        new
                        {
                            Id = "21",
                            Value = "Update role"
                        },
                        new
                        {
                            Id = "22",
                            Value = "Delete role"
                        },
                        new
                        {
                            Id = "23",
                            Value = "Setup roles"
                        },
                        new
                        {
                            Id = "24",
                            Value = "Read users"
                        },
                        new
                        {
                            Id = "25",
                            Value = "Read roles"
                        },
                        new
                        {
                            Id = "26",
                            Value = "Read personal orders"
                        });
                });

            modelBuilder.Entity("GameStore.Core.Models.Identity.Role", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Name = "Admin"
                        },
                        new
                        {
                            Id = "2",
                            Name = "Manager"
                        },
                        new
                        {
                            Id = "3",
                            Name = "Moderator"
                        },
                        new
                        {
                            Id = "4",
                            Name = "User"
                        },
                        new
                        {
                            Id = "5",
                            Name = "Publisher"
                        },
                        new
                        {
                            Id = "6",
                            Name = "Guest"
                        });
                });

            modelBuilder.Entity("GameStore.Core.Models.Identity.RolePermission", b =>
                {
                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PermissionId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("RolePermission");

                    b.HasData(
                        new
                        {
                            RoleId = "1",
                            PermissionId = "1"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "2"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "3"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "4"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "5"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "6"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "7"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "8"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "9"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "10"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "11"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "12"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "13"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "14"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "15"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "16"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "17"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "18"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "19"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "20"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "21"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "22"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "23"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "24"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "25"
                        },
                        new
                        {
                            RoleId = "1",
                            PermissionId = "26"
                        },
                        new
                        {
                            RoleId = "2",
                            PermissionId = "1"
                        },
                        new
                        {
                            RoleId = "2",
                            PermissionId = "2"
                        },
                        new
                        {
                            RoleId = "2",
                            PermissionId = "3"
                        },
                        new
                        {
                            RoleId = "2",
                            PermissionId = "4"
                        },
                        new
                        {
                            RoleId = "2",
                            PermissionId = "5"
                        },
                        new
                        {
                            RoleId = "2",
                            PermissionId = "6"
                        },
                        new
                        {
                            RoleId = "2",
                            PermissionId = "7"
                        },
                        new
                        {
                            RoleId = "2",
                            PermissionId = "8"
                        },
                        new
                        {
                            RoleId = "2",
                            PermissionId = "9"
                        },
                        new
                        {
                            RoleId = "2",
                            PermissionId = "10"
                        },
                        new
                        {
                            RoleId = "2",
                            PermissionId = "11"
                        },
                        new
                        {
                            RoleId = "2",
                            PermissionId = "12"
                        },
                        new
                        {
                            RoleId = "2",
                            PermissionId = "13"
                        },
                        new
                        {
                            RoleId = "2",
                            PermissionId = "19"
                        },
                        new
                        {
                            RoleId = "2",
                            PermissionId = "26"
                        },
                        new
                        {
                            RoleId = "3",
                            PermissionId = "1"
                        },
                        new
                        {
                            RoleId = "3",
                            PermissionId = "14"
                        },
                        new
                        {
                            RoleId = "3",
                            PermissionId = "15"
                        },
                        new
                        {
                            RoleId = "3",
                            PermissionId = "16"
                        },
                        new
                        {
                            RoleId = "3",
                            PermissionId = "19"
                        },
                        new
                        {
                            RoleId = "3",
                            PermissionId = "26"
                        },
                        new
                        {
                            RoleId = "4",
                            PermissionId = "14"
                        },
                        new
                        {
                            RoleId = "4",
                            PermissionId = "19"
                        },
                        new
                        {
                            RoleId = "4",
                            PermissionId = "26"
                        },
                        new
                        {
                            RoleId = "5",
                            PermissionId = "3"
                        },
                        new
                        {
                            RoleId = "5",
                            PermissionId = "12"
                        },
                        new
                        {
                            RoleId = "5",
                            PermissionId = "14"
                        },
                        new
                        {
                            RoleId = "5",
                            PermissionId = "19"
                        },
                        new
                        {
                            RoleId = "5",
                            PermissionId = "26"
                        },
                        new
                        {
                            RoleId = "6",
                            PermissionId = "14"
                        });
                });

            modelBuilder.Entity("GameStore.Core.Models.Identity.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("BannedTo")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Email = "admin@example.com",
                            IsDeleted = false,
                            PasswordHash = "0ecb7c82-8aea-6c70-4c34-a16891f84e7b"
                        },
                        new
                        {
                            Id = "2",
                            Email = "manager@example.com",
                            IsDeleted = false,
                            PasswordHash = "0ecb7c82-8aea-6c70-4c34-a16891f84e7b"
                        },
                        new
                        {
                            Id = "3",
                            Email = "moderator@example.com",
                            IsDeleted = false,
                            PasswordHash = "0ecb7c82-8aea-6c70-4c34-a16891f84e7b"
                        },
                        new
                        {
                            Id = "4",
                            Email = "user@example.com",
                            IsDeleted = false,
                            PasswordHash = "0ecb7c82-8aea-6c70-4c34-a16891f84e7b"
                        });
                });

            modelBuilder.Entity("GameStore.Core.Models.Identity.UserRole", b =>
                {
                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole");

                    b.HasData(
                        new
                        {
                            RoleId = "1",
                            UserId = "1"
                        },
                        new
                        {
                            RoleId = "2",
                            UserId = "2"
                        },
                        new
                        {
                            RoleId = "3",
                            UserId = "3"
                        },
                        new
                        {
                            RoleId = "4",
                            UserId = "4"
                        });
                });

            modelBuilder.Entity("GameStore.Core.Models.Order", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Freight")
                        .HasColumnType("float");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Region")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RequiredDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ShippedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ShipperEntityId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("GameStore.Core.Models.OrderDetails", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<float>("Discount")
                        .HasColumnType("real");

                    b.Property<string>("GameRootId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("OrderId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Price")
                        .HasColumnType("money");

                    b.Property<short>("Quantity")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("GameRootId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("GameStore.Core.Models.PaymentType", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("GameStore.Core.Models.Platform", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Platforms");
                });

            modelBuilder.Entity("GameStore.Core.Models.Publisher", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("ContactName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("ntext");

                    b.Property<string>("Fax")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomePage")
                        .HasColumnType("ntext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Region")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Publishers");
                });

            modelBuilder.Entity("GameStore.Core.Models.Visit", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("GameRootId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("Value")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("GameRootId")
                        .IsUnique();

                    b.ToTable("Visits");
                });

            modelBuilder.Entity("GameStore.Core.Models.Comment", b =>
                {
                    b.HasOne("GameStore.Core.Models.GameRoot", "GameRoot")
                        .WithMany("Comments")
                        .HasForeignKey("GameRootId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameStore.Core.Models.Comment", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");

                    b.HasOne("GameStore.Core.Models.Identity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GameStore.Core.Models.GameDetails", b =>
                {
                    b.HasOne("GameStore.Core.Models.GameRoot", "GameRoot")
                        .WithOne("Details")
                        .HasForeignKey("GameStore.Core.Models.GameDetails", "GameRootId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GameStore.Core.Models.GameGenre", b =>
                {
                    b.HasOne("GameStore.Core.Models.GameRoot", "GameRoot")
                        .WithMany("GameGenres")
                        .HasForeignKey("GameRootId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameStore.Core.Models.Genre", "Genre")
                        .WithMany("GameGenres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GameStore.Core.Models.GamePlatform", b =>
                {
                    b.HasOne("GameStore.Core.Models.GameRoot", "GameRoot")
                        .WithMany("GamePlatforms")
                        .HasForeignKey("GameRootId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameStore.Core.Models.Platform", "Platform")
                        .WithMany("GamePlatforms")
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GameStore.Core.Models.Genre", b =>
                {
                    b.HasOne("GameStore.Core.Models.Genre", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("GameStore.Core.Models.Identity.RolePermission", b =>
                {
                    b.HasOne("GameStore.Core.Models.Identity.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameStore.Core.Models.Identity.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GameStore.Core.Models.Identity.UserRole", b =>
                {
                    b.HasOne("GameStore.Core.Models.Identity.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameStore.Core.Models.Identity.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GameStore.Core.Models.Order", b =>
                {
                    b.HasOne("GameStore.Core.Models.Identity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GameStore.Core.Models.OrderDetails", b =>
                {
                    b.HasOne("GameStore.Core.Models.GameRoot", "GameRoot")
                        .WithMany()
                        .HasForeignKey("GameRootId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameStore.Core.Models.Order", "Order")
                        .WithMany("Details")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GameStore.Core.Models.Publisher", b =>
                {
                    b.HasOne("GameStore.Core.Models.Identity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("GameStore.Core.Models.Visit", b =>
                {
                    b.HasOne("GameStore.Core.Models.GameRoot", "GameRoot")
                        .WithOne("Visit")
                        .HasForeignKey("GameStore.Core.Models.Visit", "GameRootId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
