UTS – Pemrograman Mobile - 1 

TUGAS PENGGANTI UTS 

Sistem Informasi Akademik Sederhana 

CRUD Data Mahasiswa dan Dosen 

Gambaran Umum Tugas 

Tugas milestone ini meminta mahasiswa membangun aplikasi akademik sederhana yang terdiri atas aplikasi 

klien Blazor Hybrid dan layanan backend ASP.NET Core Web API. Aplikasi digunakan untuk mengelola data 

dosen dan mahasiswa. Setiap mahasiswa memiliki satu dosen wali, sedangkan seorang dosen dapat menjadi 

wali bagi banyak mahasiswa. 

Skenario inti: petugas akademik dapat menambah, melihat, memperbarui, dan menghapus data dosen 

maupun mahasiswa melalui aplikasi Blazor Hybrid. Seluruh data disimpan dan diakses melalui REST API, 

bukan langsung dari aplikasi klien ke database. 

Tujuan Pembelajaran 

• Menerapkan pemisahan aplikasi frontend, backend, dan database dalam arsitektur client-server. 

• Membangun endpoint RESTful untuk operasi CRUD menggunakan ASP.NET Core Web API. 

• Menggunakan Entity Framework Core untuk pemetaan entitas, relasi, query, dan penyimpanan data. 

• Mengonsumsi Web API dari Blazor Hybrid menggunakan HttpClient dan service class. 

• Membangun form input yang memiliki validasi dan memberikan umpan balik kepada pengguna. 

• Mengelola relasi one-to-many antara dosen dan mahasiswa. 

• Melakukan pengujian dasar API melalui Swagger serta mendokumentasikan hasil aplikasi. 

Batasan dan Ruang Lingkup 

• Fokus tugas adalah CRUD dan integrasi frontend-backend. 

• Autentikasi, otorisasi, upload foto, dan deployment cloud tidak diwajibkan. 

• Aplikasi boleh menggunakan SQL Server atau SQLite. 

• Data harus diakses melalui Web API. Blazor Hybrid tidak diperbolehkan mengakses database secara 

langsung. 

• Tampilan harus berfungsi dan rapi, tetapi tidak harus memiliki desain visual yang kompleks. 

UTS Pemrograman Mobile 2026 

UTS – Pemrograman Mobile - 2 

UTS Pemrograman Mobile 2026 

Deskripsi Kasus 

Bagian Administrasi Akademik membutuhkan aplikasi sederhana untuk memelihara data dosen dan 

mahasiswa. Saat ini data masih dicatat secara terpisah sehingga pencarian dan pembaruan data kurang 

efisien. Aplikasi baru harus menyediakan dua modul utama: Modul Dosen dan Modul Mahasiswa. 

&#x20;

Modul Kemampuan Minimum 

Dosen Menampilkan daftar dosen, melihat detail, menambah, mengubah, dan menghapus data 

dosen. 

Mahasiswa Menampilkan daftar mahasiswa beserta dosen wali, melihat detail, menambah, mengubah, 

dan menghapus data mahasiswa. 

Integrasi Pilihan dosen wali pada form mahasiswa harus berasal dari data dosen yang tersedia melalui 

API. 

&#x20;

Model Data 

Entitas Dosen 

Atribut Tipe Data Ketentuan 

DosenId int Primary key, identity/auto increment. 

Nidn string Wajib, unik, maksimal 20 karakter. 

NamaDosen string Wajib, maksimal 100 karakter. 

Email string Wajib, format email valid, maksimal 100 karakter. 

Prodi string Wajib, maksimal 100 karakter. 

&#x20;

Entitas Mahasiswa 

Atribut Tipe Data Ketentuan 

MahasiswaId int Primary key, identity/auto increment. 

Nim string Wajib, unik, maksimal 20 karakter. 

NamaMahasiswa string Wajib, maksimal 100 karakter. 

Email string Wajib, format email valid, maksimal 100 karakter. 

Angkatan int Wajib, berada pada rentang yang ditentukan. 

DosenId int Foreign key, wajib merujuk ke dosen yang tersedia. 

UTS – Pemrograman Mobile - 3 

UTS Pemrograman Mobile 2026 

Relasi Data 

Dosen (1) -------------------- (N) Mahasiswa 

Dosen.DosenId  <--------------  Mahasiswa.DosenId 

&#x20;

Ketentuan penghapusan: data dosen yang masih digunakan sebagai dosen wali tidak boleh dihapus. API 

harus mengembalikan respons yang jelas agar pengguna memahami alasan kegagalan penghapusan. 

&#x20;

Arsitektur Aplikasi 

Blazor Hybrid (.NET MAUI Blazor) 

&#x20;       | 

&#x20;       | HTTP/JSON menggunakan HttpClient 

&#x20;       v 

ASP.NET Core Web API 

&#x20;       | 

&#x20;       | Entity Framework Core 

&#x20;       v 

SQL Server atau SQLite 

&#x20;

Lapisan Tanggung Jawab 

Blazor Hybrid Antarmuka pengguna, navigasi, form, validasi awal, pemanggilan API, dan pesan hasil 

operasi. 

Web API Endpoint CRUD, validasi request, aturan bisnis, status HTTP, dan pertukaran data JSON. 

EF Core Akses data, pemetaan entitas, relasi, migration, dan query database. 

Database Penyimpanan permanen data dosen dan mahasiswa. 

&#x20;

Spesifikasi Backend ASP.NET Core Web API 

Struktur Proyek Minimum 

AcademicApi/ 

├── Controllers/ 

│   ├── DosenController.cs 

│   └── MahasiswaController.cs 

├── Data/ 

│   └── AcademicDbContext.cs 

├── Models/ 

│   ├── Dosen.cs 

│   └── Mahasiswa.cs 

├── DTOs/                 (direkomendasikan) 

├── Services/             (opsional) 

├── Migrations/ 

├── appsettings.json 

└── Program.cs 

&#x20;

UTS – Pemrograman Mobile - 4 

UTS Pemrograman Mobile 2026 

Endpoint Dosen 

Metode Endpoint Fungsi Respons 

Sukses 

GET /api/dosen Mengambil seluruh dosen. 200 OK 

GET /api/dosen/{id} Mengambil satu dosen berdasarkan ID. 200 OK 

POST /api/dosen Menambahkan dosen baru. 201 Created 

PUT /api/dosen/{id} Memperbarui data dosen. 204 No Content / 

200 OK 

DELETE /api/dosen/{id} Menghapus dosen jika tidak menjadi dosen wali. 204 No Content 

&#x20;

Endpoint Mahasiswa 

Metode Endpoint Fungsi Respons 

Sukses 

GET /api/mahasiswa Mengambil mahasiswa beserta identitas dosen wali. 200 OK 

GET /api/mahasiswa/{id} Mengambil detail mahasiswa. 200 OK 

POST /api/mahasiswa Menambahkan mahasiswa. 201 Created 

PUT /api/mahasiswa/{id} Memperbarui mahasiswa dan dosen wali. 204 No Content / 

200 OK 

DELETE /api/mahasiswa/{id} Menghapus mahasiswa. 204 No Content 

&#x20;

Ketentuan Respons API 

• Gunakan status 400 Bad Request untuk data tidak valid. 

• Gunakan status 404 Not Found jika ID tidak ditemukan. 

• Gunakan status 409 Conflict untuk NIDN/NIM duplikat atau saat dosen masih digunakan sebagai dosen 

wali. 

• Gunakan JSON dengan pesan yang mudah dipahami untuk respons kegagalan. 

• Endpoint daftar mahasiswa harus memuat nama dosen wali tanpa menimbulkan perulangan objek JSON. 

Gunakan DTO atau proyeksi LINQ. 

&#x20;

Contoh Request dan Response 

POST /api/dosen 

Content-Type: application/json 

&#x20;

{ 

&#x20; "nidn": "0512345678", 

&#x20; "namaDosen": "Dr. Anita Wijaya", 

&#x20; "email": "anita@kampus.ac.id", 

UTS – Pemrograman Mobile - 5 

UTS Pemrograman Mobile 2026 

&#x20; "prodi": "Sistem Informasi" 

} 

&#x20;

POST /api/mahasiswa 

Content-Type: application/json 

&#x20;

{ 

&#x20; "nim": "72230001", 

&#x20; "namaMahasiswa": "Budi Santoso", 

&#x20; "email": "budi@student.kampus.ac.id", 

&#x20; "angkatan": 2023, 

&#x20; "dosenId": 1 

} 

&#x20;

GET /api/mahasiswa 

&#x20;

\[ 

&#x20; { 

&#x20;   "mahasiswaId": 1, 

&#x20;   "nim": "72230001", 

&#x20;   "namaMahasiswa": "Budi Santoso", 

&#x20;   "email": "budi@student.kampus.ac.id", 

&#x20;   "angkatan": 2023, 

&#x20;   "dosenId": 1, 

&#x20;   "namaDosenWali": "Dr. Anita Wijaya" 

&#x20; } 

] 

&#x20;

Spesifikasi Frontend Blazor Hybrid 

Struktur Proyek Minimum 

AcademicHybridApp/ 

├── Components/Pages/ 

│   ├── Dosen/ 

│   └── Mahasiswa/ 

├── Models/ 

├── Services/ 

│   ├── DosenService.cs 

│   └── MahasiswaService.cs 

├── Layout/ 

├── MauiProgram.cs 

└── wwwroot/ 

&#x20;

Navigasi 

• Beranda atau dashboard sederhana. 

• Menu Dosen untuk mengakses daftar dan form dosen. 

• Menu Mahasiswa untuk mengakses daftar dan form mahasiswa. 

UTS – Pemrograman Mobile - 6 

UTS Pemrograman Mobile 2026 

Halaman Dosen 

Halaman Kebutuhan 

Daftar Dosen Menampilkan NIDN, nama, email, prodi, serta tombol Edit dan Hapus. Tersedia tombol 

Tambah Dosen. 

Tambah Dosen Form NIDN, nama dosen, email, dan program studi. Menampilkan validasi sebelum 

pengiriman. 

Edit Dosen Memuat data berdasarkan ID, mengizinkan perubahan, lalu menyimpan melalui PUT. 

Hapus Dosen Meminta konfirmasi sebelum DELETE dan menampilkan pesan jika dosen masih menjadi 

wali. 

&#x20;

Halaman Mahasiswa 

Halaman Kebutuhan 

Daftar Mahasiswa Menampilkan NIM, nama, email, angkatan, nama dosen wali, serta tombol Edit dan Hapus. 

Tambah Mahasiswa Form NIM, nama, email, angkatan, dan dropdown dosen wali. Isi dropdown diperoleh dari 

API dosen. 

Edit Mahasiswa Memuat data berdasarkan ID. Dosen wali dapat diubah melalui dropdown. 

Hapus Mahasiswa Meminta konfirmasi, menjalankan DELETE, memperbarui daftar, dan menampilkan 

notifikasi. 

&#x20;

Perilaku Antarmuka 

• Tampilkan indikator loading ketika data sedang dimuat. 

• Tampilkan pesan jika data kosong atau API tidak dapat dihubungi. 

• Setelah tambah/edit/hapus berhasil, daftar data harus diperbarui. 

• Gunakan notifikasi atau alert yang membedakan operasi berhasil dan gagal. 

• Tombol Simpan sebaiknya dinonaktifkan selama request berlangsung untuk mencegah pengiriman ganda. 

&#x20;

Aturan Validasi dan Bisnis 

Objek Aturan 

Dosen NIDN, nama, email, dan prodi wajib diisi; email harus valid; NIDN tidak boleh duplikat. 

Mahasiswa NIM, nama, email, angkatan, dan dosen wali wajib diisi; email valid; NIM unik. 

Angkatan Nilai minimal 2020 dan maksimal mengikuti ketentuan tugas, misalnya 2026. 

Dosen Wali DosenId harus merujuk ke data dosen yang tersedia. 

Penghapusan Dosen Tidak diperbolehkan jika masih memiliki mahasiswa bimbingan. 

&#x20;

UTS – Pemrograman Mobile - 7 

Validasi minimal harus diterapkan pada backend. Validasi pada frontend berfungsi meningkatkan 

pengalaman pengguna, tetapi tidak menggantikan validasi server. 

Instruksi Pengerjaan 

1\. Buat solution yang memuat proyek ASP.NET Core Web API dan proyek .NET MAUI Blazor Hybrid. 

2\. Definisikan model Dosen dan Mahasiswa beserta relasi one-to-many. 

3\. Konfigurasikan DbContext, connection string, dan EF Core migration. 

4\. Siapkan minimal 3 data dosen dan 5 data mahasiswa untuk pengujian. 

5\. Implementasikan endpoint CRUD dosen dan uji seluruh endpoint melalui Swagger. 

6\. Implementasikan endpoint CRUD mahasiswa dan pastikan nama dosen wali muncul pada hasil GET. 

7\. Buat model/DTO serta service HttpClient pada frontend. 

8\. Bangun halaman daftar, tambah, edit, dan hapus untuk Dosen. 

9\. Bangun halaman daftar, tambah, edit, dan hapus untuk Mahasiswa. 

10\. Terapkan validasi, loading state, konfirmasi hapus, dan notifikasi hasil. 

11\. Uji integrasi end-to-end: data dari frontend tersimpan ke database melalui API. 

12\. Siapkan README, screenshot, dan video demonstrasi singkat. 

Berkas yang Dikumpulkan 

No. 

Deliverable 

Isi Minimum 

1 

Source code 

Solution backend dan frontend, tanpa folder bin dan obj. 

2 

Database 

EF Core migration dan/atau script database. 

3 

README.md 

Prasyarat, konfigurasi connection string/API base address, langkah menjalankan, 

dan akun/data uji jika ada. 

4 

Dokumentasi PDF/DOCX 

Deskripsi aplikasi, arsitektur, model data, endpoint, screenshot Swagger, dan 

screenshot aplikasi. 

5 

Video demo 

Durasi sekitar 3-5 menit yang memperlihatkan CRUD dosen, CRUD mahasiswa, 

validasi, dan relasi dosen wali. 

6 

Tautan repository 

Repository Git sesuai ketentuan dosen; commit menunjukkan progres pengerjaan. 

Skenario Pengujian Minimum 

ID 

Skenario 

T01 

Menambah dosen dengan seluruh data valid. 

Hasil yang Diharapkan 

Data tersimpan dan muncul pada daftar. 

T02 

Menambah dosen dengan email tidak valid. 

Penyimpanan ditolak dan pesan validasi tampil. 

T03 

Menambah dosen dengan NIDN yang sama. 

API menolak duplikasi. 

T04 

Menambah mahasiswa dan memilih dosen wali. 

UTS Pemrograman Mobile 2026 

Data tersimpan dengan relasi yang benar. 

UTS – Pemrograman Mobile - 8 

UTS Pemrograman Mobile 2026 

ID Skenario Hasil yang Diharapkan 

T05 Mengubah dosen wali mahasiswa. Daftar menampilkan dosen wali yang baru. 

T06 Menghapus mahasiswa. Data hilang dari daftar dan database. 

T07 Menghapus dosen yang masih menjadi wali. Operasi ditolak dengan pesan yang jelas. 

T08 Mengakses ID yang tidak tersedia. API mengembalikan 404 Not Found. 

T09 API tidak aktif saat frontend dibuka. Aplikasi menampilkan pesan kegagalan, tidak berhenti 

secara tidak terkontrol. 

&#x20;

Rubrik Penilaian 

Komponen Bobot Indikator 

CRUD Backend Dosen 15 Endpoint lengkap, validasi, status HTTP, dan penanganan data tidak 

ditemukan. 

CRUD Backend Mahasiswa 20 Endpoint lengkap, relasi, DTO/proyeksi, dan nama dosen wali pada 

response. 

Frontend Dosen 15 Daftar, tambah, edit, hapus, konfirmasi, dan notifikasi berfungsi. 

Frontend Mahasiswa 20 Daftar, form, dropdown dosen dari API, edit relasi, dan hapus berfungsi. 

Validasi \& Error Handling 10 Validasi frontend/backend, loading state, dan pesan kesalahan 

informatif. 

Database \& EF Core 8 Relasi dan migration benar; data persisten; constraint relevan. 

Kualitas Kode 7 Penamaan jelas, pemisahan service/model, tidak ada duplikasi 

berlebihan. 

Dokumentasi \& Demo 5 README lengkap, screenshot relevan, dan demo dapat diikuti. 

TOTAL 100  

&#x20;

Fitur Bonus (Maksimal 10 Poin) 

Poin bonus tidak menggantikan fungsi wajib dan nilai akhir mengikuti kebijakan dosen. 

Fitur Poin Maks. Kriteria 

Pencarian mahasiswa 3 Pencarian berdasarkan NIM atau nama, dijalankan melalui API 

atau filtering yang jelas. 

Dashboard ringkas 2 Menampilkan jumlah dosen dan mahasiswa dari data aktual. 

Pagination/sorting 2 Daftar dapat diurutkan atau dipaginasi dengan benar. 

Unit/integration test 3 Terdapat test bermakna untuk service/controller dan dapat 

dijalankan. 

Ketentuan Teknis dan Integritas Akademik 

UTS – Pemrograman Mobile - 9 

• Mahasiswa boleh menggunakan dokumentasi resmi dan alat bantu AI sesuai kebijakan mata kuliah, tetapi 

harus memahami serta mampu menjelaskan kode yang dikumpulkan. 

• Kode yang tidak dapat dijelaskan saat demonstrasi dapat memengaruhi penilaian. 

• Cantumkan sumber atau alat bantu utama yang digunakan pada README. 

• Jangan mengunggah password, API key, atau connection string rahasia ke repository publik. 

• Pastikan proyek dapat dibangun pada komputer lain dengan mengikuti README. 

Checklist Sebelum Pengumpulan 

• ☐ Backend dapat build dan berjalan. 

• ☐ Swagger menampilkan seluruh endpoint. 

• ☐ Migration dapat diterapkan pada database kosong. 

• ☐ Frontend dapat mengakses base URL API yang benar. 

• ☐ CRUD Dosen telah diuji. 

• ☐ CRUD Mahasiswa telah diuji. 

• ☐ Dropdown dosen wali berasal dari API. 

• ☐ Validasi dan error handling telah diuji. 

• ☐ Folder bin dan obj tidak disertakan. 

• ☐ README, screenshot, video, dan tautan repository telah tersedia. 

UTS Pemrograman Mobile 2026 

Lembar Penilaian Dosen 

UTS – Pemrograman Mobile - 10 

Komponen 

Maks. 

Nilai 

Catatan 

CRUD Backend Dosen 

15   

CRUD Backend Mahasiswa 

20 

Frontend Dosen 

15   

Frontend Mahasiswa 

20 

Validasi \& Error Handling 

10   

Database \& EF Core 

8 

Kualitas Kode 

7   

Dokumentasi \& Demo 

5 

Bonus 

10   

Nilai Akhir 

Komentar umum: 

\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_ 

\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_ 

\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_ 

\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_ 

UTS Pemrograman Mobile 2026 

