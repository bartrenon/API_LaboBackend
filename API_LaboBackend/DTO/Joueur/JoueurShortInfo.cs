namespace API_LaboBackend.DTO.Joueur;

public class JoueurShortInfo
{
    public string Pseudo { get; set; } = null!;
    public DateTime DateNaissance { get; set; }
    public string Genre { get; set; } = null!;
    public int Elo { get; set; } = 1200;
}
