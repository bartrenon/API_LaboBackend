namespace Domain.Entities;

public class Inscription
{
    public int Id { get; set; }
    public int JoueurId { get; set; }
    public int TournoiId { get; set; }
    public DateTime DateInscription { get; set; } = DateTime.UtcNow;

    public Tournoi Tournoi { get; set; } = null!;
    public Joueur Joueur { get; set; } = null!;
}
