using API_LaboBackend.DTO.Categorie;
using API_LaboBackend.DTO.Inscription;
using API_LaboBackend.DTO.Rencontre;

namespace API_LaboBackend.DTO.Tournoi;
public class TournoiAll : TournoiShort
{
    public List<InscriptionShort> Inscriptions { get; set; } = new();
    public List<CategorieShort> Categories { get; set; } = new();
    public List<RencontreShort> Rencontres { get; set; } = new();
}
