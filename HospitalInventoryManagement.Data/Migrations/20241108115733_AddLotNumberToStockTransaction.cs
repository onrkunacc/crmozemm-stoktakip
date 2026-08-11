using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HospitalInventoryManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLotNumberToStockTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "LotNumber",
                table: "StockTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a7ee1922-5c16-451b-ab3a-d2d52b144875", null, "Admin", "ADMIN" },
                    { "f7cb041c-f9de-4a28-991c-0c9444c94885", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "HospitalID", "IsAdmin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "efb3c3c4-8cac-4543-b687-378beb469429", 0, "80a82865-b61c-437a-a7ad-2db2a4964efc", "admin@ozemmedikal.com", true, 0, false, false, null, "ADMIN@OZEMEDIKAL.COM", "ADMIN@OZEMMEDIKAL.COM", "AQAAAAIAAYagAAAAEBYpaaGQZQgPzuuAc3i/gQ84oJ69sKrL2vgXHGXqrDG3pLiqR+6OvGgbrlHyn5j0eQ==", null, false, "82483b27-9ee4-43b3-8209-a3a1bbcbdab4", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a7ee1922-5c16-451b-ab3a-d2d52b144875", "efb3c3c4-8cac-4543-b687-378beb469429" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f7cb041c-f9de-4a28-991c-0c9444c94885");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a7ee1922-5c16-451b-ab3a-d2d52b144875", "efb3c3c4-8cac-4543-b687-378beb469429" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a7ee1922-5c16-451b-ab3a-d2d52b144875");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "efb3c3c4-8cac-4543-b687-378beb469429");

            migrationBuilder.DropColumn(
                name: "LotNumber",
                table: "StockTransactions");

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
    }
}
