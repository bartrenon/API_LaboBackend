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
            JoueurId = reader.GetInt32(1),
            TournoiId = reader.GetInt32(2),
            DateInscription = reader.GetDateTime(3),
        };
    }
}
