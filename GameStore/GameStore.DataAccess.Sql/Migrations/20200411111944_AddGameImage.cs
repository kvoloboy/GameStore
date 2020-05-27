﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameStore.DataAccess.Sql.Migrations
{
    public partial class AddGameImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameImages",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Content = table.Column<byte[]>(nullable: true),
                    ContentType = table.Column<string>(nullable: true),
                    GameRootId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameImages_GameRoots_GameRootId",
                        column: x => x.GameRootId,
                        principalTable: "GameRoots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Value" },
                values: new object[] { "27", "Manage images" });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "RoleId", "PermissionId" },
                values: new object[] { "1", "27" });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "RoleId", "PermissionId" },
                values: new object[] { "2", "27" });

            migrationBuilder.CreateIndex(
                name: "IX_GameImages_GameRootId",
                table: "GameImages",
                column: "GameRootId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameImages");

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "27" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "2", "27" });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "27");
        }
    }
}
