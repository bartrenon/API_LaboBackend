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

    public static RencontreShortInfo ToRencontreShortInfo(Rencontre r)
    {
        return new RencontreShortInfo
        {
            Ronde = r.Ronde,
            Resultat = r.Resultat,
            TournoiId = r.TournoiId,
            JoueurBlancId = r.JoueurBlancId,
            JoueurNoirId = r.JoueurNoirId,

            Tournoi = TournoiMapper.ToTournoiShortInfo(r.Tournoi),
            JoueurBlanc = JoueurMapper.ToJoueurShortInfo(r.JoueurBlanc),
            JoueurNoir = JoueurMapper.ToJoueurShortInfo(r.JoueurNoir)
        };
    }

    public static RencontreAllInfo ToRencontreAllInfo(Rencontre r)
    {
        return new RencontreAllInfo
        {
            Id = r.Id,
            Ronde = r.Ronde,
            Resultat = r.Resultat,
            TournoiId = r.TournoiId,
            JoueurBlancId = r.JoueurBlancId,
            JoueurNoirId = r.JoueurNoirId,

            Tournoi = TournoiMapper.ToTournoiShortInfo(r.Tournoi),
            JoueurBlanc = JoueurMapper.ToJoueurShortInfo(r.JoueurBlanc),
            JoueurNoir = JoueurMapper.ToJoueurShortInfo(r.JoueurNoir)
        };
    }


}
