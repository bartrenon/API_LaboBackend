using API_LaboBackend.DTO.Tournoi;

namespace API_LaboBackend.DTO.Categorie;

public class CategorieAll : CategorieShort
{
    public List<TournoiShort> Tournois { get; set; } = new List<TournoiShort>();
}
