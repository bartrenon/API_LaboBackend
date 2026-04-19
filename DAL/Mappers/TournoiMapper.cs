using Domain.Entities;
using Microsoft.Data.SqlClient;

namespace DAL.Mapper;

public class TournoiMapper
{
    public static Tournoi ToTournoi(SqlDataReader reader)
    {
        return new Tournoi
        {
            Id = Convert.ToInt32(reader["Id"]),
            Nom = reader["Nom"].ToString() ?? "",
            Lieu = reader["Lieu"].ToString() ?? "",
            MinJoueurs = Convert.ToInt32(reader["MinJoueurs"]),
            MaxJoueurs = Convert.ToInt32(reader["MaxJoueurs"]),
            EloMin = reader["EloMin"] == DBNull.Value ? 0 : Convert.ToInt32(reader["EloMin"]),
            EloMax = reader["EloMax"] == DBNull.Value ? 0 : Convert.ToInt32(reader["EloMax"]),
            WomenOnly = Convert.ToBoolean(reader["WomenOnly"]),
            DateFinInscriptions = Convert.ToDateTime(reader["DateFinInscriptions"]),
            RondeCourante = Convert.ToInt32(reader["RondeCourante"]),
            Statut = reader["Statut"].ToString() ?? "",
            DateCreation = Convert.ToDateTime(reader["DateCreation"]),
            DateMiseAJour = Convert.ToDateTime(reader["DateMiseAJour"])
        };
    }

    public static Tournoi ToTournoiFromJoin(SqlDataReader reader)
    {
        return new Tournoi
        {
            Id = Convert.ToInt32(reader["TournoiId"]),
            Nom = reader["TournoiNom"].ToString() ?? "",
            Lieu = reader["Lieu"].ToString() ?? "",
            MinJoueurs = Convert.ToInt32(reader["MinJoueurs"]),
            MaxJoueurs = Convert.ToInt32(reader["MaxJoueurs"]),
            EloMin = reader["EloMin"] as int?,
            EloMax = reader["EloMax"] as int?,
            Statut = reader["Statut"].ToString() ?? "",
            RondeCourante = Convert.ToInt32(reader["RondeCourante"]),
            WomenOnly = Convert.ToBoolean(reader["WomenOnly"]),
            DateFinInscriptions = Convert.ToDateTime(reader["DateFinInscriptions"]),
            DateCreation = Convert.ToDateTime(reader["DateCreation"]),
            DateMiseAJour = Convert.ToDateTime(reader["DateMiseAJour"])
        };
    }

}
