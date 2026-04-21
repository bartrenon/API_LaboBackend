using System.Text.Json.Serialization;

namespace API_LaboBackend.DTO.Inscription;

public class InscriptionShort
{
    public int Id { get; set; }
    public DateTime DateInscription { get; set; } = DateTime.UtcNow;
    public int JoueurId { get; set; }
    public int TournoiId { get; set; }
}
