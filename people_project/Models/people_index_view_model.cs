using System.ComponentModel.DataAnnotations;

namespace people_project.Models;

public class people_index_view_model
{
    public List<person_item> people { get; set; } = new();

    public people_statistics_view_model statistics { get; set; } = new();

    [Display(Name = "jmeno")]
    public string search_jmeno { get; set; } = string.Empty;

    [Display(Name = "prijmeni")]
    public string search_prijmeni { get; set; } = string.Empty;
}
