using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HospitalInventoryManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddErrorLogsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3356d4f-cf6c-492b-b704-1e256875edd5");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "24e28980-6aa8-48fe-aeb8-40462e0511a9", "434e1eed-3c01-4238-8eed-a12089502dbb" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24e28980-6aa8-48fe-aeb8-40462e0511a9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "434e1eed-3c01-4238-8eed-a12089502dbb");

            migrationBuilder.CreateTable(
                name: "ErrorLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLogs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "489e4cfa-662c-497e-922a-bc87b110a5b5", null, "User", "USER" },
                    { "884f19f0-133b-4665-bc0c-6c963b9271e2", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "HospitalID", "IsAdmin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "dbf36300-2b41-4dbe-b787-ac09017f92f6", 0, "ebddb364-c2f1-48ec-84d3-c0dd20ba9624", "admin@ozemmedikal.com", true, 0, false, false, null, "ADMIN@OZEMEDIKAL.COM", "ADMIN@OZEMMEDIKAL.COM", "AQAAAAIAAYagAAAAEFReIpHKz4YsX40utkLD3CfrmPPdDtFn56w/K4cUGsSnL0F3b/HlzliXTaO6Y4OXgQ==", null, false, "118afc81-ae70-4934-86fd-cd78a81ff0ac", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "884f19f0-133b-4665-bc0c-6c963b9271e2", "dbf36300-2b41-4dbe-b787-ac09017f92f6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorLogs");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "489e4cfa-662c-497e-922a-bc87b110a5b5");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "884f19f0-133b-4665-bc0c-6c963b9271e2", "dbf36300-2b41-4dbe-b787-ac09017f92f6" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "884f19f0-133b-4665-bc0c-6c963b9271e2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dbf36300-2b41-4dbe-b787-ac09017f92f6");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "24e28980-6aa8-48fe-aeb8-40462e0511a9", null, "Admin", "ADMIN" },
                    { "a3356d4f-cf6c-492b-b704-1e256875edd5", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "HospitalID", "IsAdmin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "434e1eed-3c01-4238-8eed-a12089502dbb", 0, "0fcd7c24-fe88-4bb9-977e-c34799f237b1", "admin@ozemmedikal.com", true, 0, false, false, null, "ADMIN@OZEMEDIKAL.COM", "ADMIN@OZEMMEDIKAL.COM", "AQAAAAIAAYagAAAAEHaRsJp6wJRkqEBXA3zQl0wRivkAmav5SvslIoAg5F9rRAI/wHJPSGDGk/G6VrkuSA==", null, false, "3165107f-ad60-4115-81a0-e50c76e972c4", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "24e28980-6aa8-48fe-aeb8-40462e0511a9", "434e1eed-3c01-4238-8eed-a12089502dbb" });
        }
    }
}
