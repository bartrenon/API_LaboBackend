using DAL.Interfaces;
using DAL.Mapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories;

public class TournoiRepository : ITournoiRepository
{
    private readonly string _connectionString;

    public TournoiRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<int> CreateAsync(Tournoi t)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"
        INSERT INTO Tournoi 
        (Nom, Lieu, MinJoueurs, MaxJoueurs, EloMin, EloMax, WomenOnly, 
         DateFinInscriptions, RondeCourante, Statut, DateCreation, DateMiseAJour)
        OUTPUT INSERTED.Id
        VALUES 
        (@Nom, @Lieu, @MinJoueurs, @MaxJoueurs, @EloMin, @EloMax, @WomenOnly,
         @DateFinInscriptions, @RondeCourante, @Statut, @DateCreation, @DateMiseAJour);";

        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Nom", t.Nom);
        command.Parameters.AddWithValue("@Lieu", t.Lieu);
        command.Parameters.AddWithValue("@MinJoueurs", t.MinJoueurs);
        command.Parameters.AddWithValue("@MaxJoueurs", t.MaxJoueurs);
        command.Parameters.AddWithValue("@EloMin", t.EloMin);
        command.Parameters.AddWithValue("@EloMax", t.EloMax);
        command.Parameters.AddWithValue("@WomenOnly", t.WomenOnly);
        command.Parameters.AddWithValue("@DateFinInscriptions", t.DateFinInscriptions);
        command.Parameters.AddWithValue("@RondeCourante", t.RondeCourante);
        command.Parameters.AddWithValue("@Statut", t.Statut);
        command.Parameters.AddWithValue("@DateCreation", t.DateCreation);
        command.Parameters.AddWithValue("@DateMiseAJour", t.DateMiseAJour);

        await connection.OpenAsync();
        return Convert.ToInt32(await command.ExecuteScalarAsync());
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
        string query = "SELECT * FROM Tournoi WHERE Id = @Id";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        if (reader.Read())
        {
            tournoi = TournoiMapper.ToTournoi(reader);
        }

        return tournoi;
    }

    public async Task<List<Tournoi>> GetLastNotClosedAsync()
    {
        List<Tournoi> tournois = new List<Tournoi>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = "SELECT TOP 10 * FROM Tournoi WHERE Statut = 'En préparation' ORDER BY DateMiseAJour DESC";

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
}
