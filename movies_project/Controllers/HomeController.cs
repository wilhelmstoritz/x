using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using movies_project.Models;
using movies_project.Services;

namespace movies_project.Controllers;

public class HomeController : Controller
{
    private readonly movie_api_service m_movie_api_service;

    public HomeController(movie_api_service t_movie_api_service)
    {
        m_movie_api_service = t_movie_api_service;
    }

    public async Task<IActionResult> Index(string? search_title)
    {
        var l_movies = await m_movie_api_service.get_movies_async();
        if (!string.IsNullOrWhiteSpace(search_title))
        {
            l_movies = l_movies.Where(t_movie => t_movie.title.Contains(search_title, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        var l_view_model = new movies_index_view_model
        {
            movies = l_movies,
            search_title = search_title ?? string.Empty
        };

        return View(l_view_model);
    }

    public async Task<IActionResult> Details(string id)
    {
        var l_movies = await m_movie_api_service.get_movies_async();
        var l_movie = m_movie_api_service.get_by_id(l_movies, id);
        if (l_movie == null)
        {
            return NotFound();
        }

        return View(l_movie);
    }

    public async Task<IActionResult> Statistics()
    {
        var l_movies = await m_movie_api_service.get_movies_async();
        return View(m_movie_api_service.build_statistics(l_movies));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
