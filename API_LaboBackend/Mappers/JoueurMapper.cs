using API_LaboBackend.DTO.Inscription;
using API_LaboBackend.DTO.Joueur;
using Domain.Entities;

namespace API_LaboBackend.Mappers;

public class JoueurMapper
{
 
    public static Joueur ToJoueur(JoueurCreate j)
    {
        return new Joueur
        {
            Pseudo = j.Pseudo,
            Email = j.Email,
            MotDePasseHash = j.MotDePasseHash,
            DateNaissance = j.DateNaissance,
            Genre = j.Genre,
            Elo = j.Elo
        };
    }

    public static JoueurShort ToJoueurShort(Joueur j)
    {
        return new JoueurShort
        {
            Id = j.Id,
            Pseudo = j.Pseudo,
            Email = j.Email,
            DateNaissance = j.DateNaissance,
            Genre = j.Genre,
            Elo = j.Elo,
        };
    }


    public static JoueurAll ToJoueurAll(Joueur j)
    {
        return new JoueurAll
        {
            Id = j.Id,
            Pseudo = j.Pseudo,
            Email = j.Email,
            DateNaissance = j.DateNaissance,
            Genre = j.Genre,
            Elo = j.Elo,
            Inscriptions = j.Inscriptions != null
            ? j.Inscriptions.Select(i => InscriptionMapper.ToInscriptionShort(i)).ToList() : new List<InscriptionShort>()
        };
    }
}
