namespace people_project.Models;

public class person_item
{
    public int id { get; set; }

    public string jmeno { get; set; } = string.Empty;

    public string prijmeni { get; set; } = string.Empty;

    public DateOnly datum_narozeni { get; set; }

    public int vyska_cm { get; set; }

    public decimal vaha_kg { get; set; }

    public string mesto { get; set; } = string.Empty;
}
