\# Aturan Proyek



Acuan lengkap tugas ada di docs/SPEC.md. Baca sebelum bekerja.



\## Arsitektur

\- AcademicApi: ASP.NET Core Web API + EF Core + SQLite

\- AcademicHybridApp: .NET MAUI Blazor Hybrid, HANYA akses data lewat HttpClient ke API

\- Dilarang mengakses database langsung dari AcademicHybridApp



\## Aturan kode

\- Gunakan DTO untuk request/response, jangan kembalikan entitas langsung

\- Validasi di backend wajib (DataAnnotations), frontend hanya pelengkap

\- Status HTTP: 400 data tidak valid, 404 tidak ditemukan, 409 duplikat / dosen masih jadi wali

\- Pesan error dalam JSON dan berbahasa Indonesia yang mudah dipahami

\- Penamaan jelas, beri komentar singkat pada logika penting

\- Jangan commit folder bin dan obj

\- Kerjakan satu langkah per permintaan, jangan mengubah file di luar yang diminta

