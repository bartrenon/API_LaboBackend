using API_LaboBackend.DTO.Categorie;
using API_LaboBackend.DTO.Inscription;
using API_LaboBackend.DTO.Rencontre;
using System.Text.Json.Serialization;

namespace API_LaboBackend.DTO.Tournoi;
public class TournoiAll : TournoiShort
{
    [JsonPropertyOrder(50)]
    public List<InscriptionShort> Inscriptions { get; set; } = new();

    [JsonPropertyOrder(51)]
    public List<CategorieShort> Categories { get; set; } = new();

    [JsonPropertyOrder(52)]
    public List<RencontreShort> Rencontres { get; set; } = new();
}
