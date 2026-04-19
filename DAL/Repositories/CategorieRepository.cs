using DAL.Interfaces;
using DAL.Mapper;
using DAL.Mappers;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories;

public class CategorieRepository : ICategorieRepository
{
    private readonly string _connectionString;

    public CategorieRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!; ;
    }

    public async Task<int> Create(Categorie c)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = @"
        INSERT INTO Categorie (Nom, AgeMax, AgeMin)
        VALUES (@Nom, @AgeMax, @AgeMin);";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Nom", c.Nom);
        command.Parameters.AddWithValue("@AgeMax", c.AgeMax);
        command.Parameters.AddWithValue("@AgeMin", c.AgeMin);

        await connection.OpenAsync();
        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }

    public async Task<IEnumerable<Categorie>> GetAllAsync()
    {
        List<Categorie> categories = new List<Categorie>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = @"
               SELECT c.*, t.*
               FROM Categorie c
               JOIN TournoiCategorie tc ON c.Id = tc.CategorieId
               JOIN Tournoi t ON t.Id = tc.TournoiId;";

        using SqlCommand command = new SqlCommand(query, connection);

        await connection.OpenAsync();

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            Categorie inscription = InscriptionMapper.ToInscription(reader);


            inscription.Tournoi = TournoiMapper.ToTournoi(reader);

            inscriptions.Add(inscription);
        }

        return inscriptions;
    }

    public async Task<Categorie?> GetByIdAsync(int id)
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
            inscription = InscriptionMapper.ToInscription(reader);

            inscription.Joueur = JoueurMapper.ToJoueur(reader);

            inscription.Tournoi = TournoiMapper.ToTournoi(reader);
        }

        return inscription;
    }
}
