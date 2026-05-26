using System.Net.Http.Json;
using weather_project.Models;

namespace weather_project.Services;

public class weather_api_service
{
    private readonly HttpClient m_http_client;

    public weather_api_service(HttpClient t_http_client)
    {
        m_http_client = t_http_client;
    }

    public async Task<weather_api_response?> get_weather_async(string t_city_name)
    {
        return await m_http_client.GetFromJsonAsync<weather_api_response>($"{Uri.EscapeDataString(t_city_name)}?format=j1");
    }
}
