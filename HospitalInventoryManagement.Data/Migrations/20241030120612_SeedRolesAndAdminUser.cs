using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HospitalInventoryManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedRolesAndAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "11e6dd87-57ee-4fce-a9cc-c331dbade1e9", null, "Admin", "ADMIN" },
                    { "178d5616-548c-4977-aaef-51889082b0a5", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "HospitalID", "IsAdmin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c4cb48a6-e7e8-4b20-a3a1-2cce0e424b85", 0, "36fe1c11-7374-472f-9bf6-ca6b8c6331dd", "admin@ozemmedikal.com", true, null, false, false, null, "ADMIN@OZEMEDIKAL.COM", "ADMIN@OZEMMEDIKAL.COM", "AQAAAAIAAYagAAAAEMMRGw5DHLcJCOJmYSMSNJ2kN2j9Xj7D5+r3dH3YzmWiA0ZiRW42852WvwO+IewQJA==", null, false, "433ce0da-ebad-4fbd-83fa-74556c2a4f51", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "11e6dd87-57ee-4fce-a9cc-c331dbade1e9", "c4cb48a6-e7e8-4b20-a3a1-2cce0e424b85" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "178d5616-548c-4977-aaef-51889082b0a5");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "11e6dd87-57ee-4fce-a9cc-c331dbade1e9", "c4cb48a6-e7e8-4b20-a3a1-2cce0e424b85" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "11e6dd87-57ee-4fce-a9cc-c331dbade1e9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c4cb48a6-e7e8-4b20-a3a1-2cce0e424b85");
        }
    }
}
