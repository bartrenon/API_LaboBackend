using API_LaboBackend.DTO.Inscription;
using API_LaboBackend.DTO.Joueur;
using Domain.Entities;

namespace API_LaboBackend.Mappers;

public class JoueurMapper
{
    public static JoueurShortInfo ToJoueurShortInfo(Joueur j)
    {
        return new JoueurShortInfo
        {
            Pseudo = j.Pseudo,
            DateNaissance = j.DateNaissance,
            Genre = j.Genre,
            Elo = j.Elo,
            Inscriptions = j.Inscriptions != null
            ? j.Inscriptions.Select(i => InscriptionMapper.ToInscriptionShortInfoNotOtherObject(i)).ToList() : new List<InscriptionShortInfoNotOtherObject>()

        };
    }

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

    public static JoueurAllInfo ToJoueurAllInfo(Joueur j)
    {
        return new JoueurAllInfo
        {
            Id = j.Id,
            Pseudo = j.Pseudo,
            Email = j.Email,
            MotDePasseHash = j.MotDePasseHash,
            DateNaissance = j.DateNaissance,
            Genre = j.Genre,
            Elo = j.Elo,
            Inscriptions = j.Inscriptions != null
            ? j.Inscriptions.Select(i => InscriptionMapper.ToInscriptionShortInfoNotOtherObject(i)).ToList() : new List<InscriptionShortInfoNotOtherObject>()
        };
    }
}
