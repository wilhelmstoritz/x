namespace weather_project.Models;

public class weather_location
{
    public string name { get; set; } = string.Empty;

    public string region { get; set; } = string.Empty;

    public string country { get; set; } = string.Empty;

    public double lat { get; set; }

    public double lon { get; set; }

    public string localtime { get; set; } = string.Empty;
}
