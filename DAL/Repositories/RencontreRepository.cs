using DAL.Interfaces;
using DAL.Mapper;
using DAL.Mappers;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories;

public class RencontreRepository : IRencontreRepository
{
    private readonly string _connectionString;

    public RencontreRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<int> CreateAsync(Rencontre r)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = @"INSERT INTO Rencontre (TournoiId, Ronde, JoueurBlancId, JoueurNoirId, Resultat)
                         OUTPUT INSERTED.Id
                         VALUES (@TournoiId, @Ronde, @JoueurBlancId, @JoueurNoirId, @Resultat)";


        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@TournoiId", r.TournoiId);
        command.Parameters.AddWithValue("@Ronde", r.Ronde);
        command.Parameters.AddWithValue("@JoueurBlancId", r.JoueurBlancId);
        command.Parameters.AddWithValue("@JoueurNoirId", r.JoueurNoirId);
        command.Parameters.AddWithValue("@Resultat", (object?)r.Resultat ?? DBNull.Value);

        await connection.OpenAsync();
        return Convert.ToInt32(await command.ExecuteScalarAsync());

    }

    public async Task<List<Rencontre>> GetAllAsync()
    {
        List<Rencontre> rencontres = new List<Rencontre>();

        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"SELECT r.Id AS RencontreId,
                                r.TournoiId,r.Ronde,r.JoueurBlancId,r.JoueurNoirId,r.Resultat,

                                jb.Id AS JoueurBlancIdAlias,
                                jb.Pseudo AS JoueurBlancPseudo,
                                jb.Email AS JoueurBlancEmail,
                                jb.Elo AS JoueurBlancElo,
                                jb.DateNaissance AS JoueurBlancDateNaissance,
                                jb.Genre AS JoueurBlancGenre,

                                jn.Id AS JoueurNoirIdAlias,
                                jn.Pseudo AS JoueurNoirPseudo,
                                jn.Email AS JoueurNoirEmail,
                                jn.Elo AS JoueurNoirElo,
                                jn.DateNaissance AS JoueurNoirDateNaissance,
                                jn.Genre AS JoueurNoirGenre,

                                t.Id AS TournoiIdAlias,
                                t.Nom AS TournoiNom,
                                t.Lieu,t.MinJoueurs,t.MaxJoueurs,t.EloMin,t.EloMax,t.Statut,t.RondeCourante,
                                t.WomenOnly,t.DateFinInscriptions,t.DateCreation,t.DateMiseAJour

                                FROM Rencontre r
                                JOIN Joueur jb ON jb.Id = r.JoueurBlancId
                                JOIN Joueur jn ON jn.Id = r.JoueurNoirId
                                JOIN Tournoi t ON t.Id = r.TournoiId;";

        using SqlCommand command = new SqlCommand(query, connection);

        await connection.OpenAsync();

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            Rencontre rencontre = RencontreMapper.ToRencontreFromJoin(reader);

            rencontre.JoueurBlanc = JoueurMapper.ToJoueurBlanc(reader);

            rencontre.JoueurNoir = JoueurMapper.ToJoueurNoir(reader);

            rencontre.Tournoi = TournoiMapper.ToTournoiFromJoinNotNull(reader);

            rencontres.Add(rencontre);
        }

        return rencontres;
    }

    public async Task<Rencontre?> GetByIdAsync(int id)
    {
        Rencontre? rencontre = null;

        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = @"SELECT r.Id AS RencontreId,
                                r.TournoiId,r.Ronde,r.JoueurBlancId,r.JoueurNoirId,r.Resultat,

                                jb.Id AS JoueurBlancIdAlias,
                                jb.Pseudo AS JoueurBlancPseudo,
                                jb.Email AS JoueurBlancEmail,
                                jb.Elo AS JoueurBlancElo,
                                jb.DateNaissance AS JoueurBlancDateNaissance,
                                jb.Genre AS JoueurBlancGenre,

                                jn.Id AS JoueurNoirIdAlias,
                                jn.Pseudo AS JoueurNoirPseudo,
                                jn.Email AS JoueurNoirEmail,
                                jn.Elo AS JoueurNoirElo,
                                jn.DateNaissance AS JoueurNoirDateNaissance,
                                jn.Genre AS JoueurNoirGenre,

                                t.Id AS TournoiIdAlias,
                                t.Nom AS TournoiNom,
                                t.Lieu,t.MinJoueurs,t.MaxJoueurs,t.EloMin,t.EloMax,t.Statut,t.RondeCourante,
                                t.WomenOnly,t.DateFinInscriptions,t.DateCreation,t.DateMiseAJour

                                FROM Rencontre r
                                JOIN Joueur jb ON jb.Id = r.JoueurBlancId
                                JOIN Joueur jn ON jn.Id = r.JoueurNoirId
                                JOIN Tournoi t ON t.Id = r.TournoiId
                                WHERE r.Id = @Id;";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();

        using SqlDataReader reader = await command.ExecuteReaderAsync();


        if (reader.Read())
        {
            rencontre = RencontreMapper.ToRencontreFromJoin(reader);

            rencontre.JoueurBlanc = JoueurMapper.ToJoueurBlanc(reader);

            rencontre.JoueurNoir = JoueurMapper.ToJoueurNoir(reader);

            rencontre.Tournoi = TournoiMapper.ToTournoiFromJoinNotNull(reader);
        }

        return rencontre;
    }
}
