using System.ComponentModel.DataAnnotations;

namespace AcademicApi.Models;

public class Dosen
{
    public int DosenId { get; set; }

    [Required(ErrorMessage = "NIDN wajib diisi.")]
    [MaxLength(20, ErrorMessage = "NIDN maksimal 20 karakter.")]
    public string Nidn { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nama dosen wajib diisi.")]
    [MaxLength(100, ErrorMessage = "Nama dosen maksimal 100 karakter.")]
    public string NamaDosen { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email wajib diisi.")]
    [EmailAddress(ErrorMessage = "Format email tidak valid.")]
    [MaxLength(100, ErrorMessage = "Email maksimal 100 karakter.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Program studi wajib diisi.")]
    [MaxLength(100, ErrorMessage = "Program studi maksimal 100 karakter.")]
    public string Prodi { get; set; } = string.Empty;

    // Relasi one-to-many: satu dosen membimbing banyak mahasiswa
    public ICollection<Mahasiswa> Mahasiswas { get; set; } = new List<Mahasiswa>();
}
