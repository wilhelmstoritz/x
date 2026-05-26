using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using people_project.Models;
using people_project.Services;

namespace people_project.Controllers;

public class HomeController : Controller
{
    private readonly people_store m_people_store;

    public HomeController(people_store t_people_store)
    {
        m_people_store = t_people_store;
    }

    public IActionResult Index(string? search_jmeno, string? search_prijmeni)
    {
        var l_people = m_people_store.get_all();
        if (!string.IsNullOrWhiteSpace(search_jmeno))
        {
            l_people = l_people.Where(t_person => t_person.jmeno.Contains(search_jmeno, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (!string.IsNullOrWhiteSpace(search_prijmeni))
        {
            l_people = l_people.Where(t_person => t_person.prijmeni.Contains(search_prijmeni, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        var l_view_model = new people_index_view_model
        {
            people = l_people,
            statistics = m_people_store.build_statistics(l_people),
            search_jmeno = search_jmeno ?? string.Empty,
            search_prijmeni = search_prijmeni ?? string.Empty
        };

        return View(l_view_model);
    }

    public IActionResult Details(int id)
    {
        var l_person = m_people_store.get_by_id(id);
        if (l_person == null)
        {
            return NotFound();
        }

        return View(l_person);
    }

    public IActionResult Statistics()
    {
        var l_people = m_people_store.get_all();
        return View(m_people_store.build_statistics(l_people));
    }

    public IActionResult Create()
    {
        return View(new person_item { datum_narozeni = DateOnly.FromDateTime(DateTime.Today) });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(person_item t_person)
    {
        if (!ModelState.IsValid)
        {
            return View(t_person);
        }

        m_people_store.add(t_person);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var l_person = m_people_store.get_by_id(id);
        if (l_person == null)
        {
            return NotFound();
        }

        return View(l_person);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(person_item t_person)
    {
        if (!ModelState.IsValid)
        {
            return View(t_person);
        }

        if (!m_people_store.update(t_person))
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Details), new { id = t_person.id });
    }

    public IActionResult Delete(int id)
    {
        var l_person = m_people_store.get_by_id(id);
        if (l_person == null)
        {
            return NotFound();
        }

        return View(l_person);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(person_item t_person)
    {
        m_people_store.delete(t_person.id);
        return RedirectToAction(nameof(Index));
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
