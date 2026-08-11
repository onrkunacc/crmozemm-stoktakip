using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HospitalInventoryManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRelatedTransactionID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c66cf387-2102-4693-82cc-95f0037f853e");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "948fe3c9-4f6c-49a5-999a-e657d401c0ac", "6c07144f-aa62-46dd-8242-ca5918b343d9" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "948fe3c9-4f6c-49a5-999a-e657d401c0ac");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6c07144f-aa62-46dd-8242-ca5918b343d9");

            migrationBuilder.AddColumn<int>(
                name: "RelatedTransactionID",
                table: "StockTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "c28db181-1406-410e-ae37-a2e97d3b231d", null, "Admin", "ADMIN" },
                    { "fb131caa-460c-4611-a372-f524af05164d", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "HospitalID", "IsAdmin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c39a8c28-9d21-42d7-945e-8a5f55967477", 0, "89206c6d-15bf-4e85-bba3-f5d26d21a3d9", "admin@ozemmedikal.com", true, 0, false, false, null, "ADMIN@OZEMEDIKAL.COM", "ADMIN@OZEMMEDIKAL.COM", "AQAAAAIAAYagAAAAEHpabviKYDYMT+CbHUnuoCXkWkEGAzoGSWIrsbJ/6NpFCARqfz649BbzoH6WKjrUxQ==", null, false, "4b8591d0-0300-484a-ba57-d81e948b9673", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "c28db181-1406-410e-ae37-a2e97d3b231d", "c39a8c28-9d21-42d7-945e-8a5f55967477" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb131caa-460c-4611-a372-f524af05164d");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "c28db181-1406-410e-ae37-a2e97d3b231d", "c39a8c28-9d21-42d7-945e-8a5f55967477" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c28db181-1406-410e-ae37-a2e97d3b231d");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c39a8c28-9d21-42d7-945e-8a5f55967477");

            migrationBuilder.DropColumn(
                name: "RelatedTransactionID",
                table: "StockTransactions");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "948fe3c9-4f6c-49a5-999a-e657d401c0ac", null, "Admin", "ADMIN" },
                    { "c66cf387-2102-4693-82cc-95f0037f853e", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "HospitalID", "IsAdmin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6c07144f-aa62-46dd-8242-ca5918b343d9", 0, "fc4d5261-cc7a-4d6e-8ffb-83049b3804f5", "admin@ozemmedikal.com", true, 0, false, false, null, "ADMIN@OZEMEDIKAL.COM", "ADMIN@OZEMMEDIKAL.COM", "AQAAAAIAAYagAAAAEH1rj0LdxX5jAx7Lfz5Sb8nj0fARXSQTRMKW3/+S/D4gQ2dbgbbJ5Iz2/5BhxCTi/A==", null, false, "7722c1cb-4cb4-4269-a23b-95916502b1d9", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "948fe3c9-4f6c-49a5-999a-e657d401c0ac", "6c07144f-aa62-46dd-8242-ca5918b343d9" });
        }
    }
}
