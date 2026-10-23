using AcademicApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Samakan format error validasi otomatis menjadi { "message": "..." }
        options.InvalidModelStateResponseFactory = context =>
        {
            var pesanValidasi = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .ToList();

            var message = pesanValidasi.Count > 0
                ? string.Join(" ", pesanValidasi)
                : "Data yang dikirim tidak valid.";

            return new BadRequestObjectResult(new { message });
        };
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AcademicDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Redirect ke HTTPS hanya di luar Development agar profil http tetap bisa dipakai saat lokal
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
