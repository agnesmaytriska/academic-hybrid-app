using System.Text.Json;

namespace AcademicHybridApp.Services;

public static class ApiJson
{
    // Opsi JSON bersama untuk semua service dan pembaca error API.
    public static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };
}
