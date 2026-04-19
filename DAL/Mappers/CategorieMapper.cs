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
            AgeMin = Convert.ToInt32(reader["AgeMin"])

        };
    }
}