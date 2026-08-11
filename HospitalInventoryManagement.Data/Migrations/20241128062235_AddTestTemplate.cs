using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HospitalInventoryManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTestTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0f6f11fa-dbc1-45a3-bc1a-2e0063a39cbe");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "83e406e2-ecc7-41f3-831b-b34f01752904", "5b453e35-864f-43d4-920c-040567799bad" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "83e406e2-ecc7-41f3-831b-b34f01752904");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5b453e35-864f-43d4-920c-040567799bad");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4c77a585-e1ee-40ea-a439-fd7f732e623c", null, "User", "USER" },
                    { "95141cc7-67c4-4fb0-913d-7f2d58c896ba", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "HospitalID", "IsAdmin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "bd22e009-aafe-46fd-abeb-bbb95cc0bb5d", 0, "21d26c65-44e1-4b86-8b46-4c4be8b8d02e", "admin@ozemmedikal.com", true, 0, false, false, null, "ADMIN@OZEMEDIKAL.COM", "ADMIN@OZEMMEDIKAL.COM", "AQAAAAIAAYagAAAAEOXVBlH26UiLtTb+tuSKMunNLqE7LCFhWAM7wmOEh8NeLYfKxQ+jU3S0LXMDnNa8Kw==", null, false, "c421807a-618d-4842-acd3-c07744dffb78", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "95141cc7-67c4-4fb0-913d-7f2d58c896ba", "bd22e009-aafe-46fd-abeb-bbb95cc0bb5d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4c77a585-e1ee-40ea-a439-fd7f732e623c");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "95141cc7-67c4-4fb0-913d-7f2d58c896ba", "bd22e009-aafe-46fd-abeb-bbb95cc0bb5d" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "95141cc7-67c4-4fb0-913d-7f2d58c896ba");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bd22e009-aafe-46fd-abeb-bbb95cc0bb5d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0f6f11fa-dbc1-45a3-bc1a-2e0063a39cbe", null, "User", "USER" },
                    { "83e406e2-ecc7-41f3-831b-b34f01752904", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "HospitalID", "IsAdmin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "5b453e35-864f-43d4-920c-040567799bad", 0, "ddb04364-27ba-4303-b1d3-180706d7ded5", "admin@ozemmedikal.com", true, 0, false, false, null, "ADMIN@OZEMEDIKAL.COM", "ADMIN@OZEMMEDIKAL.COM", "AQAAAAIAAYagAAAAEOB7MG8ZqNdsBozljUFymieOEkDKBoFoXP4N7YOJtqJvWKtiSBuX8PNVcXV3Y7dX9A==", null, false, "d1ebed07-b9d2-4153-9c1c-c95bb95d1e02", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "83e406e2-ecc7-41f3-831b-b34f01752904", "5b453e35-864f-43d4-920c-040567799bad" });
        }
    }
}
