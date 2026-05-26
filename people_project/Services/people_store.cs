using System.Text.Json;
using people_project.Models;

namespace people_project.Services;

public class people_store
{
    private readonly string m_data_file_path;
    private static readonly JsonSerializerOptions g_json_options = new()
    {
        WriteIndented = true
    };

    public people_store(IWebHostEnvironment t_environment)
    {
        var l_data_directory = Path.Combine(t_environment.ContentRootPath, "Data");
        Directory.CreateDirectory(l_data_directory);
        m_data_file_path = Path.Combine(l_data_directory, "people_data.json");
        ensure_seed_data();
    }

    public List<person_item> get_all()
    {
        return load_all();
    }

    public person_item? get_by_id(int t_id)
    {
        return load_all().FirstOrDefault(t_item => t_item.id == t_id);
    }

    public void add(person_item t_item)
    {
        var l_items = load_all();
        t_item.id = l_items.Count == 0 ? 1 : l_items.Max(t_person => t_person.id) + 1;
        l_items.Add(t_item);
        save_all(l_items);
    }

    public bool update(person_item t_item)
    {
        var l_items = load_all();
        var l_existing_item = l_items.FirstOrDefault(t_person => t_person.id == t_item.id);
        if (l_existing_item == null)
        {
            return false;
        }

        l_existing_item.jmeno = t_item.jmeno;
        l_existing_item.prijmeni = t_item.prijmeni;
        l_existing_item.datum_narozeni = t_item.datum_narozeni;
        l_existing_item.vyska_cm = t_item.vyska_cm;
        l_existing_item.vaha_kg = t_item.vaha_kg;
        l_existing_item.mesto = t_item.mesto;
        save_all(l_items);
        return true;
    }

    public bool delete(int t_id)
    {
        var l_items = load_all();
        var l_removed = l_items.RemoveAll(t_person => t_person.id == t_id);
        if (l_removed == 0)
        {
            return false;
        }

        save_all(l_items);
        return true;
    }

    public people_statistics_view_model build_statistics(IEnumerable<person_item> t_items)
    {
        var l_items = t_items.ToList();
        var l_today = DateOnly.FromDateTime(DateTime.Today);
        var l_average_age = l_items.Count == 0 ? 0 : l_items.Average(t_person => l_today.Year - t_person.datum_narozeni.Year - (l_today.DayOfYear < t_person.datum_narozeni.DayOfYear ? 1 : 0));

        return new people_statistics_view_model
        {
            pocet_lidi = l_items.Count,
            prumerny_vek = Math.Round((decimal)l_average_age, 1),
            prumerna_vyska_cm = l_items.Count == 0 ? 0 : Math.Round(l_items.Average(t_person => (double)t_person.vyska_cm), 1),
            maximalni_vyska_cm = l_items.Count == 0 ? 0 : l_items.Max(t_person => t_person.vyska_cm),
            prumerna_vaha_kg = l_items.Count == 0 ? 0 : Math.Round(l_items.Average(t_person => (double)t_person.vaha_kg), 1),
            maximalni_vaha_kg = l_items.Count == 0 ? 0 : l_items.Max(t_person => t_person.vaha_kg)
        };
    }

    private List<person_item> load_all()
    {
        if (!File.Exists(m_data_file_path))
        {
            ensure_seed_data();
        }

        var l_json = File.ReadAllText(m_data_file_path);
        var l_items = JsonSerializer.Deserialize<List<person_item>>(l_json, g_json_options);
        return l_items ?? new List<person_item>();
    }

    private void save_all(List<person_item> t_items)
    {
        var l_json = JsonSerializer.Serialize(t_items, g_json_options);
        File.WriteAllText(m_data_file_path, l_json);
    }

    private void ensure_seed_data()
    {
        if (File.Exists(m_data_file_path))
        {
            return;
        }

        var l_seed_items = new List<person_item>
        {
            new() { id = 1, jmeno = "jana", prijmeni = "novakova", datum_narozeni = new DateOnly(1998, 4, 12), vyska_cm = 168, vaha_kg = 58.5m, mesto = "praha" },
            new() { id = 2, jmeno = "petr", prijmeni = "svoboda", datum_narozeni = new DateOnly(1995, 11, 3), vyska_cm = 181, vaha_kg = 79.2m, mesto = "brno" },
            new() { id = 3, jmeno = "eva", prijmeni = "dvorakova", datum_narozeni = new DateOnly(2001, 7, 21), vyska_cm = 172, vaha_kg = 61.4m, mesto = "ostrava" },
            new() { id = 4, jmeno = "martin", prijmeni = "prochazka", datum_narozeni = new DateOnly(1992, 2, 9), vyska_cm = 189, vaha_kg = 88.1m, mesto = "plzen" },
            new() { id = 5, jmeno = "lucie", prijmeni = "cerna", datum_narozeni = new DateOnly(2003, 9, 15), vyska_cm = 165, vaha_kg = 54.7m, mesto = "liberec" },
            new() { id = 6, jmeno = "tomas", prijmeni = "vesely", datum_narozeni = new DateOnly(1990, 12, 30), vyska_cm = 176, vaha_kg = 74.9m, mesto = "olomouc" },
            new() { id = 7, jmeno = "barbora", prijmeni = "kralova", datum_narozeni = new DateOnly(1997, 6, 5), vyska_cm = 170, vaha_kg = 59.8m, mesto = "hradec_kralove" },
            new() { id = 8, jmeno = "jakub", prijmeni = "horak", datum_narozeni = new DateOnly(1988, 1, 18), vyska_cm = 193, vaha_kg = 92.3m, mesto = "zlin" },
            new() { id = 9, jmeno = "veronika", prijmeni = "mareckova", datum_narozeni = new DateOnly(2000, 5, 27), vyska_cm = 160, vaha_kg = 50.2m, mesto = "pardubice" },
            new() { id = 10, jmeno = "filip", prijmeni = "urban", datum_narozeni = new DateOnly(1993, 8, 8), vyska_cm = 185, vaha_kg = 83.6m, mesto = "ceske_budejovice" }
        };

        save_all(l_seed_items);
    }
}
