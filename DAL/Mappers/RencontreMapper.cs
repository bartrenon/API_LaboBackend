using Domain.Entities;
using Microsoft.Data.SqlClient;

namespace DAL.Mappers;

public class RencontreMapper
{
    public static Rencontre ToRencontre(SqlDataReader reader)
    {
        return new Rencontre
        {
            Id = Convert.ToInt32(reader["Id"]),
            TournoiId = Convert.ToInt32(reader["TournoiId"]),
            Ronde = Convert.ToInt32(reader["Ronde"]),
            JoueurBlancId = Convert.ToInt32(reader["JoueurBlancId"]),
            JoueurNoirId = Convert.ToInt32(reader["JoueurNoirId"]),
            Resultat = reader["Resultat"].ToString() ?? ""
        };
    }


    public static Rencontre ToRencontreFromJoin(SqlDataReader reader)
    {
        return new Rencontre
        {
            Id = Convert.ToInt32(reader["RencontreId"]),
            TournoiId = Convert.ToInt32(reader["TournoiId"]),
            Ronde = Convert.ToInt32(reader["Ronde"]),
            JoueurBlancId = Convert.ToInt32(reader["JoueurBlancId"]),
            JoueurNoirId = Convert.ToInt32(reader["JoueurNoirId"]),
            Resultat = reader["Resultat"].ToString() ?? ""
        };
    }

}
