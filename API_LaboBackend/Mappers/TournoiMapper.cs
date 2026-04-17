using API_LaboBackend.DTO.Tournoi;
using Domain.Entities;

namespace API_LaboBackend.Mappers;

public class TournoiMapper
{
    public static TournoiShortInfo ToTournoiShortInfo(Tournoi t)
    {
        return new TournoiShortInfo
        {
            Nom = t.Nom,
            Lieu = t.Lieu,
            DateFinInscriptions = t.DateFinInscriptions,
            Statut = t.Statut
        };
    }

    public static Tournoi ToTournoi(TournoiCreate dto)
    {
        return new Tournoi
        {
            Nom = dto.Nom,
            Lieu = dto.Lieu,
            MinJoueurs = dto.MinJoueurs,
            MaxJoueurs = dto.MaxJoueurs,
            EloMin = dto.EloMin,
            EloMax = dto.EloMax,
            WomenOnly = dto.WomenOnly,
            DateFinInscriptions = dto.DateFinInscriptions,
            RondeCourante = 0, 
            Statut = "en attente de joueurs", 
            DateCreation = DateTime.Now,
            DateMiseAJour = DateTime.Now
        };
    }

    public static TournoiAllInfo ToTournoiAllInfo(Tournoi t)
    {
        return new TournoiAllInfo
        {
            Id = t.Id,
            Nom = t.Nom,
            Lieu = t.Lieu,
            MinJoueurs = t.MinJoueurs,
            MaxJoueurs = t.MaxJoueurs,
            EloMin = t.EloMin,
            EloMax = t.EloMax,
            WomenOnly = t.WomenOnly,
            DateFinInscriptions = t.DateFinInscriptions,
            RondeCourante = t.RondeCourante,
            Statut = t.Statut,
            DateCreation = t.DateCreation,
            DateMiseAJour = t.DateMiseAJour
        };
    }

}
