using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HospitalInventoryManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class YeniOzelliklerTestBazlı : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<int>(
                name: "HospitalID",
                table: "StockTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantityInTests",
                table: "StockTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "StockTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "StockTransactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TestCount",
                table: "Stocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_UserId",
                table: "StockTransactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_AspNetUsers_UserId",
                table: "StockTransactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.Sql(@"
                UPDATE s
                SET s.TestCount = s.Quantity * p.BoxTextCount
                FROM Stocks s
                INNER JOIN Products p ON p.ProductID = s.ProductID
                WHERE p.BoxTextCount > 0;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_AspNetUsers_UserId",
                table: "StockTransactions");

            migrationBuilder.DropIndex(
                name: "IX_StockTransactions_UserId",
                table: "StockTransactions");

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

            migrationBuilder.DropColumn(
                name: "HospitalID",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "QuantityInTests",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "TestCount",
                table: "Stocks");

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
    }
}
