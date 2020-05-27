using Microsoft.EntityFrameworkCore.Migrations;

namespace GameStore.DataAccess.Sql.Migrations
{
    public partial class AddIdentitySeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Value" },
                values: new object[,]
                {
                    { "1", "Read deleted games" },
                    { "26", "Read personal orders" },
                    { "25", "Read roles" },
                    { "24", "Read users" },
                    { "23", "Setup roles" },
                    { "22", "Delete role" },
                    { "21", "Update role" },
                    { "20", "Create role" },
                    { "19", "Make order" },
                    { "17", "Read orders" },
                    { "16", "Ban user" },
                    { "15", "Delete comment" },
                    { "14", "Create comment" },
                    { "18", "Update order" },
                    { "12", "Update publisher" },
                    { "2", "Create game" },
                    { "13", "Delete publisher" },
                    { "4", "Delete game" },
                    { "5", "Create genre" },
                    { "6", "Update genre" },
                    { "3", "Update game" },
                    { "8", "Create platform" },
                    { "9", "Update platform" },
                    { "10", "Delete platform" },
                    { "11", "Create publisher" },
                    { "7", "Delete genre" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "1", "Admin" },
                    { "2", "Manager" },
                    { "3", "Moderator" },
                    { "4", "User" },
                    { "5", "Publisher" },
                    { "6", "Guest" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BannedTo", "Email", "IsDeleted", "PasswordHash" },
                values: new object[,]
                {
                    { "3", null, "moderator@example.com", false, "0ecb7c82-8aea-6c70-4c34-a16891f84e7b" },
                    { "1", null, "admin@example.com", false, "0ecb7c82-8aea-6c70-4c34-a16891f84e7b" },
                    { "2", null, "manager@example.com", false, "0ecb7c82-8aea-6c70-4c34-a16891f84e7b" },
                    { "4", null, "user@example.com", false, "0ecb7c82-8aea-6c70-4c34-a16891f84e7b" }
                });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "RoleId", "PermissionId" },
                values: new object[,]
                {
                    { "1", "1" },
                    { "2", "6" },
                    { "2", "7" },
                    { "2", "8" },
                    { "2", "9" },
                    { "2", "10" },
                    { "2", "11" },
                    { "2", "12" },
                    { "2", "13" },
                    { "2", "19" },
                    { "2", "26" },
                    { "3", "1" },
                    { "3", "14" },
                    { "3", "15" },
                    { "3", "16" },
                    { "3", "19" },
                    { "3", "26" },
                    { "4", "14" },
                    { "4", "19" },
                    { "4", "26" },
                    { "5", "3" },
                    { "5", "12" },
                    { "5", "14" },
                    { "5", "19" },
                    { "5", "26" },
                    { "6", "14" },
                    { "2", "5" },
                    { "2", "3" },
                    { "2", "4" },
                    { "1", "13" },
                    { "1", "3" },
                    { "1", "4" },
                    { "1", "5" },
                    { "1", "6" },
                    { "1", "7" },
                    { "1", "8" },
                    { "1", "9" },
                    { "1", "10" },
                    { "1", "11" },
                    { "1", "12" },
                    { "2", "2" },
                    { "1", "14" },
                    { "1", "15" },
                    { "1", "16" },
                    { "1", "17" },
                    { "1", "18" },
                    { "1", "19" },
                    { "1", "20" },
                    { "1", "21" },
                    { "1", "22" },
                    { "1", "23" },
                    { "1", "24" },
                    { "1", "25" },
                    { "1", "26" },
                    { "2", "1" },
                    { "1", "2" }
                });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "2", "2" },
                    { "3", "3" },
                    { "1", "1" },
                    { "4", "4" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "1" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "10" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "11" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "12" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "13" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "14" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "15" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "16" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "17" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "18" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "19" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "2" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "20" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "21" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "22" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "23" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "24" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "25" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "26" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "3" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "4" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "5" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "6" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "7" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "8" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "1", "9" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "2", "1" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "2", "10" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "2", "11" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "2", "12" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "2", "13" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "2", "19" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "2", "2" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "2", "26" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "2", "3" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "2", "4" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "2", "5" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "2", "6" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "2", "7" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "2", "8" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "2", "9" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "3", "1" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "3", "14" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "3", "15" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "3", "16" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "3", "19" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "3", "26" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "4", "14" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "4", "19" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "4", "26" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "5", "12" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "5", "14" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "5", "19" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "5", "26" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "5", "3" });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { "6", "14" });

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "1" });

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2", "2" });

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3", "3" });

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4", "4" });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "10");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "11");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "12");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "13");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "14");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "15");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "16");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "17");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "18");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "19");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "20");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "21");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "22");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "23");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "24");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "25");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "26");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "5");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "6");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "7");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "8");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: "9");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "5");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "6");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "4");
        }
    }
}
