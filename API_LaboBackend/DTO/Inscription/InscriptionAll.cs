using API_LaboBackend.DTO.Joueur;
using API_LaboBackend.DTO.Tournoi;

namespace API_LaboBackend.DTO.Inscription;

public class InscriptionAll : InscriptionShort
{
    public TournoiShort Tournoi { get; set; } = null!;
    public JoueurShort Joueur { get; set; } = null!;
}
