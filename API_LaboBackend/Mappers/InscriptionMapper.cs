using API_LaboBackend.DTO.Inscription;
using Domain.Entities;

namespace API_LaboBackend.Mappers;

public class InscriptionMapper
{
    public static Inscription ToInscription(InscriptionCreate i)
    {
        return new Inscription
        {
            JoueurId = i.JoueurId,
            TournoiId = i.TournoiId,
            DateInscription = i.DateInscription
        };
    }

    public static InscriptionShortInfo ToInscriptionShortInfo(Inscription i)
    {
        return new InscriptionShortInfo
        {
             DateInscription = i.DateInscription,
             Tournoi = TournoiMapper.ToTournoiShortInfo(i.Tournoi),
             Joueur = JoueurMapper.ToJoueurShortInfo(i.Joueur)
        }; 
    }

    public static InscriptionAllInfo ToInscriptionAllInfo(Inscription i)
    {
        return new InscriptionAllInfo
        {
            Id = i.Id,
            JoueurId = i.JoueurId,
            TournoiId = i.TournoiId,
            DateInscription = i.DateInscription,
            Tournoi = TournoiMapper.ToTournoiShortInfo(i.Tournoi),
            Joueur = JoueurMapper.ToJoueurShortInfo(i.Joueur)
        };
    }
}