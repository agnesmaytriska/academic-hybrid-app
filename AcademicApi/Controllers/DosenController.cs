using AcademicApi.Data;
using AcademicApi.DTOs;
using AcademicApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademicApi.Controllers;

[ApiController]
[Route("api/dosen")]
public class DosenController : ControllerBase
{
    private readonly AcademicDbContext _context;

    public DosenController(AcademicDbContext context)
    {
        _context = context;
    }

    // GET: api/dosen
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DosenDto>>> GetAll()
    {
        var daftarDosen = await _context.DosenSet
            .AsNoTracking()
            .OrderBy(d => d.DosenId)
            .Select(d => new DosenDto
            {
                DosenId = d.DosenId,
                Nidn = d.Nidn,
                NamaDosen = d.NamaDosen,
                Email = d.Email,
                Prodi = d.Prodi
            })
            .ToListAsync();

        return Ok(daftarDosen);
    }

    // GET: api/dosen/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DosenDto>> GetById(int id)
    {
        var dosen = await _context.DosenSet
            .AsNoTracking()
            .Where(d => d.DosenId == id)
            .Select(d => new DosenDto
            {
                DosenId = d.DosenId,
                Nidn = d.Nidn,
                NamaDosen = d.NamaDosen,
                Email = d.Email,
                Prodi = d.Prodi
            })
            .FirstOrDefaultAsync();

        if (dosen is null)
        {
            return NotFound(new { message = $"Dosen dengan ID {id} tidak ditemukan." });
        }

        return Ok(dosen);
    }

    // POST: api/dosen
    [HttpPost]
    public async Task<ActionResult<DosenDto>> Create(DosenCreateDto request)
    {
        var nidn = request.Nidn.Trim();

        // NIDN harus unik
        if (await NidnSudahDigunakanAsync(nidn))
        {
            return Conflict(new { message = $"NIDN '{nidn}' sudah digunakan oleh dosen lain." });
        }

        var dosen = new Dosen
        {
            Nidn = nidn,
            NamaDosen = request.NamaDosen.Trim(),
            Email = request.Email.Trim(),
            Prodi = request.Prodi.Trim()
        };

        _context.DosenSet.Add(dosen);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // Pengaman terakhir jika unique index NIDN terpicu (mis. request bersamaan)
            return Conflict(new { message = "NIDN sudah digunakan oleh dosen lain." });
        }

        var response = new DosenDto
        {
            DosenId = dosen.DosenId,
            Nidn = dosen.Nidn,
            NamaDosen = dosen.NamaDosen,
            Email = dosen.Email,
            Prodi = dosen.Prodi
        };

        // 201 Created + header Location ke GET /api/dosen/{id}
        return CreatedAtAction(nameof(GetById), new { id = dosen.DosenId }, response);
    }

    // PUT: api/dosen/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, DosenUpdateDto request)
    {
        var dosen = await _context.DosenSet.FindAsync(id);

        if (dosen is null)
        {
            return NotFound(new { message = $"Dosen dengan ID {id} tidak ditemukan." });
        }

        var nidn = request.Nidn.Trim();

        // Cek duplikat, kecuali NIDN milik dosen ini sendiri
        if (await NidnSudahDigunakanAsync(nidn, id))
        {
            return Conflict(new { message = $"NIDN '{nidn}' sudah digunakan oleh dosen lain." });
        }

        dosen.Nidn = nidn;
        dosen.NamaDosen = request.NamaDosen.Trim();
        dosen.Email = request.Email.Trim();
        dosen.Prodi = request.Prodi.Trim();

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // Pengaman terakhir jika unique index NIDN terpicu (mis. request bersamaan)
            return Conflict(new { message = "NIDN sudah digunakan oleh dosen lain." });
        }

        return NoContent();
    }

    // DELETE: api/dosen/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var dosen = await _context.DosenSet.FindAsync(id);

        if (dosen is null)
        {
            return NotFound(new { message = $"Dosen dengan ID {id} tidak ditemukan." });
        }

        // Dosen yang masih menjadi wali tidak boleh dihapus
        var jumlahMahasiswa = await _context.MahasiswaSet
            .CountAsync(m => m.DosenId == id);

        if (jumlahMahasiswa > 0)
        {
            return Conflict(new
            {
                message = $"Dosen tidak dapat dihapus karena masih menjadi dosen wali {jumlahMahasiswa} mahasiswa."
            });
        }

        _context.DosenSet.Remove(dosen);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Helper: cek NIDN duplikat; kecualiDosenId dipakai saat PUT
    private async Task<bool> NidnSudahDigunakanAsync(string nidn, int? kecualiDosenId = null)
    {
        var query = _context.DosenSet.Where(d => d.Nidn == nidn);

        if (kecualiDosenId.HasValue)
        {
            query = query.Where(d => d.DosenId != kecualiDosenId.Value);
        }

        return await query.AnyAsync();
    }
}
