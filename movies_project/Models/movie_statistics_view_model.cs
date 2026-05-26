namespace movies_project.Models;

public class movie_statistics_view_model
{
    public int movies_count { get; set; }

    public double average_score { get; set; }

    public int max_score { get; set; }

    public int min_score { get; set; }

    public string top_movie_title { get; set; } = string.Empty;
}
