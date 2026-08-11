using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HospitalInventoryManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddErrorLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0c48775c-01f1-4025-81da-9104306e1ecb");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "972b4722-3c61-405f-bbbd-ef1e98596900", "70280d65-04fb-489d-9652-c569253a5c80" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "972b4722-3c61-405f-bbbd-ef1e98596900");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "70280d65-04fb-489d-9652-c569253a5c80");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0c48775c-01f1-4025-81da-9104306e1ecb", null, "User", "USER" },
                    { "972b4722-3c61-405f-bbbd-ef1e98596900", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "HospitalID", "IsAdmin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "70280d65-04fb-489d-9652-c569253a5c80", 0, "279df767-6574-4d1a-adeb-2568e3d5bb18", "admin@ozemmedikal.com", true, 0, false, false, null, "ADMIN@OZEMEDIKAL.COM", "ADMIN@OZEMMEDIKAL.COM", "AQAAAAIAAYagAAAAECeWJpl9jJZQ8falZROgXuLhAi8w/lq8Ldb48K6PoWpRCpqqy+tqY03YZbqk41GU/w==", null, false, "a657fdd5-8339-4b7b-ba42-cd7b3892c471", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "972b4722-3c61-405f-bbbd-ef1e98596900", "70280d65-04fb-489d-9652-c569253a5c80" });
        }
    }
}
