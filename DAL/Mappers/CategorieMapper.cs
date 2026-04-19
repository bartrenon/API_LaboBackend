using Domain.Entities;
using Microsoft.Data.SqlClient;

namespace DAL.Mappers;

public class CategorieMapper
{
    public static Categorie ToCategorie(SqlDataReader reader)
    {
        return new Categorie
        {
            Id = Convert.ToInt32(reader["Id"]),
            Nom = reader["Nom"].ToString() ?? "",
            AgeMax = Convert.ToInt32(reader["AgeMax"]),
            AgeMin = Convert.ToInt32(reader["AgeMin"]),
            Tournois = new List<Tournoi>()
        };
    }

    public static Categorie ToCategorieFromJoin(SqlDataReader reader)
    {
        return new Categorie
        {
            Id = Convert.ToInt32(reader["CategorieId"]),
            Nom = reader["CategorieNom"].ToString() ?? "",
            AgeMax = Convert.ToInt32(reader["AgeMax"]),
            AgeMin = Convert.ToInt32(reader["AgeMin"]),
            Tournois = new List<Tournoi>()
        };
    }
}