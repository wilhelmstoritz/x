namespace weather_project.Models;

public class weather_index_view_model
{
    public string city_name { get; set; } = string.Empty;

    public weather_api_response? weather { get; set; }

    public string? error_message { get; set; }
}
