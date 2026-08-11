using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HospitalInventoryManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateMessageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "67fb74af-d377-413e-a073-e02ec639512f", null, "Admin", "ADMIN" },
                    { "c2b38052-109c-43b3-ad5f-71ac33366cc4", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "HospitalID", "IsAdmin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "3a09ea40-c159-4d98-bd40-18c7a6628bbb", 0, "a98e23e4-27ae-45c6-a04e-9e358f860d6c", "admin@ozemmedikal.com", true, 0, false, false, null, "ADMIN@OZEMEDIKAL.COM", "ADMIN@OZEMMEDIKAL.COM", "AQAAAAIAAYagAAAAEBKnSiAwbV3jlz5y8sGBaRPPrY/81z45AcQf7mFvzF/KUe3/aTllDgwWkb5JTx7B+Q==", null, false, "573fc24a-07e9-464b-991c-6a7dc8811698", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "67fb74af-d377-413e-a073-e02ec639512f", "3a09ea40-c159-4d98-bd40-18c7a6628bbb" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c2b38052-109c-43b3-ad5f-71ac33366cc4");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "67fb74af-d377-413e-a073-e02ec639512f", "3a09ea40-c159-4d98-bd40-18c7a6628bbb" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "67fb74af-d377-413e-a073-e02ec639512f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3a09ea40-c159-4d98-bd40-18c7a6628bbb");

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
    }
}
