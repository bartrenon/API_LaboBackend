using DAL.Interfaces;
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
         @DateFinInscriptions, @RondeCourante, @Statut, @DateCreation, @DateMiseAJour);
         ";

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
            Tournoi tournoi = new Tournoi
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
            tournoi = new Tournoi
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

        return tournoi;
    }
}
