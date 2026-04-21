using API_LaboBackend.DTO.Joueur;
using API_LaboBackend.DTO.Tournoi;
using System.Text.Json.Serialization;

namespace API_LaboBackend.DTO.Inscription;

public class InscriptionAll : InscriptionShort
{
    [JsonPropertyOrder(50)]
    public TournoiShort Tournoi { get; set; } = null!;
    [JsonPropertyOrder(51)]
    public JoueurShort Joueur { get; set; } = null!;
}
