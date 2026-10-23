using AcademicApi.Data;
using AcademicApi.DTOs;
using AcademicApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademicApi.Controllers;

[ApiController]
[Route("api/mahasiswa")]
public class MahasiswaController : ControllerBase
{
    private readonly AcademicDbContext _context;

    public MahasiswaController(AcademicDbContext context)
    {
        _context = context;
    }

    // GET: api/mahasiswa
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MahasiswaDto>>> GetAll()
    {
        var daftarMahasiswa = await _context.MahasiswaSet
            .AsNoTracking()
            .OrderBy(m => m.MahasiswaId)
            .Select(m => new MahasiswaDto
            {
                MahasiswaId = m.MahasiswaId,
                Nim = m.Nim,
                NamaMahasiswa = m.NamaMahasiswa,
                Email = m.Email,
                Angkatan = m.Angkatan,
                DosenId = m.DosenId,
                // Proyeksi nama dosen wali; tidak mengembalikan objek Dosen
                NamaDosenWali = m.Dosen != null ? m.Dosen.NamaDosen : string.Empty
            })
            .ToListAsync();

        return Ok(daftarMahasiswa);
    }

    // GET: api/mahasiswa/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MahasiswaDto>> GetById(int id)
    {
        var mahasiswa = await _context.MahasiswaSet
            .AsNoTracking()
            .Where(m => m.MahasiswaId == id)
            .Select(m => new MahasiswaDto
            {
                MahasiswaId = m.MahasiswaId,
                Nim = m.Nim,
                NamaMahasiswa = m.NamaMahasiswa,
                Email = m.Email,
                Angkatan = m.Angkatan,
                DosenId = m.DosenId,
                NamaDosenWali = m.Dosen != null ? m.Dosen.NamaDosen : string.Empty
            })
            .FirstOrDefaultAsync();

        if (mahasiswa is null)
        {
            return NotFound(new { message = $"Mahasiswa dengan ID {id} tidak ditemukan." });
        }

        return Ok(mahasiswa);
    }

    // POST: api/mahasiswa
    [HttpPost]
    public async Task<ActionResult<MahasiswaDto>> Create(MahasiswaCreateDto request)
    {
        var nim = request.Nim.Trim();

        // NIM harus unik
        if (await NimSudahDigunakanAsync(nim))
        {
            return Conflict(new { message = $"NIM '{nim}' sudah digunakan oleh mahasiswa lain." });
        }

        // Dosen wali wajib merujuk ke dosen yang ada; sekaligus ambil namanya
        var namaDosenWali = await CariNamaDosenAsync(request.DosenId);

        if (namaDosenWali is null)
        {
            return BadRequest(new { message = "Dosen wali tidak ditemukan." });
        }

        var mahasiswa = new Mahasiswa
        {
            Nim = nim,
            NamaMahasiswa = request.NamaMahasiswa.Trim(),
            Email = request.Email.Trim(),
            Angkatan = request.Angkatan,
            DosenId = request.DosenId
        };

        _context.MahasiswaSet.Add(mahasiswa);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // Pengaman terakhir jika unique index NIM atau foreign key terpicu
            return Conflict(new
            {
                message = "Data tidak dapat disimpan. Pastikan NIM belum digunakan dan dosen wali valid."
            });
        }

        var response = new MahasiswaDto
        {
            MahasiswaId = mahasiswa.MahasiswaId,
            Nim = mahasiswa.Nim,
            NamaMahasiswa = mahasiswa.NamaMahasiswa,
            Email = mahasiswa.Email,
            Angkatan = mahasiswa.Angkatan,
            DosenId = mahasiswa.DosenId,
            NamaDosenWali = namaDosenWali
        };

        // 201 Created + header Location ke GET /api/mahasiswa/{id}
        return CreatedAtAction(nameof(GetById), new { id = mahasiswa.MahasiswaId }, response);
    }

    // PUT: api/mahasiswa/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, MahasiswaUpdateDto request)
    {
        var mahasiswa = await _context.MahasiswaSet.FindAsync(id);

        if (mahasiswa is null)
        {
            return NotFound(new { message = $"Mahasiswa dengan ID {id} tidak ditemukan." });
        }

        var nim = request.Nim.Trim();

        // Cek duplikat, kecuali NIM milik mahasiswa ini sendiri
        if (await NimSudahDigunakanAsync(nim, id))
        {
            return Conflict(new { message = $"NIM '{nim}' sudah digunakan oleh mahasiswa lain." });
        }

        // Dosen wali wajib ada
        if (await CariNamaDosenAsync(request.DosenId) is null)
        {
            return BadRequest(new { message = "Dosen wali tidak ditemukan." });
        }

        mahasiswa.Nim = nim;
        mahasiswa.NamaMahasiswa = request.NamaMahasiswa.Trim();
        mahasiswa.Email = request.Email.Trim();
        mahasiswa.Angkatan = request.Angkatan;
        mahasiswa.DosenId = request.DosenId;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // Pengaman terakhir jika unique index NIM atau foreign key terpicu
            return Conflict(new
            {
                message = "Data tidak dapat disimpan. Pastikan NIM belum digunakan dan dosen wali valid."
            });
        }

        return NoContent();
    }

    // DELETE: api/mahasiswa/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var mahasiswa = await _context.MahasiswaSet.FindAsync(id);

        if (mahasiswa is null)
        {
            return NotFound(new { message = $"Mahasiswa dengan ID {id} tidak ditemukan." });
        }

        _context.MahasiswaSet.Remove(mahasiswa);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Helper: cek NIM duplikat; kecualiMahasiswaId dipakai saat PUT
    private async Task<bool> NimSudahDigunakanAsync(string nim, int? kecualiMahasiswaId = null)
    {
        var query = _context.MahasiswaSet.Where(m => m.Nim == nim);

        if (kecualiMahasiswaId.HasValue)
        {
            query = query.Where(m => m.MahasiswaId != kecualiMahasiswaId.Value);
        }

        return await query.AnyAsync();
    }

    // Helper: ambil nama dosen wali; null berarti DosenId tidak ditemukan
    private async Task<string?> CariNamaDosenAsync(int dosenId)
    {
        return await _context.DosenSet
            .AsNoTracking()
            .Where(d => d.DosenId == dosenId)
            .Select(d => d.NamaDosen)
            .FirstOrDefaultAsync();
    }
}
