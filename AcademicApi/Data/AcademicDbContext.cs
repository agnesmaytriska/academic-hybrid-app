using AcademicApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AcademicApi.Data;

public class AcademicDbContext : DbContext
{
    public AcademicDbContext(DbContextOptions<AcademicDbContext> options)
        : base(options)
    {
    }

    public DbSet<Dosen> DosenSet => Set<Dosen>();
    public DbSet<Mahasiswa> MahasiswaSet => Set<Mahasiswa>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ===== Konfigurasi tabel Dosen =====
        modelBuilder.Entity<Dosen>(entity =>
        {
            entity.ToTable("Dosen");
            entity.HasKey(d => d.DosenId);
            entity.Property(d => d.DosenId).ValueGeneratedOnAdd();

            entity.Property(d => d.Nidn).IsRequired().HasMaxLength(20);
            entity.HasIndex(d => d.Nidn).IsUnique(); // NIDN tidak boleh duplikat

            entity.Property(d => d.NamaDosen).IsRequired().HasMaxLength(100);
            entity.Property(d => d.Email).IsRequired().HasMaxLength(100);
            entity.Property(d => d.Prodi).IsRequired().HasMaxLength(100);
        });

        // ===== Konfigurasi tabel Mahasiswa =====
        modelBuilder.Entity<Mahasiswa>(entity =>
        {
            entity.ToTable("Mahasiswa");
            entity.HasKey(m => m.MahasiswaId);
            entity.Property(m => m.MahasiswaId).ValueGeneratedOnAdd();

            entity.Property(m => m.Nim).IsRequired().HasMaxLength(20);
            entity.HasIndex(m => m.Nim).IsUnique(); // NIM tidak boleh duplikat

            entity.Property(m => m.NamaMahasiswa).IsRequired().HasMaxLength(100);
            entity.Property(m => m.Email).IsRequired().HasMaxLength(100);
            entity.Property(m => m.Angkatan).IsRequired();

            // Relasi 1-N; Restrict mencegah mahasiswa ikut terhapus saat dosen dihapus
            entity.HasOne(m => m.Dosen)
                  .WithMany(d => d.Mahasiswas)
                  .HasForeignKey(m => m.DosenId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ===== Seed data awal untuk pengujian (3 dosen, 5 mahasiswa) =====
        modelBuilder.Entity<Dosen>().HasData(
            new Dosen
            {
                DosenId = 1,
                Nidn = "0512345678",
                NamaDosen = "Dr. Anita Wijaya",
                Email = "anita@kampus.ac.id",
                Prodi = "Sistem Informasi"
            },
            new Dosen
            {
                DosenId = 2,
                Nidn = "0523456789",
                NamaDosen = "Dr. Budi Hartono",
                Email = "budi@kampus.ac.id",
                Prodi = "Teknik Informatika"
            },
            new Dosen
            {
                DosenId = 3,
                Nidn = "0534567890",
                NamaDosen = "Dr. Citra Lestari",
                Email = "citra@kampus.ac.id",
                Prodi = "Manajemen Informatika"
            }
        );

        modelBuilder.Entity<Mahasiswa>().HasData(
            new Mahasiswa
            {
                MahasiswaId = 1,
                Nim = "72230001",
                NamaMahasiswa = "Budi Santoso",
                Email = "budi@student.kampus.ac.id",
                Angkatan = 2023,
                DosenId = 1
            },
            new Mahasiswa
            {
                MahasiswaId = 2,
                Nim = "72230002",
                NamaMahasiswa = "Siti Aminah",
                Email = "siti@student.kampus.ac.id",
                Angkatan = 2023,
                DosenId = 1
            },
            new Mahasiswa
            {
                MahasiswaId = 3,
                Nim = "72230003",
                NamaMahasiswa = "Andi Pratama",
                Email = "andi@student.kampus.ac.id",
                Angkatan = 2022,
                DosenId = 2
            },
            new Mahasiswa
            {
                MahasiswaId = 4,
                Nim = "72230004",
                NamaMahasiswa = "Dewi Kartika",
                Email = "dewi@student.kampus.ac.id",
                Angkatan = 2024,
                DosenId = 2
            },
            new Mahasiswa
            {
                MahasiswaId = 5,
                Nim = "72230005",
                NamaMahasiswa = "Rizky Ramadhan",
                Email = "rizky@student.kampus.ac.id",
                Angkatan = 2025,
                DosenId = 3
            }
        );
    }
}
