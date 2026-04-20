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


    public static InscriptionAll ToInscriptionAll(Inscription i)
    {
        return new InscriptionAll
        {
            Id = i.Id,
            JoueurId = i.JoueurId,
            TournoiId = i.TournoiId,
            DateInscription = i.DateInscription,
            Tournoi = TournoiMapper.ToTournoiShort(i.Tournoi),
            Joueur = JoueurMapper.ToJoueurShort(i.Joueur)
        };
    }

    public static InscriptionShort ToInscriptionShort(Inscription i)
    {
        return new InscriptionShort
        {
            JoueurId = i.JoueurId,
            TournoiId = i.TournoiId,
            DateInscription = i.DateInscription
        };
    }
}