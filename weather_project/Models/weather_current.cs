namespace weather_project.Models;

public class weather_current
{
    public decimal temp_c { get; set; }

    public int is_day { get; set; }

    public weather_condition condition { get; set; } = new();

    public decimal wind_kph { get; set; }

    public int humidity { get; set; }
}
