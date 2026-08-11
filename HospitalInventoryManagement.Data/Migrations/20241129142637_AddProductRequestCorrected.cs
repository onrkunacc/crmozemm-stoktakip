using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HospitalInventoryManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductRequestCorrected : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "30037a32-38d3-4fb4-8ab5-9dc451200636");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "7a52e32e-03c7-40e8-a36c-9f316ce3de9d", "62a058db-3985-4e4d-9f71-d83f137c44d0" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7a52e32e-03c7-40e8-a36c-9f316ce3de9d");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62a058db-3985-4e4d-9f71-d83f137c44d0");

            migrationBuilder.CreateTable(
                name: "ProductRequests",
                columns: table => new
                {
                    ProductRequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LotNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestedByUserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HospitalID = table.Column<int>(type: "int", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminComments = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRequests", x => x.ProductRequestID);
                    table.ForeignKey(
                        name: "FK_ProductRequests_AspNetUsers_RequestedByUserID",
                        column: x => x.RequestedByUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductRequests_Hospitals_HospitalID",
                        column: x => x.HospitalID,
                        principalTable: "Hospitals",
                        principalColumn: "HospitalID",
                        onDelete: ReferentialAction.SetNull);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ProductRequests_HospitalID",
                table: "ProductRequests",
                column: "HospitalID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRequests_RequestedByUserID",
                table: "ProductRequests",
                column: "RequestedByUserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductRequests");

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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "30037a32-38d3-4fb4-8ab5-9dc451200636", null, "User", "USER" },
                    { "7a52e32e-03c7-40e8-a36c-9f316ce3de9d", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "HospitalID", "IsAdmin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "62a058db-3985-4e4d-9f71-d83f137c44d0", 0, "e8807462-e114-4478-9884-deba35694076", "admin@ozemmedikal.com", true, 0, false, false, null, "ADMIN@OZEMEDIKAL.COM", "ADMIN@OZEMMEDIKAL.COM", "AQAAAAIAAYagAAAAEERvZcumV7QRWRLZkmZiR2M+ptez7eKSLdm5vUhiEGf3JG/kkgbbTVcsOuTIwW4gOA==", null, false, "4a0eeca9-1f89-4c08-9be2-0b80b19b4693", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "7a52e32e-03c7-40e8-a36c-9f316ce3de9d", "62a058db-3985-4e4d-9f71-d83f137c44d0" });
        }
    }
}
