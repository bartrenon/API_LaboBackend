using API_LaboBackend.DTO.Inscription;

namespace API_LaboBackend.DTO.Joueur;

public class JoueurShortInfo
{
    public string Pseudo { get; set; } = null!;
    public DateTime DateNaissance { get; set; }
    public string Genre { get; set; } = null!;
    public int Elo { get; set; }

    public List<InscriptionShortInfoNotOtherObject> Inscriptions { get; set; } = new List<InscriptionShortInfoNotOtherObject>();
}
