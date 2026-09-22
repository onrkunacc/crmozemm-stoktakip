using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HospitalInventoryManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class Asama_SayimDonemi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // NOT: AspNetRoles/AspNetUsers/AspNetUserRoles ile ilgili tüm DeleteData/InsertData
            // satırları BİLEREK kaldırıldı (seed GUID'leri her migration'da değiştiği için
            // "duplicate key (ADMIN)" hatasına yol açıyordu). Bu migration sadece Sayım özelliğini kurar.

            migrationBuilder.AddColumn<int>(
                name: "SayimID",
                table: "Stocks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sayims",
                columns: table => new
                {
                    SayimID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HospitalID = table.Column<int>(type: "int", nullable: false),
                    SayimNo = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sayims", x => x.SayimID);
                    table.ForeignKey(
                        name: "FK_Sayims_Hospitals_HospitalID",
                        column: x => x.HospitalID,
                        principalTable: "Hospitals",
                        principalColumn: "HospitalID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_SayimID",
                table: "Stocks",
                column: "SayimID");

            migrationBuilder.CreateIndex(
                name: "IX_Sayims_HospitalID",
                table: "Sayims",
                column: "HospitalID");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Sayims_SayimID",
                table: "Stocks",
                column: "SayimID",
                principalTable: "Sayims",
                principalColumn: "SayimID",
                onDelete: ReferentialAction.Restrict);

            // Backfill: eldeki stoğu hastane başına "Sayım #1 (aktif)"e bağla
            migrationBuilder.Sql(@"
                INSERT INTO Sayims (HospitalID, SayimNo, CreatedDate, IsActive)
                SELECT DISTINCT s.HospitalID, 1, GETDATE(), 1
                FROM Stocks s
                WHERE NOT EXISTS (SELECT 1 FROM Sayims z WHERE z.HospitalID = s.HospitalID);

                UPDATE s
                SET s.SayimID = z.SayimID
                FROM Stocks s
                INNER JOIN Sayims z ON z.HospitalID = s.HospitalID AND z.SayimNo = 1
                WHERE s.SayimID IS NULL;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Sayims_SayimID",
                table: "Stocks");

            migrationBuilder.DropTable(
                name: "Sayims");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_SayimID",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "SayimID",
                table: "Stocks");
        }
    }
}