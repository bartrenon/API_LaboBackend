using Domain.Entities;
using Microsoft.Data.SqlClient;

namespace DAL.Mappers;

public class JoueurMapper
{
    public static Joueur MapToJoueur(SqlDataReader reader)
    {
        return new Joueur
        {
            Id = Convert.ToInt32(reader["Id"]),
            Pseudo = reader["Pseudo"].ToString() ?? "",
            Email = reader["Email"].ToString() ?? "",
            MotDePasseHash = reader["MotDePasseHash"].ToString() ?? "",
            DateNaissance = Convert.ToDateTime(reader["DateNaissance"]),
            Genre = reader["Genre"].ToString() ?? "",
            Elo = Convert.ToInt32(reader["Elo"])
        };
    }
}
