using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HospitalInventoryManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductRequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRequests_Hospitals_HospitalID",
                table: "ProductRequests");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "58ce0768-dd8d-4938-a613-bdcc13f9bcc6");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "77329c7b-a1f0-44cc-a3dc-95feb933b1f9", "4e637bfa-408e-4711-9498-59705a4094de" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "77329c7b-a1f0-44cc-a3dc-95feb933b1f9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4e637bfa-408e-4711-9498-59705a4094de");

            migrationBuilder.DropColumn(
                name: "LotNumber",
                table: "ProductRequests");

            migrationBuilder.AlterColumn<int>(
                name: "HospitalID",
                table: "ProductRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRequests_Hospitals_HospitalID",
                table: "ProductRequests",
                column: "HospitalID",
                principalTable: "Hospitals",
                principalColumn: "HospitalID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRequests_Hospitals_HospitalID",
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

            migrationBuilder.AlterColumn<int>(
                name: "HospitalID",
                table: "ProductRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "LotNumber",
                table: "ProductRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "58ce0768-dd8d-4938-a613-bdcc13f9bcc6", null, "User", "USER" },
                    { "77329c7b-a1f0-44cc-a3dc-95feb933b1f9", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "HospitalID", "IsAdmin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "4e637bfa-408e-4711-9498-59705a4094de", 0, "c43d61de-1052-4448-b905-507e32dfa97e", "admin@ozemmedikal.com", true, 0, false, false, null, "ADMIN@OZEMEDIKAL.COM", "ADMIN@OZEMMEDIKAL.COM", "AQAAAAIAAYagAAAAEJBwHw1ZZO7CmOYGULcdvOg/oESMXPjEjD+tSDITpvAI3I7v6QYP4/gGukAJttcH/g==", null, false, "69e3853f-d4c0-4bfb-a664-b204b3340d92", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "77329c7b-a1f0-44cc-a3dc-95feb933b1f9", "4e637bfa-408e-4711-9498-59705a4094de" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRequests_Hospitals_HospitalID",
                table: "ProductRequests",
                column: "HospitalID",
                principalTable: "Hospitals",
                principalColumn: "HospitalID",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
