using API_LaboBackend.DTO.Joueur;
using API_LaboBackend.DTO.Tournoi;

namespace API_LaboBackend.DTO.Inscription;

public class InscriptionAllInfoNotOtherObject
{
    public int Id { get; set; }
    public int JoueurId { get; set; }
    public int TournoiId { get; set; }
    public DateTime DateInscription { get; set; } = DateTime.UtcNow;
}
