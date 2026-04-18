using DAL.Interfaces;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories;

public class InscriptionRepository : IInscriptionRepository
{
    private readonly string _connectionString;

    public InscriptionRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<int> CreateAsync(Inscription i)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = @"
        INSERT INTO Inscription (JoueurId, TournoiId, DateInscription)
        VALUES (@JoueurId, @TournoiId, @DateInscription);";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@JoueurId", i.JoueurId);
        command.Parameters.AddWithValue("@TournoiId", i.TournoiId);
        command.Parameters.AddWithValue("@DateInscription", i.DateInscription);

        await connection.OpenAsync();
        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }

    public async Task<List<Inscription>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Inscription?> GetByIdAsync(int id)
    {
        Inscription? inscription = null;

        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = @"
        SELECT i.*, j.*, t.*
        FROM Inscription i
        JOIN Joueur j ON j.Id = i.JoueurId
        JOIN Tournoi t ON t.Id = i.TournoiId
        WHERE i.Id = @Id;";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        if (reader.Read())
        {
            inscription = new Inscription
            {
                Id = Convert.ToInt32(reader["Id"]),
                JoueurId = reader.GetInt32(1),
                TournoiId = reader.GetInt32(2),
                DateInscription = reader.GetDateTime(3),
            };

            inscription.Joueur = new Joueur
            {
                Id = Convert.ToInt32(reader["Id"]),
                Pseudo = reader["Pseudo"].ToString() ?? "",
                Email = reader["Email"].ToString() ?? "",
                MotDePasseHash = reader["MotDePasseHash"].ToString() ?? "",
                DateNaissance = Convert.ToDateTime(reader["DateNaissance"]),
                Genre = reader["Genre"].ToString() ?? "",
                Elo = Convert.ToInt32(reader["Elo"])
            };

            inscription.Tournoi = new Tournoi
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

        return inscription;
    }
}
