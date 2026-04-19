using API_LaboBackend.DTO.Tournoi;

namespace API_LaboBackend.DTO.Categorie;

public class CategorieAllInfo
{
    public int Id { get; set; }
    public string Nom { get; set; } = null!;
    public int AgeMin { get; set; }
    public int AgeMax { get; set; }

    public List<TournoiShortInfo> Tournois { get; set; } = new List<TournoiShortInfo>();
}
