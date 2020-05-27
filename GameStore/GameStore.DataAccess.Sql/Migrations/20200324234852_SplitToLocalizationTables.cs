using System.IO;
using GameStore.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameStore.DataAccess.Sql.Migrations
{
    public partial class SplitToLocalizationTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "ContactName",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "ContactTitle",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "GameDetails");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "GameDetails");

            migrationBuilder.DropColumn(
                name: "QuantityPerUnit",
                table: "GameDetails");

            migrationBuilder.CreateTable(
                name: "UserCultures",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_UserCultures", x => x.Name); });

            migrationBuilder.CreateTable(
                name: "GameLocalizations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    QuantityPerUnit = table.Column<string>(nullable: true),
                    GameRootId = table.Column<string>(nullable: false),
                    CultureName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameLocalizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameLocalizations_UserCultures_CultureName",
                        column: x => x.CultureName,
                        principalTable: "UserCultures",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameLocalizations_GameRoots_GameRootId",
                        column: x => x.GameRootId,
                        principalTable: "GameRoots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PublisherLocalizations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PublisherEntityId = table.Column<string>(nullable: true),
                    ContactName = table.Column<string>(nullable: true),
                    ContactTitle = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Description = table.Column<string>(type: "ntext", nullable: true),
                    CultureName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublisherLocalizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublisherLocalizations_UserCultures_CultureName",
                        column: x => x.CultureName,
                        principalTable: "UserCultures",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UserCultures",
                column: "Name",
                value: "en-US");

            migrationBuilder.InsertData(
                table: "UserCultures",
                column: "Name",
                value: "ru-RU");

            migrationBuilder.CreateIndex(
                name: "IX_GameLocalizations_CultureName",
                table: "GameLocalizations",
                column: "CultureName");

            migrationBuilder.CreateIndex(
                name: "IX_GameLocalizations_GameRootId",
                table: "GameLocalizations",
                column: "GameRootId");

            migrationBuilder.CreateIndex(
                name: "IX_PublisherLocalizations_CultureName",
                table: "PublisherLocalizations",
                column: "CultureName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameLocalizations");

            migrationBuilder.DropTable(
                name: "PublisherLocalizations");

            migrationBuilder.DropTable(
                name: "UserCultures");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactName",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactTitle",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Publishers",
                type: "ntext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "GameDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "GameDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QuantityPerUnit",
                table: "GameDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}