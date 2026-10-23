using System.Net.Http.Json;
using System.Text.Json;

namespace AcademicHybridApp.Services;

public sealed class ServiceResult<T>
{
    public bool IsSuccess { get; init; }
    public T Data { get; init; } = default!;
    public string? Error { get; init; }

    public static ServiceResult<T> Ok(T data) =>
        new() { IsSuccess = true, Data = data };

    public static ServiceResult<T> Fail(string error) =>
        new() { IsSuccess = false, Error = error };
}

internal sealed class ApiErrorResponse
{
    public string? Message { get; set; }
}

internal static class ApiErrorReader
{
    public static async Task<string> ReadMessageAsync(HttpResponseMessage response)
    {
        try
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>(ApiJson.Options);

            if (!string.IsNullOrWhiteSpace(error?.Message))
            {
                return error.Message;
            }
        }
        catch (JsonException)
        {
            // Bukan JSON yang valid; gunakan pesan cadangan.
        }
        catch (NotSupportedException)
        {
            // Content-Type tidak didukung; gunakan pesan cadangan.
        }

        return $"Terjadi kesalahan pada server API ({(int)response.StatusCode}).";
    }
}

internal static class ApiErrorMessages
{
    public const string Timeout = "Permintaan ke server API melebihi batas waktu. Silakan coba lagi.";
    public const string Cancelled = "Permintaan dibatalkan.";
    public const string Connection = "Tidak dapat terhubung ke server API. Pastikan API sedang berjalan.";
    public const string InvalidJson = "Respons dari server API tidak dapat dibaca.";
    public const string Unexpected = "Terjadi kesalahan tak terduga saat menghubungi server API.";
}
