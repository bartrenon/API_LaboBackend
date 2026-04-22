using DAL.Dto;
using DAL.Interfaces;
using DAL.Mapper;
using DAL.Mappers;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DAL.Repositories;

public class TournoiRepository : ITournoiRepository
{
    private readonly string _connectionString;

    public TournoiRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<int> CreateAsync(TournoiCreate t)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using SqlTransaction transaction = connection.BeginTransaction();

        try
        {
            string query = @"INSERT INTO Tournoi 
                             (Nom, Lieu, MinJoueurs, MaxJoueurs, EloMin, EloMax, WomenOnly, 
                             DateFinInscriptions, RondeCourante, Statut, DateCreation, DateMiseAJour)
                             OUTPUT INSERTED.Id
                             VALUES (@Nom, @Lieu, @MinJoueurs, @MaxJoueurs, @EloMin, @EloMax, @WomenOnly,
                             @DateFinInscriptions, 0, 'en attente de joueurs', GETDATE(), GETDATE());";

            using SqlCommand command = new SqlCommand(query, connection, transaction);

            command.Parameters.Add("@Nom", SqlDbType.NVarChar, 100).Value = t.Nom;
            command.Parameters.Add("@Lieu", SqlDbType.NVarChar, 100).Value = t.Lieu;
            command.Parameters.Add("@MinJoueurs", SqlDbType.Int).Value = t.MinJoueurs;
            command.Parameters.Add("@MaxJoueurs", SqlDbType.Int).Value = t.MaxJoueurs;
            command.Parameters.Add("@EloMin", SqlDbType.Int).Value = (object?)t.EloMin ?? DBNull.Value;
            command.Parameters.Add("@EloMax", SqlDbType.Int).Value = (object?)t.EloMax ?? DBNull.Value;
            command.Parameters.Add("@WomenOnly", SqlDbType.Bit).Value = t.WomenOnly;
            command.Parameters.Add("@DateFinInscriptions", SqlDbType.Date).Value = t.DateFinInscriptions;

            int idTournoi = Convert.ToInt32(await command.ExecuteScalarAsync());

            if (t.CategoriesIds != null && t.CategoriesIds.Count > 0)
            {
                foreach (int catId in t.CategoriesIds)
                {
                    string queryCat = @"INSERT INTO TournoiCategorie (TournoiId, CategorieId)
                                        VALUES (@TournoiId, @CategorieId);";

                    using SqlCommand cmdCat = new SqlCommand(queryCat, connection, transaction);
                    cmdCat.Parameters.Add("@TournoiId", SqlDbType.Int).Value = idTournoi;
                    cmdCat.Parameters.Add("@CategorieId", SqlDbType.Int).Value = catId;

                    await cmdCat.ExecuteNonQueryAsync();
                }
            }

            transaction.Commit();

            return idTournoi;
        }
        catch 
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = @"DELETE FROM Tournoi WHERE Id = @Id AND 
                            GETDATE() > (SELECT DateFinInscriptions FROM Tournoi WHERE Id = @Id)";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        int rowsAffected = await command.ExecuteNonQueryAsync();

        return rowsAffected > 0;
    }

    public async Task<List<Tournoi>> GetAllAsync()
    {
        List<Tournoi> tournois = new List<Tournoi>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = "SELECT * FROM Tournoi ";

        using SqlCommand command = new SqlCommand(query, connection);

        await connection.OpenAsync();

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            Tournoi tournoi = TournoiMapper.ToTournoi(reader);

            tournois.Add(tournoi);
        }

        return tournois;
    }

    public async Task<Tournoi?> GetByIdAsync(int id)
    {
        Tournoi? tournoi = null;

        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        string queryTournoi = @"SELECT * FROM Tournoi WHERE Id = @Id;

                                SELECT * FROM Inscription WHERE TournoiId = @Id;

                                SELECT c.* FROM Categorie c
                                INNER JOIN TournoiCategorie tc ON tc.CategorieId = c.Id
                                WHERE tc.TournoiId = @Id;

                                SELECT * FROM Rencontre  WHERE TournoiId = @Id ";

        using SqlCommand cmd = new SqlCommand(queryTournoi, connection);

        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

        using SqlDataReader reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            tournoi = TournoiMapper.ToTournoi(reader);
        }

        if (tournoi == null) 
        {
            return null;
        }

        await reader.NextResultAsync();

        while (await reader.ReadAsync())
        {
            tournoi.Inscriptions.Add(InscriptionMapper.ToInscription(reader));
        }

        await reader.NextResultAsync();

        while (await reader.ReadAsync())
        {
            tournoi.Categories.Add(CategorieMapper.ToCategorie(reader));
        }

        await reader.NextResultAsync();

        while (await reader.ReadAsync())
        {
            tournoi.Rencontres.Add(RencontreMapper.ToRencontre(reader));
        }

        return tournoi;
    }


    public async Task<List<Tournoi>> GetLastNotClosedAsync()
    {
        List<Tournoi> tournois = new List<Tournoi>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = @"
        SELECT TOP 10 *
        FROM Tournoi
        WHERE Statut <> 'Clôturé'
        ORDER BY DateMiseAJour DESC";

        using SqlCommand command = new SqlCommand(query, connection);

        await connection.OpenAsync();

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            Tournoi tournoi = TournoiMapper.ToTournoi(reader);
            tournois.Add(tournoi);
        }

        return tournois;
    }

    public async Task<Tournoi?> GetDetails(int id)
    {
        Tournoi? tournoi = null;

        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        string query= @"SELECT * FROM Tournoi WHERE Id = @Id;

                        SELECT j.* FROM Inscription i 
                        INNER JOIN Joueur j ON j.Id = i.JoueurId WHERE i.TournoiId = @Id;
                        
                        SELECT c.* FROM Categorie c 
                        INNER JOIN TournoiCategorie tc ON tc.CategorieId = c.Id WHERE tc.TournoiId = @Id;
                        
                        SELECT r.* FROM Rencontre r
                        INNER JOIN Tournoi t ON t.Id = r.TournoiId
                        WHERE t.Id = @Id AND r.Ronde = t.RondeCourante;";

        using SqlCommand cmd = new SqlCommand(query, connection);
        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

        using SqlDataReader reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            tournoi = TournoiMapper.ToTournoi(reader);
        }

        if (tournoi == null) 
        {
            return null;
        }

        await reader.NextResultAsync();

        while (await reader.ReadAsync()) 
        {
            tournoi.JoueursInscrits.Add(JoueurMapper.ToJoueur(reader));
        }

        await reader.NextResultAsync();

        while (await reader.ReadAsync())
        {
            tournoi.Categories.Add(CategorieMapper.ToCategorie(reader));
        }

        await reader.NextResultAsync();

        while (await reader.ReadAsync())
        {
            tournoi.Rencontres.Add(RencontreMapper.ToRencontre(reader));
        }

        return tournoi;
    }

    public async Task<bool> StartAsync(int id)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = @"
            UPDATE Tournoi
            SET Statut = 'En cours',
                RondeCourante = 1,
                DateMiseAJour = GETDATE()
            WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(query, connection);
        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

        await connection.OpenAsync();
        int rows = await cmd.ExecuteNonQueryAsync();

        return rows > 0;
    }

    public async Task<bool> CloturerAsync(int id)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"
        UPDATE Tournoi
        SET Statut = 'Clôturé',
            DateMiseAJour = GETDATE()
        WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(query, connection);
        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

        await connection.OpenAsync();
        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> UpdateRondeAsync(int tournoiId, int nouvelleRonde, string nouveauStatut)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"
        UPDATE Tournoi
        SET RondeCourante = @Ronde,
            Statut = @Statut,
            DateMiseAJour = GETDATE()
        WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@Id", tournoiId);
        cmd.Parameters.AddWithValue("@Ronde", nouvelleRonde);
        cmd.Parameters.AddWithValue("@Statut", nouveauStatut);

        await connection.OpenAsync();
        return await cmd.ExecuteNonQueryAsync() > 0;
    }


    public async Task<List<Rencontre>> GetByTournoiAndRondeAsync(int tournoiId, int? ronde)
    {
        List<Rencontre> list = new();

        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = @"
        SELECT Id, JoueurBlancId, JoueurNoirId, Resultat
        FROM Rencontre
        WHERE TournoiId = @TournoiId";

        if (ronde.HasValue)
            query += " AND Ronde = @Ronde";

        using SqlCommand cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@TournoiId", tournoiId);

        if (ronde.HasValue)
            cmd.Parameters.AddWithValue("@Ronde", ronde.Value);

        await connection.OpenAsync();
        using SqlDataReader reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(new Rencontre
            {
                Id = reader.GetInt32(0),
                JoueurBlancId = reader.GetInt32(1),
                JoueurNoirId = reader.GetInt32(2),
                Resultat = reader.IsDBNull(3) ? null : reader.GetString(3)
            });
        }

        return list;
    }

}
