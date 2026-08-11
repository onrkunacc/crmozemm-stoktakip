using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HospitalInventoryManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRequests_Hospitals_HospitalID",
                table: "ProductRequests");

            migrationBuilder.DropIndex(
                name: "IX_ProductRequests_HospitalID",
                table: "ProductRequests");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7a1a3346-2879-4a60-8010-373f23a02311");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "53304951-5464-4576-af53-9793c290bfc9", "292c6e43-54b7-4fa9-9967-ffc952375f88" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "53304951-5464-4576-af53-9793c290bfc9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "292c6e43-54b7-4fa9-9967-ffc952375f88");

            migrationBuilder.DropColumn(
                name: "HospitalID",
                table: "ProductRequests");

            migrationBuilder.AlterColumn<string>(
                name: "AdminComments",
                table: "ProductRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "AdminComments",
                table: "ProductRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HospitalID",
                table: "ProductRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "53304951-5464-4576-af53-9793c290bfc9", null, "Admin", "ADMIN" },
                    { "7a1a3346-2879-4a60-8010-373f23a02311", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "HospitalID", "IsAdmin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "292c6e43-54b7-4fa9-9967-ffc952375f88", 0, "b7bc5993-1427-4f0d-8a44-af4c1fec6343", "admin@ozemmedikal.com", true, 0, false, false, null, "ADMIN@OZEMEDIKAL.COM", "ADMIN@OZEMMEDIKAL.COM", "AQAAAAIAAYagAAAAEGMM9/bbm5yZHtftb3dUlg03pQAxMA4kSnPrLnOZ1aH+q5uIuUqoX/FlhaqbYx9t+Q==", null, false, "c4a2da9b-7329-4728-98e4-c55a5a0a4168", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "53304951-5464-4576-af53-9793c290bfc9", "292c6e43-54b7-4fa9-9967-ffc952375f88" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductRequests_HospitalID",
                table: "ProductRequests",
                column: "HospitalID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRequests_Hospitals_HospitalID",
                table: "ProductRequests",
                column: "HospitalID",
                principalTable: "Hospitals",
                principalColumn: "HospitalID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
