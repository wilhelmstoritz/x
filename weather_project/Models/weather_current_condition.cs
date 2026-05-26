namespace weather_project.Models;

public class weather_current_condition
{
    public string temp_C { get; set; } = string.Empty;

    public List<weather_desc> weatherDesc { get; set; } = new();

    public string windspeedKmph { get; set; } = string.Empty;

    public string humidity { get; set; } = string.Empty;

    public string FeelsLikeC { get; set; } = string.Empty;
}
