using System.Numerics;

namespace Domain.Entities;

public class Rencontre
{
    public int Id { get; set; }
    public int Ronde { get; set; }
    public int? Resulta { get; set; }
    public int TournoiId { get; set; }
    public int JoueurBlancId { get; set; }
    public int JoueurNoirId { get; set; }

    public Tournoi Tournoi { get; set; } = null!;
    public Joueur JoueurBlanc { get; set; } = null!;
    public Joueur JoueurNoir { get; set; } = null!;
}
