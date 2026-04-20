using API_LaboBackend.DTO.Inscription;

namespace API_LaboBackend.DTO.Joueur;

public class JoueurAll : JoueurShort
{
    public List<InscriptionShort> Inscriptions { get; set; } = new List<InscriptionShort> ();
}
