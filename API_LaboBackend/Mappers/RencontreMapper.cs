using API_LaboBackend.DTO.Rencontre;
using Domain.Entities;

namespace API_LaboBackend.Mappers;

public class RencontreMapper
{
    public static Rencontre ToRencontre(RencontreCreate r)
    {
        return new Rencontre
        {
            TournoiId = r.TournoiId,
            JoueurBlancId = r.JoueurBlancId,
            JoueurNoirId = r.JoueurNoirId,   
        };
    }

    public static RencontreShort ToRencontreShort(Rencontre r)
    {
        return new RencontreShort
        {
            Id = r.Id,
            Ronde = r.Ronde,
            Resultat = r.Resultat,
            TournoiId = r.TournoiId,
            JoueurBlancId = r.JoueurBlancId,
            JoueurNoirId = r.JoueurNoirId
        };
    }


    public static RencontreAll ToRencontreAll(Rencontre r)
    {
        return new RencontreAll
        {
            Id = r.Id,
            Ronde = r.Ronde,
            Resultat = r.Resultat,
            TournoiId = r.TournoiId,
            JoueurBlancId = r.JoueurBlancId,
            JoueurNoirId = r.JoueurNoirId,

            Tournoi = TournoiMapper.ToTournoiShort(r.Tournoi),
            JoueurBlanc = JoueurMapper.ToJoueurShort(r.JoueurBlanc),
            JoueurNoir = JoueurMapper.ToJoueurShort(r.JoueurNoir)
        };
    }


}
