using API_LaboBackend.DTO.Joueur;
using API_LaboBackend.DTO.Tournoi;

namespace API_LaboBackend.DTO.Rencontre;

public class RencontreAll : RencontreShort
{
  
    public TournoiShort Tournoi { get; set; } = null!;
    public JoueurShort JoueurBlanc { get; set; } = null!;
    public JoueurShort JoueurNoir { get; set; } = null!;
}
