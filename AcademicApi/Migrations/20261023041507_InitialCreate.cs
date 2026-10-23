using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AcademicApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dosen",
                columns: table => new
                {
                    DosenId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nidn = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    NamaDosen = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Prodi = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dosen", x => x.DosenId);
                });

            migrationBuilder.CreateTable(
                name: "Mahasiswa",
                columns: table => new
                {
                    MahasiswaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nim = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    NamaMahasiswa = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Angkatan = table.Column<int>(type: "INTEGER", nullable: false),
                    DosenId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mahasiswa", x => x.MahasiswaId);
                    table.ForeignKey(
                        name: "FK_Mahasiswa_Dosen_DosenId",
                        column: x => x.DosenId,
                        principalTable: "Dosen",
                        principalColumn: "DosenId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Dosen",
                columns: new[] { "DosenId", "Email", "NamaDosen", "Nidn", "Prodi" },
                values: new object[,]
                {
                    { 1, "anita@kampus.ac.id", "Dr. Anita Wijaya", "0512345678", "Sistem Informasi" },
                    { 2, "budi@kampus.ac.id", "Dr. Budi Hartono", "0523456789", "Teknik Informatika" },
                    { 3, "citra@kampus.ac.id", "Dr. Citra Lestari", "0534567890", "Manajemen Informatika" }
                });

            migrationBuilder.InsertData(
                table: "Mahasiswa",
                columns: new[] { "MahasiswaId", "Angkatan", "DosenId", "Email", "NamaMahasiswa", "Nim" },
                values: new object[,]
                {
                    { 1, 2023, 1, "budi@student.kampus.ac.id", "Budi Santoso", "72230001" },
                    { 2, 2023, 1, "siti@student.kampus.ac.id", "Siti Aminah", "72230002" },
                    { 3, 2022, 2, "andi@student.kampus.ac.id", "Andi Pratama", "72230003" },
                    { 4, 2024, 2, "dewi@student.kampus.ac.id", "Dewi Kartika", "72230004" },
                    { 5, 2025, 3, "rizky@student.kampus.ac.id", "Rizky Ramadhan", "72230005" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dosen_Nidn",
                table: "Dosen",
                column: "Nidn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mahasiswa_DosenId",
                table: "Mahasiswa",
                column: "DosenId");

            migrationBuilder.CreateIndex(
                name: "IX_Mahasiswa_Nim",
                table: "Mahasiswa",
                column: "Nim",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mahasiswa");

            migrationBuilder.DropTable(
                name: "Dosen");
        }
    }
}
