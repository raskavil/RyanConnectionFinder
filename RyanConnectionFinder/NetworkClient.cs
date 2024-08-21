using System.Text.Json;
namespace RyanConnectionFinder;

public static class NetworkClient
{
    private static readonly HttpClient HttpClient = new HttpClient();

    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        { PropertyNameCaseInsensitive = true };

    public static async Task<T?> GetDataAsync<T>(string url)
    {
        try
        {
            var response = await HttpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(responseBody, Options);

        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
            return default;
        }
        catch (JsonException e)
        {
            Console.WriteLine($"JSON parse error: {e.Message}");
            return default;
        }
    }
}