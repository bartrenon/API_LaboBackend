using API_LaboBackend.DTO.Inscription;
using System.Text.Json.Serialization;

namespace API_LaboBackend.DTO.Joueur;

public class JoueurAll : JoueurShort
{
    [JsonPropertyOrder(50)]
    public List<InscriptionShort> Inscriptions { get; set; } = new List<InscriptionShort> ();
}
