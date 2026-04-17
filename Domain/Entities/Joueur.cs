namespace Domain.Entities;

public class Joueur
{
    public int Id { get; set; }
    public string Pseudo { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string MotDePasseHash { get; set; } = null!;
    public DateTime DateNaissance { get; set; }
    public string Genre { get; set; } = null!; 
    public int Elo { get; set; } = 1200;

    public IEnumerable<Inscription> Inscriptions { get; set; } = new List<Inscription>();
}
