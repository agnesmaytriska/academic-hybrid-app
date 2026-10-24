# Sistem Informasi Akademik Sederhana

Aplikasi untuk mengelola data **dosen** dan **mahasiswa** (CRUD). Setiap mahasiswa
memiliki satu dosen wali, dan seorang dosen dapat menjadi wali bagi banyak
mahasiswa (relasi one-to-many).

Solution terdiri dari dua proyek:

| Proyek | Teknologi | Peran |
|---|---|---|
| `AcademicApi` | ASP.NET Core Web API, EF Core | Backend REST API, validasi, aturan bisnis, akses database |
| `AcademicHybridApp` | .NET MAUI Blazor Hybrid | Aplikasi klien: antarmuka, form, validasi awal, pemanggilan API |

## Arsitektur

```
Blazor Hybrid (.NET MAUI)
        |
        |  HTTP/JSON (HttpClient + service class)
        v
ASP.NET Core Web API
        |
        |  Entity Framework Core
        v
Database ([SQLite / SQL Server])
```

Aplikasi klien **tidak** mengakses database secara langsung. Seluruh data
diambil dan disimpan melalui REST API.

## Fitur

- CRUD Dosen: daftar, tambah, ubah, hapus.
- CRUD Mahasiswa: daftar beserta nama dosen wali, tambah, ubah, hapus.
- Dropdown dosen wali pada form mahasiswa diisi dari API dosen.
- Beranda berisi ringkasan jumlah dosen dan mahasiswa dari data aktual.
- Validasi di frontend (DataAnnotations) dan backend (sumber validasi utama).
- Indikator loading, pesan data kosong, dan pesan saat API tidak dapat dihubungi
  lengkap dengan tombol **Coba lagi**.
- Konfirmasi sebelum menghapus dan notifikasi berhasil/gagal.
- Tombol Simpan dinonaktifkan selama request berlangsung (mencegah kirim ganda).

## Aturan Data dan Bisnis

| Objek | Aturan |
|---|---|
| Dosen | NIDN, nama, email, prodi wajib; email valid; NIDN unik (maks. 20 karakter) |
| Mahasiswa | NIM, nama, email, angkatan, dosen wali wajib; email valid; NIM unik |
| Angkatan | 2020 sampai 2026 |
| Dosen wali | `DosenId` harus merujuk ke dosen yang ada |
| Hapus dosen | Ditolak (409) jika masih menjadi dosen wali mahasiswa |

## Endpoint API

### Dosen

| Metode | Endpoint | Fungsi | Respons sukses |
|---|---|---|---|
| GET | `/api/dosen` | Semua dosen | 200 |
| GET | `/api/dosen/{id}` | Satu dosen | 200 |
| POST | `/api/dosen` | Tambah dosen | 201 |
| PUT | `/api/dosen/{id}` | Ubah dosen | 204 / 200 |
| DELETE | `/api/dosen/{id}` | Hapus dosen (jika bukan wali) | 204 |

### Mahasiswa

| Metode | Endpoint | Fungsi | Respons sukses |
|---|---|---|---|
| GET | `/api/mahasiswa` | Semua mahasiswa beserta nama dosen wali | 200 |
| GET | `/api/mahasiswa/{id}` | Detail mahasiswa | 200 |
| POST | `/api/mahasiswa` | Tambah mahasiswa | 201 |
| PUT | `/api/mahasiswa/{id}` | Ubah mahasiswa dan dosen wali | 204 / 200 |
| DELETE | `/api/mahasiswa/{id}` | Hapus mahasiswa | 204 |

### Status kegagalan

| Status | Arti |
|---|---|
| 400 Bad Request | Data tidak valid |
| 404 Not Found | ID tidak ditemukan |
| 409 Conflict | NIDN/NIM duplikat, atau dosen masih menjadi dosen wali |

Respons kegagalan berupa JSON dengan pesan yang dapat dibaca pengguna.

## Prasyarat

- Windows 10/11 (target aplikasi: `net10.0-windows10.0.19041.0`)
- [.NET SDK 10](https://dotnet.microsoft.com/download)
- Workload .NET MAUI:
  ```bash
  dotnet workload install maui
  ```
- EF Core CLI:
  ```bash
  dotnet tool install --global dotnet-ef
  ```
- [Hanya jika memakai SQL Server] SQL Server atau LocalDB terpasang.

## Konfigurasi

### 1. Connection string (API)

Buka `AcademicApi/appsettings.json` dan sesuaikan:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "[isi sesuai database yang dipakai]"
  }
}
```

Contoh SQLite: `Data Source=academic.db`

Contoh SQL Server (LocalDB):
`Server=(localdb)\\MSSQLLocalDB;Database=AcademicDb;Trusted_Connection=True;TrustServerCertificate=True`

> Jangan menaruh password atau connection string rahasia di repository publik.

### 2. Alamat API (klien)

Aplikasi klien memakai base address untuk memanggil API. Lokasinya di
`AcademicHybridApp/MauiProgram.cs` (pendaftaran `HttpClient`/service).
Samakan alamat tersebut dengan alamat API saat dijalankan, misalnya:

```
https://localhost:[port-api]/
```

Port dapat dilihat pada keluaran terminal saat API dijalankan atau pada
`AcademicApi/Properties/launchSettings.json`.

## Menjalankan

Urutan penting: **database, lalu API, lalu aplikasi**.

```bash
# 1. Terapkan migration (membuat database kosong beserta data awal)
cd AcademicApi
dotnet ef database update

# 2. Jalankan API
dotnet run
# Swagger tersedia di: https://localhost:[port-api]/swagger
```

Biarkan API tetap berjalan, lalu di terminal lain:

```bash
# 3. Jalankan aplikasi klien
cd AcademicHybridApp
dotnet build -f net10.0-windows10.0.19041.0
dotnet run -f net10.0-windows10.0.19041.0
```

Alternatif: buka solution di Visual Studio, atur **Multiple startup projects**
(`AcademicApi` dan `AcademicHybridApp`), lalu jalankan.

Jika file `.exe` terkunci saat build ulang, tutup aplikasi yang sedang berjalan
terlebih dahulu.

## Data Uji

Migration menyertakan data awal: **4 dosen** dan **6 mahasiswa**.
Tidak ada akun login (autentikasi tidak termasuk ruang lingkup tugas).

## Skenario Pengujian

| ID | Skenario | Hasil yang diharapkan |
|---|---|---|
| T01 | Tambah dosen dengan data valid | Data tersimpan dan muncul di daftar |
| T02 | Tambah dosen dengan email tidak valid | Ditolak, pesan validasi tampil |
| T03 | Tambah dosen dengan NIDN yang sama | API menolak (409) |
| T04 | Tambah mahasiswa dan pilih dosen wali | Tersimpan dengan relasi yang benar |
| T05 | Ubah dosen wali mahasiswa | Daftar menampilkan dosen wali baru |
| T06 | Hapus mahasiswa | Hilang dari daftar dan database |
| T07 | Hapus dosen yang masih menjadi wali | Ditolak dengan pesan yang jelas |
| T08 | Akses ID yang tidak tersedia | API mengembalikan 404 |
| T09 | API tidak aktif saat aplikasi dibuka | Pesan kegagalan tampil, aplikasi tidak crash |

## Struktur Proyek

```
AcademicApi/
├── Controllers/        DosenController, MahasiswaController
├── Data/               AcademicDbContext
├── Models/             Dosen, Mahasiswa
├── DTOs/               DTO request dan response
├── Migrations/         Migration EF Core
├── appsettings.json
└── Program.cs

AcademicHybridApp/
├── Components/
│   ├── Layout/         MainLayout, NavMenu
│   ├── Pages/
│   │   ├── Dosen/      DosenList, DosenForm
│   │   ├── Mahasiswa/  MahasiswaList, MahasiswaForm
│   │   ├── Home.razor  Beranda / dashboard
│   │   └── NotFound.razor
│   └── Shared/         NotificationAlert, PageHeader, EmptyState, LoadErrorAlert
├── Models/             Model dan form model
├── Services/           DosenService, MahasiswaService
├── MauiProgram.cs
└── wwwroot/
```

## Alat Bantu yang Digunakan

- **Claude (Anthropic)**: perencanaan, peninjauan kode, dan penyusunan dokumentasi.
- **opencode**: membantu penulisan kode berdasarkan rencana yang telah ditinjau.
- Dokumentasi resmi Microsoft (ASP.NET Core, EF Core, Blazor, .NET MAUI) dan Bootstrap.

Seluruh kode telah ditinjau, diuji, dan dapat dijelaskan oleh penulis.

## Catatan

- Folder `bin/` dan `obj/` tidak disertakan dalam pengumpulan.
- Autentikasi, otorisasi, upload foto, dan deployment cloud tidak termasuk ruang lingkup tugas.

## Penulis

[Nama] - [NIM] - [Mata kuliah / kelas]
