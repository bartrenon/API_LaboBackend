using API_LaboBackend.DTO.Tournoi;

namespace API_LaboBackend.DTO.Categorie;

public class CategorieCreate
{
    public string Nom { get; set; } = null!;
    public int AgeMin { get; set; }
    public int AgeMax { get; set; }
}
