using System.Text.Json.Serialization;

namespace API_LaboBackend.DTO.Categorie;

public class CategorieShort 
{
    public int Id { get; set; }
    public string Nom { get; set; } = null!;

    public int AgeMin { get; set; }

    public int AgeMax { get; set; }
}
