using API_LaboBackend.DTO.Joueur;
using API_LaboBackend.DTO.Tournoi;
using System.Text.Json.Serialization;

namespace API_LaboBackend.DTO.Rencontre;

public class RencontreAll : RencontreShort
{
    [JsonPropertyOrder(50)]
    public TournoiShort Tournoi { get; set; } = null!;
    [JsonPropertyOrder(51)]
    public JoueurShort JoueurBlanc { get; set; } = null!;
    [JsonPropertyOrder(52)]
    public JoueurShort JoueurNoir { get; set; } = null!;
}
