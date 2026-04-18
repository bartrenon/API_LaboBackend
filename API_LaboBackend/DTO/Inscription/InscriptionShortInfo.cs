using API_LaboBackend.DTO.Joueur;
using API_LaboBackend.DTO.Tournoi;

namespace API_LaboBackend.DTO.Inscription;

public class InscriptionShortInfo
{
    public DateTime DateInscription { get; set; } = DateTime.UtcNow;

    public TournoiShortInfo Tournoi { get; set; } = null!;
    public JoueurShortInfo Joueur { get; set; } = null!;
}
