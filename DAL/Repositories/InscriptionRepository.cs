using DAL.Interfaces;
using DAL.Mapper;
using DAL.Mappers;
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
        List<Inscription> inscriptions = new List<Inscription>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = @"SELECT 
                       i.Id AS InscriptionId,
                       i.JoueurId,i.TournoiId,i.DateInscription,
                       j.Id AS JoueurIdAlias,j.Pseudo,j.Email,j.MotDePasseHash,j.DateNaissance,j.Genre,j.Elo,
                       t.Id AS TournoiIdAlias,
                       t.Nom AS TournoiNom,
                       t.Lieu,t.MinJoueurs,t.MaxJoueurs,t.EloMin,t.EloMax,t.Statut,t.RondeCourante,t.WomenOnly,
                       t.DateFinInscriptions,t.DateCreation,t.DateMiseAJour
                       FROM Inscription i
                       JOIN Joueur j ON j.Id = i.JoueurId
                       JOIN Tournoi t ON t.Id = i.TournoiId;";

        using SqlCommand command = new SqlCommand(query, connection);

        await connection.OpenAsync();

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            Inscription inscription = InscriptionMapper.ToInscriptionFromJoinIsNotNull(reader);

            inscription.Joueur = JoueurMapper.ToJoueurFromJoin(reader);

            inscription.Tournoi = TournoiMapper.ToTournoiFromJoinNotNull(reader);

            inscriptions.Add(inscription);
        }

        return inscriptions;
    }

    public async Task<Inscription?> GetByIdAsync(int id)
    {
        Inscription? inscription = null;

        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = @"SELECT 
                       i.Id AS InscriptionId,
                       i.JoueurId,i.TournoiId,i.DateInscription,
                       j.Id AS JoueurIdAlias,j.Pseudo,j.Email,j.MotDePasseHash,j.DateNaissance,j.Genre,j.Elo,
                       t.Id AS TournoiIdAlias,
                       t.Nom AS TournoiNom,
                       t.Lieu,t.MinJoueurs,t.MaxJoueurs,t.EloMin,t.EloMax,t.Statut,t.RondeCourante,t.WomenOnly,
                       t.DateFinInscriptions,t.DateCreation,t.DateMiseAJour
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
            inscription = InscriptionMapper.ToInscriptionFromJoinIsNotNull(reader);

            inscription.Joueur = JoueurMapper.ToJoueurFromJoin(reader);

            inscription.Tournoi = TournoiMapper.ToTournoiFromJoinNotNull(reader);
        }

        return inscription;
    }
}
