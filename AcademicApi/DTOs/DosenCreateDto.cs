using System.ComponentModel.DataAnnotations;

namespace AcademicApi.DTOs;

public class DosenCreateDto
{
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
}
