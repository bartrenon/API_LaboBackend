using API_LaboBackend.DTO.Categorie;
using API_LaboBackend.DTO.Tournoi;
using Domain.Entities;

namespace API_LaboBackend.Mappers;

public class CategorieMapper
{
    public static CategorieAll ToCategorieAll(Categorie c)
    {
        return new CategorieAll
        {
            Id = c.Id,
            Nom = c.Nom,
            AgeMin = c.AgeMin,
            AgeMax = c.AgeMax,
            Tournois = c.Tournois != null
            ? c.Tournois.Select(t => TournoiMapper.ToTournoiShort(t)).ToList() : new List<TournoiShort>()
        };
    }


    public static CategorieShort ToCategorieShort(Categorie c)
    {
        return new CategorieShort
        {
            Id = c.Id,      
            Nom = c.Nom,
            AgeMin = c.AgeMin,
            AgeMax = c.AgeMax,
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
