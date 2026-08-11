using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HospitalInventoryManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFlaconTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c6bee78c-af61-4388-b5bc-9fa9106864e0");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "cfee4c28-e24a-4ab9-bb04-6d0e913b6015", "761cbfc1-84b8-4b07-a440-49cce4b966fe" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cfee4c28-e24a-4ab9-bb04-6d0e913b6015");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "761cbfc1-84b8-4b07-a440-49cce4b966fe");

            migrationBuilder.AddColumn<int>(
                name: "FlaconCount",
                table: "Stocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlaconCountPerBox",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "FlaconCount",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "FlaconCountPerBox",
                table: "Products");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "c6bee78c-af61-4388-b5bc-9fa9106864e0", null, "User", "USER" },
                    { "cfee4c28-e24a-4ab9-bb04-6d0e913b6015", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "HospitalID", "IsAdmin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "761cbfc1-84b8-4b07-a440-49cce4b966fe", 0, "a2ffcdee-f271-4187-9bfa-3ae6dd30b291", "admin@ozemmedikal.com", true, 0, false, false, null, "ADMIN@OZEMEDIKAL.COM", "ADMIN@OZEMMEDIKAL.COM", "AQAAAAIAAYagAAAAEHHYIYDqC7RbqdA3Xxg1DIVRyNaF7Vh09azu3bDQJ9j6A/FFX64lu98PCEEaj6UgZA==", null, false, "35af2b5c-b1af-4e76-b5fc-5467a30d4761", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "cfee4c28-e24a-4ab9-bb04-6d0e913b6015", "761cbfc1-84b8-4b07-a440-49cce4b966fe" });
        }
    }
}
