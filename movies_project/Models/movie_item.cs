namespace movies_project.Models;

public class movie_item
{
    public string id { get; set; } = string.Empty;

    public string title { get; set; } = string.Empty;

    public string original_title { get; set; } = string.Empty;

    public string original_title_romanised { get; set; } = string.Empty;

    public string description { get; set; } = string.Empty;

    public string director { get; set; } = string.Empty;

    public string producer { get; set; } = string.Empty;

    public string release_date { get; set; } = string.Empty;

    public string running_time { get; set; } = string.Empty;

    public int rt_score { get; set; }
}
