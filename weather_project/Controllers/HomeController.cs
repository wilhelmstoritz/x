using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using weather_project.Models;
using weather_project.Services;

namespace weather_project.Controllers;

public class HomeController : Controller
{
    private readonly weather_api_service m_weather_api_service;

    public HomeController(weather_api_service t_weather_api_service)
    {
        m_weather_api_service = t_weather_api_service;
    }

    public async Task<IActionResult> Index(string? city_name = "prague")
    {
        var l_view_model = new weather_index_view_model
        {
            city_name = string.IsNullOrWhiteSpace(city_name) ? "prague" : city_name
        };

        try
        {
            l_view_model.weather = await m_weather_api_service.get_weather_async(l_view_model.city_name);
        }
        catch (Exception t_exception)
        {
            l_view_model.error_message = t_exception.Message;
        }

        return View(l_view_model);
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
