using System.Net.Http.Json;
using movies_project.Models;

namespace movies_project.Services;

public class movie_api_service
{
    private readonly HttpClient m_http_client;

    public movie_api_service(HttpClient t_http_client)
    {
        m_http_client = t_http_client;
    }

    public async Task<List<movie_item>> get_movies_async()
    {
        var l_movies = await m_http_client.GetFromJsonAsync<List<movie_item>>(string.Empty);
        return l_movies ?? new List<movie_item>();
    }

    public movie_statistics_view_model build_statistics(IEnumerable<movie_item> t_movies)
    {
        var l_movies = t_movies.ToList();
        return new movie_statistics_view_model
        {
            movies_count = l_movies.Count,
            average_score = l_movies.Count == 0 ? 0 : Math.Round(l_movies.Average(t_movie => t_movie.rt_score), 1),
            max_score = l_movies.Count == 0 ? 0 : l_movies.Max(t_movie => t_movie.rt_score),
            min_score = l_movies.Count == 0 ? 0 : l_movies.Min(t_movie => t_movie.rt_score),
            top_movie_title = l_movies.OrderByDescending(t_movie => t_movie.rt_score).FirstOrDefault()?.title ?? string.Empty
        };
    }

    public movie_item? get_by_id(IEnumerable<movie_item> t_movies, string t_id)
    {
        return t_movies.FirstOrDefault(t_movie => string.Equals(t_movie.id, t_id, StringComparison.OrdinalIgnoreCase));
    }
}
