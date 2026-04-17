using System.Text.RegularExpressions;

namespace Domain.Entities;

public class Tournoi
{
    public int Id { get; set; }
    public string Nom { get; set; } = null!;
    public string Lieu { get; set; } = null!;
    public int MinJoueurs { get; set; }
    public int MaxJoueurs { get; set; }
    public int? EloMin { get; set; }
    public int? EloMax { get; set; }
    public bool WomenOnly { get; set; }
    public DateTime DateFinInscriptions { get; set; }
    public int RondeCourante { get; set; } = 0;
    public string Statut { get; set; } = "en attente de joueurs";
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;
    public DateTime DateMiseAJour { get; set; } = DateTime.UtcNow;

    public IEnumerable<Categorie> Categories { get; set; } = new List<Categorie>();
    public IEnumerable<Inscription> Inscriptions { get; set; } = new List<Inscription>();
    public IEnumerable<Rencontre> Rencontres { get; set; } = new List<Rencontre>();
}
