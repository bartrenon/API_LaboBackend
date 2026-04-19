using API_LaboBackend.DTO.Categorie;
using API_LaboBackend.DTO.Tournoi;
using Domain.Entities;
using Microsoft.Data.SqlClient;

namespace API_LaboBackend.Mappers;

public class CategorieMapper
{
    public static CategorieAllInfo ToCategorieAllInfo(Categorie c)
    {
        return new CategorieAllInfo
        {
            Id = c.Id,
            Nom = c.Nom,
            AgeMin = c.AgeMin,
            AgeMax = c.AgeMax,
            Tournois = c.Tournois != null
            ? c.Tournois.Select(t => TournoiMapper.ToTournoiShortInfo(t)).ToList() : new List<TournoiShortInfo>()
        };
    }

    public static CategorieShortInfo ToCategorieShortInfo(Categorie c)
    {
        return new CategorieShortInfo
        {
            Nom = c.Nom,
            AgeMin = c.AgeMin,
            AgeMax = c.AgeMax,
            Tournois = c.Tournois != null
            ? c.Tournois.Select(t => TournoiMapper.ToTournoiShortInfo(t)).ToList() : new List<TournoiShortInfo>()
        };
    }

    public static Categorie ToCategorie(CategorieCreate c)
    {
        return new Categorie
        {
            Nom = c.Nom,
            AgeMin = c.AgeMin,
            AgeMax = c.AgeMax
        };
    }
}
