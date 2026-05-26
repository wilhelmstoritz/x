namespace movies_project.Models;

public class movies_index_view_model
{
    public List<movie_item> movies { get; set; } = new();

    public string search_title { get; set; } = string.Empty;
}
