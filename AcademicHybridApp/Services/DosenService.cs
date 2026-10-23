using System.Net.Http.Json;
using System.Text.Json;
using AcademicHybridApp.Models;

namespace AcademicHybridApp.Services;

public class DosenService
{
    private const string Endpoint = "api/dosen";

    private readonly HttpClient _httpClient;

    public DosenService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ServiceResult<List<DosenModel>>> GetAllAsync()
    {
        try
        {
            using var response = await _httpClient.GetAsync(Endpoint);

            if (!response.IsSuccessStatusCode)
            {
                var message = await ApiErrorReader.ReadMessageAsync(response);
                return ServiceResult<List<DosenModel>>.Fail(message);
            }

            var data = await response.Content.ReadFromJsonAsync<List<DosenModel>>(ApiJson.Options);
            return ServiceResult<List<DosenModel>>.Ok(data ?? new List<DosenModel>());
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return ServiceResult<List<DosenModel>>.Fail(ApiErrorMessages.Timeout);
        }
        catch (OperationCanceledException)
        {
            return ServiceResult<List<DosenModel>>.Fail(ApiErrorMessages.Cancelled);
        }
        catch (HttpRequestException)
        {
            return ServiceResult<List<DosenModel>>.Fail(ApiErrorMessages.Connection);
        }
        catch (JsonException)
        {
            return ServiceResult<List<DosenModel>>.Fail(ApiErrorMessages.InvalidJson);
        }
        catch (Exception)
        {
            return ServiceResult<List<DosenModel>>.Fail(ApiErrorMessages.Unexpected);
        }
    }

    public async Task<ServiceResult<DosenModel>> GetByIdAsync(int id)
    {
        try
        {
            using var response = await _httpClient.GetAsync($"{Endpoint}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var message = await ApiErrorReader.ReadMessageAsync(response);
                return ServiceResult<DosenModel>.Fail(message);
            }

            var data = await response.Content.ReadFromJsonAsync<DosenModel>(ApiJson.Options);

            return data is null
                ? ServiceResult<DosenModel>.Fail("Data dosen tidak ditemukan pada respons API.")
                : ServiceResult<DosenModel>.Ok(data);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return ServiceResult<DosenModel>.Fail(ApiErrorMessages.Timeout);
        }
        catch (OperationCanceledException)
        {
            return ServiceResult<DosenModel>.Fail(ApiErrorMessages.Cancelled);
        }
        catch (HttpRequestException)
        {
            return ServiceResult<DosenModel>.Fail(ApiErrorMessages.Connection);
        }
        catch (JsonException)
        {
            return ServiceResult<DosenModel>.Fail(ApiErrorMessages.InvalidJson);
        }
        catch (Exception)
        {
            return ServiceResult<DosenModel>.Fail(ApiErrorMessages.Unexpected);
        }
    }

    public async Task<ServiceResult<DosenModel>> CreateAsync(DosenFormModel form)
    {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync(Endpoint, form, ApiJson.Options);

            if (!response.IsSuccessStatusCode)
            {
                var message = await ApiErrorReader.ReadMessageAsync(response);
                return ServiceResult<DosenModel>.Fail(message);
            }

            // POST API mengembalikan 201 Created + body DosenDto.
            var data = await response.Content.ReadFromJsonAsync<DosenModel>(ApiJson.Options);

            return data is null
                ? ServiceResult<DosenModel>.Fail("Data dosen tidak ditemukan pada respons API.")
                : ServiceResult<DosenModel>.Ok(data);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return ServiceResult<DosenModel>.Fail(ApiErrorMessages.Timeout);
        }
        catch (OperationCanceledException)
        {
            return ServiceResult<DosenModel>.Fail(ApiErrorMessages.Cancelled);
        }
        catch (HttpRequestException)
        {
            return ServiceResult<DosenModel>.Fail(ApiErrorMessages.Connection);
        }
        catch (JsonException)
        {
            return ServiceResult<DosenModel>.Fail(ApiErrorMessages.InvalidJson);
        }
        catch (Exception)
        {
            return ServiceResult<DosenModel>.Fail(ApiErrorMessages.Unexpected);
        }
    }

    public async Task<ServiceResult<bool>> UpdateAsync(int id, DosenFormModel form)
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
