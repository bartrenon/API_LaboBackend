using Domain.Entities;
using Microsoft.Data.SqlClient;

namespace DAL.Mappers;

public class InscriptionMapper
{
    public static Inscription ToInscription(SqlDataReader reader)
    {
        return new Inscription
        {
            Id = Convert.ToInt32(reader["Id"]),
            JoueurId = Convert.ToInt32(reader["JoueurId"]),
            TournoiId = Convert.ToInt32(reader["TournoiId"]),
            DateInscription = Convert.ToDateTime(reader["DateInscription"]),
        };
    }

    public static Inscription ToInscriptionFromJoin(SqlDataReader reader)
    {
        return new Inscription
        {
            Id = Convert.ToInt32(reader["InscriptionId"]),
            JoueurId = Convert.ToInt32(reader["JoueurId"]),
            TournoiId = Convert.ToInt32(reader["TournoiId"]),
            DateInscription = Convert.ToDateTime(reader["DateInscription"])
        };
    }

}
