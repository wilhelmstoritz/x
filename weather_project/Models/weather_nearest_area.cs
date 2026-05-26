namespace weather_project.Models;

public class weather_nearest_area
{
    public List<weather_area_name> areaName { get; set; } = new();

    public List<weather_area_region> region { get; set; } = new();

    public List<weather_area_country> country { get; set; } = new();
}
