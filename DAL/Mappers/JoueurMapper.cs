using Domain.Entities;
using Microsoft.Data.SqlClient;

namespace DAL.Mappers;

public class JoueurMapper
{
    public static Joueur ToJoueur(SqlDataReader reader)
    {
        return new Joueur
        {
            Id = Convert.ToInt32(reader["Id"]),
            Pseudo = reader["Pseudo"].ToString() ?? "",
            Email = reader["Email"].ToString() ?? "",
            MotDePasseHash = reader["MotDePasseHash"].ToString() ?? "",
            DateNaissance = Convert.ToDateTime(reader["DateNaissance"]),
            Genre = reader["Genre"].ToString() ?? "",
            Elo = Convert.ToInt32(reader["Elo"]),
            Inscriptions = new List<Inscription>()
        };
    }

    public static Joueur ToJoueurFromJoin(SqlDataReader reader)
    {
        return new Joueur
        {
            Id = Convert.ToInt32(reader["JoueurIdAlias"]),
            Pseudo = reader["Pseudo"].ToString() ?? "",
            Email = reader["Email"].ToString() ?? "",
            MotDePasseHash = reader["MotDePasseHash"].ToString() ?? "",
            DateNaissance = Convert.ToDateTime(reader["DateNaissance"]),
            Genre = reader["Genre"].ToString() ?? "",
            Elo = Convert.ToInt32(reader["Elo"]),
            Inscriptions = new List<Inscription>()
        };
    }

    public static Joueur ToJoueurBlanc(SqlDataReader reader)
    {
        return new Joueur
        {
            Id = Convert.ToInt32(reader["JoueurBlancIdAlias"]),
            Pseudo = reader["JoueurBlancPseudo"].ToString() ?? "",
            Email = reader["JoueurBlancEmail"].ToString() ?? "",
            Elo = Convert.ToInt32(reader["JoueurBlancElo"]),
            Genre = reader["JoueurBlancGenre"].ToString() ?? "",
            DateNaissance = Convert.ToDateTime(reader["JoueurBlancDateNaissance"]),
        };
    }

    public static Joueur ToJoueurNoir(SqlDataReader reader)
    {
        return new Joueur
        {
            Id = Convert.ToInt32(reader["JoueurNoirIdAlias"]),
            Pseudo = reader["JoueurNoirPseudo"].ToString() ?? "",
            Email = reader["JoueurNoirEmail"].ToString() ?? "",
            Elo = Convert.ToInt32(reader["JoueurNoirElo"]),
            Genre = reader["JoueurNoirGenre"].ToString() ?? "",
            DateNaissance = Convert.ToDateTime(reader["JoueurNoirDateNaissance"]),
        };
    }
}
