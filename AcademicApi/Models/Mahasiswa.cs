using System.ComponentModel.DataAnnotations;

namespace AcademicApi.Models;

public class Mahasiswa
{
    public int MahasiswaId { get; set; }

    [Required(ErrorMessage = "NIM wajib diisi.")]
    [MaxLength(20, ErrorMessage = "NIM maksimal 20 karakter.")]
    public string Nim { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nama mahasiswa wajib diisi.")]
    [MaxLength(100, ErrorMessage = "Nama mahasiswa maksimal 100 karakter.")]
    public string NamaMahasiswa { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email wajib diisi.")]
    [EmailAddress(ErrorMessage = "Format email tidak valid.")]
    [MaxLength(100, ErrorMessage = "Email maksimal 100 karakter.")]
    public string Email { get; set; } = string.Empty;

    [Range(2020, 2026, ErrorMessage = "Angkatan harus berada di antara 2020 dan 2026.")]
    public int Angkatan { get; set; }

    // [Required] tidak efektif untuk int; gunakan Range agar nilai 0 ditolak
    [Range(1, int.MaxValue, ErrorMessage = "Dosen wali wajib dipilih.")]
    public int DosenId { get; set; }

    // Navigasi ke dosen wali
    public Dosen? Dosen { get; set; }
}
