namespace Domain.Entities;

public class Categorie
{
    public int Id { get; set;}
    public string Nom { get; set; } = null!;
    public int AgeMin { get; set; }
    public int AgeMax { get; set; }

    public List<Tournoi> Tournois { get; set; } = new List<Tournoi>();
}
