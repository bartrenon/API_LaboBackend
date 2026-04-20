using API_LaboBackend.DTO.Joueur;
using API_LaboBackend.DTO.Tournoi;

namespace API_LaboBackend.DTO.Rencontre;

public class RencontreShortInfo
{
    public int Ronde { get; set; }
    public string? Resultat { get; set; }
    public int TournoiId { get; set; }
    public int JoueurBlancId { get; set; }
    public int JoueurNoirId { get; set; }

    public TournoiShortInfo Tournoi { get; set; } = null!;
    public JoueurShortInfo JoueurBlanc { get; set; } = null!;
    public JoueurShortInfo JoueurNoir { get; set; } = null!;
}
