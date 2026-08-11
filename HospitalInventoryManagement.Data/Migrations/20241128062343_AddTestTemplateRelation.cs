using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HospitalInventoryManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTestTemplateRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4c77a585-e1ee-40ea-a439-fd7f732e623c");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "95141cc7-67c4-4fb0-913d-7f2d58c896ba", "bd22e009-aafe-46fd-abeb-bbb95cc0bb5d" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "95141cc7-67c4-4fb0-913d-7f2d58c896ba");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bd22e009-aafe-46fd-abeb-bbb95cc0bb5d");

            migrationBuilder.CreateTable(
                name: "TestTemplates",
                columns: table => new
                {
                    TestTemplateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HospitalID = table.Column<int>(type: "int", nullable: false),
                    TestName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestTemplates", x => x.TestTemplateID);
                    table.ForeignKey(
                        name: "FK_TestTemplates_Hospitals_HospitalID",
                        column: x => x.HospitalID,
                        principalTable: "Hospitals",
                        principalColumn: "HospitalID",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_TestTemplates_HospitalID",
                table: "TestTemplates",
                column: "HospitalID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestTemplates");

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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4c77a585-e1ee-40ea-a439-fd7f732e623c", null, "User", "USER" },
                    { "95141cc7-67c4-4fb0-913d-7f2d58c896ba", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "HospitalID", "IsAdmin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "bd22e009-aafe-46fd-abeb-bbb95cc0bb5d", 0, "21d26c65-44e1-4b86-8b46-4c4be8b8d02e", "admin@ozemmedikal.com", true, 0, false, false, null, "ADMIN@OZEMEDIKAL.COM", "ADMIN@OZEMMEDIKAL.COM", "AQAAAAIAAYagAAAAEOXVBlH26UiLtTb+tuSKMunNLqE7LCFhWAM7wmOEh8NeLYfKxQ+jU3S0LXMDnNa8Kw==", null, false, "c421807a-618d-4842-acd3-c07744dffb78", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "95141cc7-67c4-4fb0-913d-7f2d58c896ba", "bd22e009-aafe-46fd-abeb-bbb95cc0bb5d" });
        }
    }
}
