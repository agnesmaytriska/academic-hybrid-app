using System.Net.Http.Json;
using System.Text.Json;
using AcademicHybridApp.Models;

namespace AcademicHybridApp.Services;

public class MahasiswaService
{
    private const string Endpoint = "api/mahasiswa";

    private readonly HttpClient _httpClient;

    public MahasiswaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ServiceResult<List<MahasiswaModel>>> GetAllAsync()
    {
        try
        {
            using var response = await _httpClient.GetAsync(Endpoint);

            if (!response.IsSuccessStatusCode)
            {
                var message = await ApiErrorReader.ReadMessageAsync(response);
                return ServiceResult<List<MahasiswaModel>>.Fail(message);
            }

            var data = await response.Content.ReadFromJsonAsync<List<MahasiswaModel>>(ApiJson.Options);
            return ServiceResult<List<MahasiswaModel>>.Ok(data ?? new List<MahasiswaModel>());
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return ServiceResult<List<MahasiswaModel>>.Fail(ApiErrorMessages.Timeout);
        }
        catch (OperationCanceledException)
        {
            return ServiceResult<List<MahasiswaModel>>.Fail(ApiErrorMessages.Cancelled);
        }
        catch (HttpRequestException)
        {
            return ServiceResult<List<MahasiswaModel>>.Fail(ApiErrorMessages.Connection);
        }
        catch (JsonException)
        {
            return ServiceResult<List<MahasiswaModel>>.Fail(ApiErrorMessages.InvalidJson);
        }
        catch (Exception)
        {
            return ServiceResult<List<MahasiswaModel>>.Fail(ApiErrorMessages.Unexpected);
        }
    }

    public async Task<ServiceResult<MahasiswaModel>> GetByIdAsync(int id)
    {
        try
        {
            using var response = await _httpClient.GetAsync($"{Endpoint}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var message = await ApiErrorReader.ReadMessageAsync(response);
                return ServiceResult<MahasiswaModel>.Fail(message);
            }

            var data = await response.Content.ReadFromJsonAsync<MahasiswaModel>(ApiJson.Options);

            return data is null
                ? ServiceResult<MahasiswaModel>.Fail("Data mahasiswa tidak ditemukan pada respons API.")
                : ServiceResult<MahasiswaModel>.Ok(data);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return ServiceResult<MahasiswaModel>.Fail(ApiErrorMessages.Timeout);
        }
        catch (OperationCanceledException)
        {
            return ServiceResult<MahasiswaModel>.Fail(ApiErrorMessages.Cancelled);
        }
        catch (HttpRequestException)
        {
            return ServiceResult<MahasiswaModel>.Fail(ApiErrorMessages.Connection);
        }
        catch (JsonException)
        {
            return ServiceResult<MahasiswaModel>.Fail(ApiErrorMessages.InvalidJson);
        }
        catch (Exception)
        {
            return ServiceResult<MahasiswaModel>.Fail(ApiErrorMessages.Unexpected);
        }
    }

    public async Task<ServiceResult<MahasiswaModel>> CreateAsync(MahasiswaFormModel form)
    {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync(Endpoint, form, ApiJson.Options);

            if (!response.IsSuccessStatusCode)
            {
                var message = await ApiErrorReader.ReadMessageAsync(response);
                return ServiceResult<MahasiswaModel>.Fail(message);
            }

            // POST API mengembalikan 201 Created + body MahasiswaDto.
            var data = await response.Content.ReadFromJsonAsync<MahasiswaModel>(ApiJson.Options);

            return data is null
                ? ServiceResult<MahasiswaModel>.Fail("Data mahasiswa tidak ditemukan pada respons API.")
                : ServiceResult<MahasiswaModel>.Ok(data);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return ServiceResult<MahasiswaModel>.Fail(ApiErrorMessages.Timeout);
        }
        catch (OperationCanceledException)
        {
            return ServiceResult<MahasiswaModel>.Fail(ApiErrorMessages.Cancelled);
        }
        catch (HttpRequestException)
        {
            return ServiceResult<MahasiswaModel>.Fail(ApiErrorMessages.Connection);
        }
        catch (JsonException)
        {
            return ServiceResult<MahasiswaModel>.Fail(ApiErrorMessages.InvalidJson);
        }
        catch (Exception)
        {
            return ServiceResult<MahasiswaModel>.Fail(ApiErrorMessages.Unexpected);
        }
    }

    public async Task<ServiceResult<bool>> UpdateAsync(int id, MahasiswaFormModel form)
    {
        try
        {
            using var response = await _httpClient.PutAsJsonAsync($"{Endpoint}/{id}", form, ApiJson.Options);

            if (!response.IsSuccessStatusCode)
            {
                var message = await ApiErrorReader.ReadMessageAsync(response);
                return ServiceResult<bool>.Fail(message);
            }

            // PUT API mengembalikan 204 No Content.
            return ServiceResult<bool>.Ok(true);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return ServiceResult<bool>.Fail(ApiErrorMessages.Timeout);
        }
        catch (OperationCanceledException)
        {
            return ServiceResult<bool>.Fail(ApiErrorMessages.Cancelled);
        }
        catch (HttpRequestException)
        {
            return ServiceResult<bool>.Fail(ApiErrorMessages.Connection);
        }
        catch (JsonException)
        {
            return ServiceResult<bool>.Fail(ApiErrorMessages.InvalidJson);
        }
        catch (Exception)
        {
            return ServiceResult<bool>.Fail(ApiErrorMessages.Unexpected);
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        try
        {
            using var response = await _httpClient.DeleteAsync($"{Endpoint}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var message = await ApiErrorReader.ReadMessageAsync(response);
                return ServiceResult<bool>.Fail(message);
            }

            // DELETE API mengembalikan 204 No Content.
            return ServiceResult<bool>.Ok(true);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return ServiceResult<bool>.Fail(ApiErrorMessages.Timeout);
        }
        catch (OperationCanceledException)
        {
            return ServiceResult<bool>.Fail(ApiErrorMessages.Cancelled);
        }
        catch (HttpRequestException)
        {
            return ServiceResult<bool>.Fail(ApiErrorMessages.Connection);
        }
        catch (JsonException)
        {
            return ServiceResult<bool>.Fail(ApiErrorMessages.InvalidJson);
        }
        catch (Exception)
        {
            return ServiceResult<bool>.Fail(ApiErrorMessages.Unexpected);
        }
    }
}
