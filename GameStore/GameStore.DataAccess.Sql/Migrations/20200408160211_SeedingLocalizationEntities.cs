using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameStore.DataAccess.Sql.Migrations
{
    public partial class SeedingLocalizationEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GameRoots",
                columns: new[] { "Id", "IsDeleted", "Key", "PublisherEntityId" },
                values: new object[,]
                {
                    { "1", false, "key1", "102" },
                    { "28", false, "key28", "102" },
                    { "29", false, "key29", "103" },
                    { "30", false, "key30", "101" },
                    { "31", false, "key31", "102" },
                    { "32", false, "key32", "103" },
                    { "33", false, "key33", "101" },
                    { "34", false, "key34", "102" },
                    { "35", false, "key35", "103" },
                    { "36", false, "key36", "101" },
                    { "38", false, "key38", "103" },
                    { "27", false, "key27", "101" },
                    { "39", false, "key39", "101" },
                    { "41", false, "key41", "103" },
                    { "42", false, "key42", "101" },
                    { "43", false, "key43", "102" },
                    { "44", false, "key44", "103" },
                    { "45", false, "key45", "101" },
                    { "46", false, "key46", "102" },
                    { "47", false, "key47", "103" },
                    { "48", false, "key48", "101" },
                    { "49", false, "key49", "102" },
                    { "50", false, "key50", "103" },
                    { "40", false, "key40", "102" },
                    { "26", false, "key26", "103" },
                    { "37", false, "key37", "102" },
                    { "24", false, "key24", "101" },
                    { "25", false, "key25", "102" },
                    { "3", false, "key3", "101" },
                    { "4", false, "key4", "102" },
                    { "5", false, "key5", "103" },
                    { "6", false, "key6", "101" },
                    { "7", false, "key7", "102" },
                    { "8", false, "key8", "103" },
                    { "9", false, "key9", "101" },
                    { "10", false, "key10", "102" },
                    { "11", false, "key11", "103" },
                    { "12", false, "key12", "101" },
                    { "2", false, "key2", "103" },
                    { "14", false, "key14", "103" },
                    { "15", false, "key15", "101" },
                    { "16", false, "key16", "102" },
                    { "17", false, "key17", "103" },
                    { "18", false, "key18", "101" },
                    { "19", false, "key19", "102" },
                    { "20", false, "key20", "103" },
                    { "21", false, "key21", "101" },
                    { "22", false, "key22", "102" },
                    { "23", false, "key23", "103" },
                    { "13", false, "key13", "102" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "IsDeleted", "Name", "ParentId" },
                values: new object[,]
                {
                    { "15", false, "Puzzle & Skill", null },
                    { "11", false, "Action", null },
                    { "6", false, "Races", null },
                    { "14", false, "Adventure", null },
                    { "4", false, "RPG", null },
                    { "1", false, "Strategy", null },
                    { "5", false, "Sports", null }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "Description", "ImagePath", "Title" },
                values: new object[,]
                {
                    { "1", "Best bank in the world", "/img/Bank.png", "Bank" },
                    { "2", "Best IBox terminal in the world", "/img/IBox.png", "IBox terminal" },
                    { "3", "Best Visa in the world", "/img/Visa.png", "Visa" }
                });

            migrationBuilder.InsertData(
                table: "Platforms",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { "3", false, "Mobile" },
                    { "4", false, "Browser" },
                    { "1", false, "Console" },
                    { "2", false, "Desktop" }
                });

            migrationBuilder.InsertData(
                table: "PublisherLocalizations",
                columns: new[] { "Id", "Address", "City", "ContactName", "ContactTitle", "Country", "CultureName", "Description", "PublisherEntityId", "Region" },
                values: new object[,]
                {
                    { "5", null, null, null, null, null, "en-US", "Not bad", "3", null },
                    { "1", null, null, null, null, null, "en-US", "Best company in the world", "1", null },
                    { "2", null, null, null, null, null, "ru-RU", "Самая лучшая компания в мире", "1", null },
                    { "3", null, null, null, null, null, "en-US", "Good company", "2", null },
                    { "4", null, null, null, null, null, "ru-RU", "Хорошая компания", "2", null },
                    { "6", null, null, null, null, null, "ru-RU", "Ничё так", "3", null }
                });

            migrationBuilder.InsertData(
                table: "Publishers",
                columns: new[] { "Id", "CompanyName", "Fax", "HomePage", "IsDeleted", "Phone", "PostalCode", "UserId" },
                values: new object[,]
                {
                    { "101", "Microsoft", null, "https://www.microsoft.com/", false, null, null, null },
                    { "102", "Google", null, "https://www.google.com/", false, null, null, null },
                    { "103", "Amazon", null, "https://www.amazon.com/", false, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "GameDetails",
                columns: new[] { "Id", "CreationDate", "Discount", "GameRootId", "IsDiscontinued", "Price", "UnitsInStock", "UnitsOnOrder" },
                values: new object[,]
                {
                    { "1", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "1", false, 10000m, null, 0 },
                    { "27", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "27", false, 370m, null, 0 },
                    { "26", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "26", false, 384m, null, 0 },
                    { "48", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "48", false, 208m, null, 0 },
                    { "25", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "25", false, 400m, null, 0 },
                    { "49", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "49", false, 204m, null, 0 },
                    { "24", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "24", false, 416m, null, 0 },
                    { "47", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "47", false, 212m, null, 0 },
                    { "23", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "23", false, 434m, null, 0 },
                    { "22", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "22", false, 454m, null, 0 },
                    { "21", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "21", false, 476m, null, 0 },
                    { "19", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "19", false, 526m, null, 0 },
                    { "18", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "18", false, 555m, null, 0 },
                    { "17", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "17", false, 588m, null, 0 },
                    { "16", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "16", false, 625m, null, 0 },
                    { "50", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "50", false, 200m, null, 0 },
                    { "28", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "28", false, 357m, null, 0 },
                    { "46", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "46", false, 217m, null, 0 },
                    { "29", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "29", false, 344m, null, 0 },
                    { "38", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "38", false, 263m, null, 0 },
                    { "40", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "40", false, 250m, null, 0 },
                    { "37", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "37", false, 270m, null, 0 },
                    { "41", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "41", false, 243m, null, 0 },
                    { "36", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "36", false, 277m, null, 0 },
                    { "35", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "35", false, 285m, null, 0 },
                    { "42", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "42", false, 238m, null, 0 },
                    { "34", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "34", false, 294m, null, 0 },
                    { "43", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "43", false, 232m, null, 0 },
                    { "33", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "33", false, 303m, null, 0 },
                    { "32", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "32", false, 312m, null, 0 },
                    { "44", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "44", false, 227m, null, 0 },
                    { "31", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "31", false, 322m, null, 0 },
                    { "45", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "45", false, 222m, null, 0 },
                    { "30", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "30", false, 333m, null, 0 },
                    { "15", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "15", false, 666m, null, 0 },
                    { "14", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "14", false, 714m, null, 0 },
                    { "20", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "20", false, 500m, null, 0 },
                    { "8", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "8", false, 1250m, null, 0 },
                    { "2", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "2", false, 5000m, null, 0 },
                    { "3", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "3", false, 3333m, null, 0 },
                    { "4", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "4", false, 2500m, null, 0 },
                    { "5", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "5", false, 2000m, null, 0 },
                    { "6", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "6", false, 1666m, null, 0 },
                    { "7", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "7", false, 1428m, null, 0 },
                    { "13", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "13", false, 769m, null, 0 },
                    { "9", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "9", false, 1111m, null, 0 },
                    { "10", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "10", false, 1000m, null, 0 },
                    { "11", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "11", false, 909m, null, 0 },
                    { "39", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "39", false, 256m, null, 0 },
                    { "12", new DateTime(2019, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, "12", false, 833m, null, 0 }
                });

            migrationBuilder.InsertData(
                table: "GameGenre",
                columns: new[] { "GameRootId", "GenreId" },
                values: new object[,]
                {
                    { "16", "4" },
                    { "5", "11" },
                    { "12", "11" },
                    { "44", "5" },
                    { "19", "11" },
                    { "26", "11" },
                    { "37", "5" },
                    { "33", "11" },
                    { "40", "11" },
                    { "2", "5" },
                    { "47", "11" },
                    { "7", "14" },
                    { "9", "5" },
                    { "14", "14" },
                    { "21", "14" },
                    { "30", "5" },
                    { "28", "14" },
                    { "35", "14" },
                    { "16", "5" },
                    { "42", "14" },
                    { "49", "14" },
                    { "37", "4" },
                    { "45", "6" },
                    { "44", "4" },
                    { "38", "6" },
                    { "9", "4" },
                    { "3", "6" },
                    { "2", "4" },
                    { "49", "1" },
                    { "23", "4" },
                    { "42", "1" },
                    { "35", "1" },
                    { "28", "1" },
                    { "21", "1" },
                    { "10", "6" },
                    { "14", "1" },
                    { "7", "1" },
                    { "17", "6" },
                    { "23", "5" },
                    { "30", "4" },
                    { "24", "6" },
                    { "31", "6" }
                });

            migrationBuilder.InsertData(
                table: "GameLocalizations",
                columns: new[] { "Id", "CultureName", "Description", "GameRootId", "Name", "QuantityPerUnit" },
                values: new object[,]
                {
                    { "88", "ru-RU", null, "44", "Имя", null },
                    { "78", "ru-RU", null, "39", "Имя", null },
                    { "96", "ru-RU", null, "48", "Имя", null },
                    { "94", "ru-RU", null, "47", "Имя", null },
                    { "79", "en-US", null, "40", "Name", null },
                    { "93", "en-US", null, "47", "Name", null },
                    { "80", "ru-RU", null, "40", "Имя", null },
                    { "97", "en-US", null, "49", "Name", null },
                    { "81", "en-US", null, "41", "Name", null },
                    { "82", "ru-RU", null, "41", "Имя", null },
                    { "98", "ru-RU", null, "49", "Имя", null },
                    { "99", "en-US", null, "50", "Name", null },
                    { "83", "en-US", null, "42", "Name", null },
                    { "77", "en-US", null, "39", "Name", null },
                    { "84", "ru-RU", null, "42", "Имя", null },
                    { "91", "en-US", null, "46", "Name", null },
                    { "85", "en-US", null, "43", "Name", null },
                    { "95", "en-US", null, "48", "Name", null },
                    { "90", "ru-RU", null, "45", "Имя", null },
                    { "87", "en-US", null, "44", "Name", null },
                    { "89", "en-US", null, "45", "Name", null },
                    { "92", "ru-RU", null, "46", "Имя", null },
                    { "86", "ru-RU", null, "43", "Имя", null },
                    { "100", "ru-RU", null, "50", "Имя", null },
                    { "75", "en-US", null, "38", "Name", null },
                    { "20", "ru-RU", null, "10", "Имя", null },
                    { "21", "en-US", null, "11", "Name", null },
                    { "22", "ru-RU", null, "11", "Имя", null },
                    { "23", "en-US", null, "12", "Name", null },
                    { "24", "ru-RU", null, "12", "Имя", null },
                    { "25", "en-US", null, "13", "Name", null },
                    { "26", "ru-RU", null, "13", "Имя", null },
                    { "27", "en-US", null, "14", "Name", null },
                    { "29", "en-US", null, "15", "Name", null },
                    { "30", "ru-RU", null, "15", "Имя", null },
                    { "31", "en-US", null, "16", "Name", null },
                    { "32", "ru-RU", null, "16", "Имя", null },
                    { "33", "en-US", null, "17", "Name", null },
                    { "34", "ru-RU", null, "17", "Имя", null },
                    { "35", "en-US", null, "18", "Name", null },
                    { "19", "en-US", null, "10", "Name", null },
                    { "18", "ru-RU", null, "9", "Имя", null },
                    { "17", "en-US", null, "9", "Name", null },
                    { "16", "ru-RU", null, "8", "Имя", null },
                    { "76", "ru-RU", null, "38", "Имя", null },
                    { "1", "en-US", null, "1", "Name", null },
                    { "2", "ru-RU", null, "1", "Имя", null },
                    { "3", "en-US", null, "2", "Name", null },
                    { "4", "ru-RU", null, "2", "Имя", null },
                    { "5", "en-US", null, "3", "Name", null },
                    { "6", "ru-RU", null, "3", "Имя", null },
                    { "36", "ru-RU", null, "18", "Имя", null },
                    { "7", "en-US", null, "4", "Name", null },
                    { "9", "en-US", null, "5", "Name", null },
                    { "10", "ru-RU", null, "5", "Имя", null },
                    { "11", "en-US", null, "6", "Name", null },
                    { "12", "ru-RU", null, "6", "Имя", null },
                    { "13", "en-US", null, "7", "Name", null },
                    { "14", "ru-RU", null, "7", "Имя", null },
                    { "15", "en-US", null, "8", "Name", null },
                    { "8", "ru-RU", null, "4", "Имя", null },
                    { "37", "en-US", null, "19", "Name", null },
                    { "28", "ru-RU", null, "14", "Имя", null },
                    { "39", "en-US", null, "20", "Name", null },
                    { "58", "ru-RU", null, "29", "Имя", null },
                    { "59", "en-US", null, "30", "Name", null },
                    { "60", "ru-RU", null, "30", "Имя", null },
                    { "38", "ru-RU", null, "19", "Имя", null },
                    { "62", "ru-RU", null, "31", "Имя", null },
                    { "63", "en-US", null, "32", "Name", null },
                    { "64", "ru-RU", null, "32", "Имя", null },
                    { "65", "en-US", null, "33", "Name", null },
                    { "66", "ru-RU", null, "33", "Имя", null },
                    { "67", "en-US", null, "34", "Name", null },
                    { "68", "ru-RU", null, "34", "Имя", null },
                    { "69", "en-US", null, "35", "Name", null },
                    { "70", "ru-RU", null, "35", "Имя", null },
                    { "71", "en-US", null, "36", "Name", null },
                    { "72", "ru-RU", null, "36", "Имя", null },
                    { "73", "en-US", null, "37", "Name", null },
                    { "74", "ru-RU", null, "37", "Имя", null },
                    { "57", "en-US", null, "29", "Name", null },
                    { "56", "ru-RU", null, "28", "Имя", null },
                    { "61", "en-US", null, "31", "Name", null },
                    { "54", "ru-RU", null, "27", "Имя", null },
                    { "40", "ru-RU", null, "20", "Имя", null },
                    { "41", "en-US", null, "21", "Name", null },
                    { "55", "en-US", null, "28", "Name", null },
                    { "43", "en-US", null, "22", "Name", null },
                    { "44", "ru-RU", null, "22", "Имя", null },
                    { "45", "en-US", null, "23", "Name", null },
                    { "46", "ru-RU", null, "23", "Имя", null },
                    { "47", "en-US", null, "24", "Name", null },
                    { "48", "ru-RU", null, "24", "Имя", null },
                    { "42", "ru-RU", null, "21", "Имя", null },
                    { "50", "ru-RU", null, "25", "Имя", null },
                    { "53", "en-US", null, "27", "Name", null },
                    { "51", "en-US", null, "26", "Name", null },
                    { "52", "ru-RU", null, "26", "Имя", null },
                    { "49", "en-US", null, "25", "Name", null }
                });

            migrationBuilder.InsertData(
                table: "GamePlatform",
                columns: new[] { "GameRootId", "PlatformId" },
                values: new object[,]
                {
                    { "4", "3" },
                    { "3", "3" },
                    { "1", "3" },
                    { "50", "2" },
                    { "49", "2" },
                    { "47", "2" },
                    { "46", "2" },
                    { "44", "2" },
                    { "43", "2" },
                    { "41", "2" },
                    { "40", "2" },
                    { "37", "2" },
                    { "35", "2" },
                    { "23", "2" },
                    { "25", "2" },
                    { "22", "2" },
                    { "26", "2" },
                    { "28", "2" },
                    { "34", "2" },
                    { "32", "2" },
                    { "29", "2" },
                    { "31", "2" },
                    { "6", "3" },
                    { "38", "2" },
                    { "7", "3" },
                    { "42", "3" },
                    { "10", "3" },
                    { "46", "3" },
                    { "45", "3" },
                    { "43", "3" },
                    { "20", "2" },
                    { "40", "3" },
                    { "39", "3" },
                    { "37", "3" },
                    { "36", "3" },
                    { "34", "3" },
                    { "33", "3" },
                    { "31", "3" },
                    { "9", "3" },
                    { "30", "3" },
                    { "27", "3" },
                    { "25", "3" },
                    { "24", "3" },
                    { "22", "3" },
                    { "21", "3" },
                    { "19", "3" },
                    { "18", "3" },
                    { "16", "3" },
                    { "15", "3" },
                    { "13", "3" },
                    { "12", "3" },
                    { "28", "3" },
                    { "19", "2" },
                    { "12", "1" },
                    { "16", "2" },
                    { "29", "1" },
                    { "17", "2" },
                    { "26", "1" },
                    { "24", "1" },
                    { "23", "1" },
                    { "21", "1" },
                    { "20", "1" },
                    { "18", "1" },
                    { "17", "1" },
                    { "15", "1" },
                    { "14", "1" },
                    { "11", "1" },
                    { "9", "1" },
                    { "8", "1" },
                    { "6", "1" },
                    { "5", "1" },
                    { "3", "1" },
                    { "2", "1" },
                    { "48", "3" },
                    { "30", "1" },
                    { "32", "1" },
                    { "27", "1" },
                    { "35", "1" },
                    { "14", "2" },
                    { "13", "2" },
                    { "11", "2" },
                    { "10", "2" },
                    { "8", "2" },
                    { "33", "1" },
                    { "5", "2" },
                    { "4", "2" },
                    { "2", "2" },
                    { "1", "2" },
                    { "7", "2" },
                    { "48", "1" },
                    { "36", "1" },
                    { "50", "1" },
                    { "38", "1" },
                    { "39", "1" },
                    { "49", "3" },
                    { "42", "1" },
                    { "44", "1" },
                    { "45", "1" },
                    { "47", "1" },
                    { "41", "1" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "IsDeleted", "Name", "ParentId" },
                values: new object[,]
                {
                    { "13", false, "TPS", "11" },
                    { "10", false, "Off-road", "6" },
                    { "9", false, "Formula", "6" },
                    { "8", false, "Arcade", "6" },
                    { "7", false, "Rally", "6" },
                    { "3", false, "TBS", "1" },
                    { "2", false, "RTS", "1" },
                    { "12", false, "FPS", "11" }
                });

            migrationBuilder.InsertData(
                table: "GameGenre",
                columns: new[] { "GameRootId", "GenreId" },
                values: new object[,]
                {
                    { "1", "2" },
                    { "11", "9" },
                    { "18", "9" },
                    { "25", "9" },
                    { "32", "9" },
                    { "39", "9" },
                    { "46", "9" },
                    { "5", "10" },
                    { "12", "10" },
                    { "19", "10" },
                    { "26", "10" },
                    { "33", "10" },
                    { "40", "10" },
                    { "47", "10" },
                    { "6", "12" },
                    { "13", "12" },
                    { "20", "12" },
                    { "27", "12" },
                    { "34", "12" },
                    { "41", "12" },
                    { "48", "12" },
                    { "6", "13" },
                    { "13", "13" },
                    { "20", "13" },
                    { "27", "13" },
                    { "34", "13" },
                    { "4", "9" },
                    { "46", "8" },
                    { "39", "8" },
                    { "32", "8" },
                    { "8", "2" },
                    { "15", "2" },
                    { "22", "2" },
                    { "29", "2" },
                    { "36", "2" },
                    { "43", "2" },
                    { "50", "2" },
                    { "1", "3" },
                    { "8", "3" },
                    { "15", "3" },
                    { "22", "3" },
                    { "29", "3" },
                    { "41", "13" },
                    { "36", "3" },
                    { "50", "3" },
                    { "3", "7" },
                    { "10", "7" },
                    { "17", "7" },
                    { "24", "7" },
                    { "31", "7" },
                    { "38", "7" },
                    { "45", "7" },
                    { "4", "8" },
                    { "11", "8" },
                    { "18", "8" },
                    { "25", "8" },
                    { "43", "3" },
                    { "48", "13" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "10");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "11");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "12");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "13");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "14");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "15");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "16");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "17");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "18");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "19");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "20");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "21");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "22");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "23");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "24");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "25");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "26");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "27");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "28");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "29");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "30");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "31");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "32");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "33");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "34");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "35");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "36");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "37");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "38");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "39");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "40");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "41");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "42");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "43");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "44");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "45");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "46");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "47");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "48");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "49");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "5");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "50");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "6");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "7");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "8");

            migrationBuilder.DeleteData(
                table: "GameDetails",
                keyColumn: "Id",
                keyValue: "9");

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "1", "2" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "1", "3" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "10", "6" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "10", "7" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "11", "8" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "11", "9" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "12", "10" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "12", "11" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "13", "12" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "13", "13" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "14", "1" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "14", "14" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "15", "2" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "15", "3" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "16", "4" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "16", "5" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "17", "6" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "17", "7" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "18", "8" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "18", "9" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "19", "10" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "19", "11" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "2", "4" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "2", "5" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "20", "12" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "20", "13" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "21", "1" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "21", "14" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "22", "2" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "22", "3" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "23", "4" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "23", "5" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "24", "6" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "24", "7" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "25", "8" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "25", "9" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "26", "10" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "26", "11" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "27", "12" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "27", "13" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "28", "1" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "28", "14" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "29", "2" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "29", "3" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "3", "6" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "3", "7" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "30", "4" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "30", "5" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "31", "6" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "31", "7" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "32", "8" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "32", "9" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "33", "10" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "33", "11" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "34", "12" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "34", "13" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "35", "1" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "35", "14" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "36", "2" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "36", "3" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "37", "4" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "37", "5" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "38", "6" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "38", "7" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "39", "8" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "39", "9" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "4", "8" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "4", "9" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "40", "10" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "40", "11" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "41", "12" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "41", "13" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "42", "1" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "42", "14" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "43", "2" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "43", "3" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "44", "4" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "44", "5" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "45", "6" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "45", "7" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "46", "8" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "46", "9" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "47", "10" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "47", "11" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "48", "12" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "48", "13" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "49", "1" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "49", "14" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "5", "10" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "5", "11" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "50", "2" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "50", "3" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "6", "12" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "6", "13" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "7", "1" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "7", "14" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "8", "2" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "8", "3" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "9", "4" });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "GameRootId", "GenreId" },
                keyValues: new object[] { "9", "5" });

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "10");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "100");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "11");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "12");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "13");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "14");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "15");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "16");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "17");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "18");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "19");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "20");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "21");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "22");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "23");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "24");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "25");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "26");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "27");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "28");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "29");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "30");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "31");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "32");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "33");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "34");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "35");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "36");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "37");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "38");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "39");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "40");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "41");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "42");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "43");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "44");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "45");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "46");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "47");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "48");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "49");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "5");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "50");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "51");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "52");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "53");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "54");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "55");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "56");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "57");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "58");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "59");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "6");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "60");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "61");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "62");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "63");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "64");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "65");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "66");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "67");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "68");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "69");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "7");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "70");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "71");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "72");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "73");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "74");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "75");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "76");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "77");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "78");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "79");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "8");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "80");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "81");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "82");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "83");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "84");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "85");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "86");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "87");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "88");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "89");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "9");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "90");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "91");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "92");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "93");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "94");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "95");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "96");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "97");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "98");

            migrationBuilder.DeleteData(
                table: "GameLocalizations",
                keyColumn: "Id",
                keyValue: "99");

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "1", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "1", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "10", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "10", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "11", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "11", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "12", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "12", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "13", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "13", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "14", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "14", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "15", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "15", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "16", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "16", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "17", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "17", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "18", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "18", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "19", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "19", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "2", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "2", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "20", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "20", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "21", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "21", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "22", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "22", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "23", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "23", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "24", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "24", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "25", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "25", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "26", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "26", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "27", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "27", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "28", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "28", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "29", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "29", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "3", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "3", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "30", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "30", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "31", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "31", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "32", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "32", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "33", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "33", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "34", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "34", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "35", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "35", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "36", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "36", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "37", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "37", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "38", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "38", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "39", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "39", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "4", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "4", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "40", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "40", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "41", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "41", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "42", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "42", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "43", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "43", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "44", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "44", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "45", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "45", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "46", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "46", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "47", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "47", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "48", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "48", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "49", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "49", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "5", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "5", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "50", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "50", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "6", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "6", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "7", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "7", "3" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "8", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "8", "2" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "9", "1" });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "GameRootId", "PlatformId" },
                keyValues: new object[] { "9", "3" });

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: "15");

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "PublisherLocalizations",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "PublisherLocalizations",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "PublisherLocalizations",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "PublisherLocalizations",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "PublisherLocalizations",
                keyColumn: "Id",
                keyValue: "5");

            migrationBuilder.DeleteData(
                table: "PublisherLocalizations",
                keyColumn: "Id",
                keyValue: "6");

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "Id",
                keyValue: "101");

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "Id",
                keyValue: "102");

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "Id",
                keyValue: "103");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "10");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "11");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "12");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "13");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "14");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "15");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "16");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "17");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "18");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "19");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "20");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "21");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "22");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "23");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "24");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "25");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "26");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "27");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "28");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "29");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "30");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "31");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "32");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "33");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "34");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "35");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "36");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "37");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "38");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "39");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "40");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "41");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "42");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "43");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "44");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "45");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "46");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "47");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "48");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "49");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "5");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "50");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "6");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "7");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "8");

            migrationBuilder.DeleteData(
                table: "GameRoots",
                keyColumn: "Id",
                keyValue: "9");

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: "10");

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: "12");

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: "13");

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: "14");

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: "5");

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: "7");

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: "8");

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: "9");

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: "11");

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: "6");
        }
    }
}
