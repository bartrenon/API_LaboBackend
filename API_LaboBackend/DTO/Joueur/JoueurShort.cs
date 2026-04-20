namespace API_LaboBackend.DTO.Joueur;

public class JoueurShort 
{
    public int Id { get; set; }
    public string Pseudo { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime DateNaissance { get; set; }
    public string Genre { get; set; } = null!;
    public int Elo { get; set; }
}
