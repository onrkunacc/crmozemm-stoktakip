using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HospitalInventoryManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLotNumberStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6cbed2d-2404-4683-8d7a-0efb9c755af2");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "fd90079f-290a-4fcc-9411-70998e65fd94", "21be7af6-af99-4f39-9d3c-148b4e43cce5" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fd90079f-290a-4fcc-9411-70998e65fd94");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "21be7af6-af99-4f39-9d3c-148b4e43cce5");

            migrationBuilder.AddColumn<string>(
                name: "LotNumber",
                table: "Stocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LotNumber",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1794a3cd-b181-4791-a770-8835f5a65ea8", null, "User", "USER" },
                    { "b9fe3c68-f29d-472d-954d-1038fc52258a", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "HospitalID", "IsAdmin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "eb0842ad-315a-4af8-a1b5-7a5e739341ad", 0, "29eca725-5184-42ba-83c7-28dc44c01d9d", "admin@ozemmedikal.com", true, 0, false, false, null, "ADMIN@OZEMEDIKAL.COM", "ADMIN@OZEMMEDIKAL.COM", "AQAAAAIAAYagAAAAEJpAl20LD52PDijHqAtqhpnKEDI/PCmvrERkGwsoV5FAhd+R8Nhi8IAK/BxxXAU6lA==", null, false, "5b8e8091-affb-43ec-bef6-de831f6bbdb3", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "b9fe3c68-f29d-472d-954d-1038fc52258a", "eb0842ad-315a-4af8-a1b5-7a5e739341ad" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1794a3cd-b181-4791-a770-8835f5a65ea8");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "b9fe3c68-f29d-472d-954d-1038fc52258a", "eb0842ad-315a-4af8-a1b5-7a5e739341ad" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b9fe3c68-f29d-472d-954d-1038fc52258a");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "eb0842ad-315a-4af8-a1b5-7a5e739341ad");

            migrationBuilder.DropColumn(
                name: "LotNumber",
                table: "Stocks");

            migrationBuilder.AlterColumn<string>(
                name: "LotNumber",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "e6cbed2d-2404-4683-8d7a-0efb9c755af2", null, "User", "USER" },
                    { "fd90079f-290a-4fcc-9411-70998e65fd94", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "HospitalID", "IsAdmin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "21be7af6-af99-4f39-9d3c-148b4e43cce5", 0, "9e0b342d-6f45-4369-8f12-7f766cfc5d11", "admin@ozemmedikal.com", true, 0, false, false, null, "ADMIN@OZEMEDIKAL.COM", "ADMIN@OZEMMEDIKAL.COM", "AQAAAAIAAYagAAAAEIpFdxI4ryqCiiU83w6q5Gpoakn6YTWwZdKL0d4iIAXyaxJaGQE87HXWyKFqPRXznQ==", null, false, "b5eebc6f-7c9e-487c-aa41-e9ef2513b3cf", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "fd90079f-290a-4fcc-9411-70998e65fd94", "21be7af6-af99-4f39-9d3c-148b4e43cce5" });
        }
    }
}
