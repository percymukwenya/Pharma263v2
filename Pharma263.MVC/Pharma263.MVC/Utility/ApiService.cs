using Microsoft.Extensions.Logging;
using Pharma263.Integration.Api.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pharma263.MVC.Utility
{
    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ApiService> _logger;
        private readonly ITokenService _tokenService;

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiService(IHttpClientFactory httpClientFactory, ITokenService tokenService, ILogger<ApiService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                using var client = CreateClient();
                var response = await client.GetAsync(endpoint);

                // Handle authentication failures gracefully - BaseController will redirect
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    _logger.LogWarning("Authentication failed for GET {Endpoint}. Status: {StatusCode}",
                        endpoint, response.StatusCode);
                    return default; // Return default instead of throwing
                }

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error in GetAsync for endpoint {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task<List<T>> GetAllAsync<T>(string endpoint)
        {
            try
            {
                using var client = CreateClient();
                var response = await client.GetAsync(endpoint);

                // Handle authentication failures gracefully - BaseController will redirect
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    _logger.LogWarning("Authentication failed for GET {Endpoint}. Status: {StatusCode}",
                        endpoint, response.StatusCode);
                    return default;
                }

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<T>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error in GetAllAsync for endpoint {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task<T> PostAsync<T>(string endpoint, T item)
        {
            try
            {
                using var client = CreateClient();

                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(item),
                    Encoding.UTF8,
                    "application/json");

                var response = await client.PostAsync(endpoint, jsonContent);

                // Handle authentication failures gracefully - BaseController will redirect
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    _logger.LogWarning("Authentication failed for POST {Endpoint}. Status: {StatusCode}",
                        endpoint, response.StatusCode);
                    return default;
                }

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error in PostAsync for endpoint {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task<T> PutAsync<T>(string endpoint, T item)
        {
            try
            {
                using var client = CreateClient();
                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(item),
                    Encoding.UTF8,
                    "application/json");

                var response = await client.PutAsync(endpoint, jsonContent);

                // Handle authentication failures gracefully - BaseController will redirect
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    _logger.LogWarning("Authentication failed for PUT {Endpoint}. Status: {StatusCode}",
                        endpoint, response.StatusCode);
                    return default;
                }

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error in PutAsync for endpoint {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                using var client = CreateClient();
                var response = await client.DeleteAsync(endpoint);

                // Handle authentication failures gracefully - BaseController will redirect
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    _logger.LogWarning("Authentication failed for DELETE {Endpoint}. Status: {StatusCode}",
                        endpoint, response.StatusCode);
                    return false;
                }

                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error in DeleteAsync for endpoint {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task<ApiResponse<T>> GetApiResponseAsync<T>(string endpoint)
        {
            try
            {
                using var client = CreateClient();
                var response = await client.GetAsync(endpoint);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var apiResponse = JsonSerializer.Deserialize<ApiResponse<T>>(responseContent, _jsonOptions);

                        return apiResponse;
                    }
                    catch
                    {
                        // If that fails, try to deserialize directly to T
                        try
                        {
                            var data = JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);

                            return ApiResponse<T>.CreateSuccess(data);
                        }
                        catch (Exception ex)
                        {
                            return ApiResponse<T>.CreateFailure($"Failed to deserialize response: {ex.Message}");
                        }
                    }
                }

                try
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<object>>(responseContent, _jsonOptions);
                    return ApiResponse<T>.CreateFailure(
                        errorResponse.Message ?? $"API error: {response.StatusCode}",
                        errorResponse.StatusCode,
                        errorResponse.Errors
                    );
                }
                catch
                {
                    return ApiResponse<T>.CreateFailure(
                        $"API error: {response.StatusCode}",
                        (int)response.StatusCode
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllAsync for endpoint {Endpoint}", endpoint);

                return new ApiResponse<T>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }        

        public async Task<ApiResponse<T>> PostApiResponseAsync<T>(string endpoint, object data)
        {
            try
            {
                using var client = CreateClient();
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Try to deserialize as ApiResponse<T>
                    try
                    {
                        var apiResponse = JsonSerializer.Deserialize<ApiResponse<T>>(responseContent, _jsonOptions);
                        return apiResponse;
                    }
                    catch
                    {
                        // If that fails, try to deserialize directly to T
                        try
                        {
                            var result = JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
                            return ApiResponse<T>.CreateSuccess(result);
                        }
                        catch (JsonException)
                        {
                            // For primitive types like bool, handle specially
                            if (typeof(T) == typeof(bool))
                            {
                                return ApiResponse<T>.CreateSuccess((T)(object)true);
                            }
                            throw;
                        }
                    }
                }

                // Try to get error details from response
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<object>>(responseContent, _jsonOptions);
                    return ApiResponse<T>.CreateFailure(
                        errorResponse.Message ?? $"API error: {response.StatusCode}",
                        errorResponse.StatusCode,
                        errorResponse.Errors
                    );
                }
                catch
                {
                    return ApiResponse<T>.CreateFailure(
                        $"API error: {response.StatusCode}",
                        (int)response.StatusCode
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PostApiResponseAsync for endpoint {Endpoint}", endpoint);

                return new ApiResponse<T>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<T>> PutApiResponseAsync<T>(string endpoint, object data)
        {
            try
            {
                using var client = CreateClient();
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var apiResponse = JsonSerializer.Deserialize<ApiResponse<T>>(responseContent, _jsonOptions);
                        return apiResponse;
                    }
                    catch
                    {
                        // If that fails, try to deserialize directly to T
                        try
                        {
                            var result = JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
                            return ApiResponse<T>.CreateSuccess(result);
                        }
                        catch (JsonException)
                        {
                            // For primitive types like bool, handle specially
                            if (typeof(T) == typeof(bool))
                            {
                                return ApiResponse<T>.CreateSuccess((T)(object)true);
                            }
                            throw;
                        }
                    }
                }

                try
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<object>>(responseContent, _jsonOptions);
                    return ApiResponse<T>.CreateFailure(
                        errorResponse.Message ?? $"API error: {response.StatusCode}",
                        errorResponse.StatusCode,
                        errorResponse.Errors
                    );
                }
                catch
                {
                    return ApiResponse<T>.CreateFailure(
                        $"API error: {response.StatusCode}",
                        (int)response.StatusCode
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PutApiResponseAsync for endpoint {Endpoint}", endpoint);

                return new ApiResponse<T>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<bool>> DeleteApiResponseAsync(string endpoint)
        {
            try
            {
                using var client = CreateClient();
                var response = await client.DeleteAsync(endpoint);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Try to deserialize as ApiResponse<bool>
                    try
                    {
                        var apiResponse = JsonSerializer.Deserialize<ApiResponse<bool>>(responseContent, _jsonOptions);
                        return apiResponse;
                    }
                    catch
                    {
                        // If the API returns a success status code, consider it successful
                        return ApiResponse<bool>.CreateSuccess(true);
                    }
                }

                // Try to get error details from response
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<object>>(responseContent, _jsonOptions);
                    return ApiResponse<bool>.CreateFailure(
                        errorResponse.Message ?? $"API error: {response.StatusCode}",
                        errorResponse.StatusCode,
                        errorResponse.Errors
                    );
                }
                catch
                {
                    return ApiResponse<bool>.CreateFailure(
                        $"API error: {response.StatusCode}",
                        (int)response.StatusCode
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in DeleteApiResponseAsync for endpoint {Endpoint}", endpoint);

                return new ApiResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient("PharmaApiClient");

            var token = _tokenService.GetToken();

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }
    }
}
