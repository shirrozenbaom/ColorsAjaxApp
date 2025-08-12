using System.ComponentModel.DataAnnotations;

namespace ColorsAjaxApp.Models;

public class ColorItem
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string Name { get; set; } = "";

    [Range(0, 100000)]
    public decimal Price { get; set; }

    [Range(0, 10000)]
    public int DisplayOrder { get; set; }

    public bool InStock { get; set; }
}
