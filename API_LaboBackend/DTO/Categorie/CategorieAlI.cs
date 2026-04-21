using API_LaboBackend.DTO.Tournoi;
using System.Text.Json.Serialization;

namespace API_LaboBackend.DTO.Categorie;

public class CategorieAll : CategorieShort
{
    [JsonPropertyOrder(50)]
    public List<TournoiShort> Tournois { get; set; } = new List<TournoiShort>();
}
