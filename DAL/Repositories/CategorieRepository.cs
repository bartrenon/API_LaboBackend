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

    public async Task<int> CreateAsync(Categorie c)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = @"
        INSERT INTO Categorie (Nom, AgeMax, AgeMin)
        OUTPUT INSERTED.Id
        VALUES (@Nom, @AgeMax, @AgeMin);";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Nom", c.Nom);
        command.Parameters.AddWithValue("@AgeMax", c.AgeMax);
        command.Parameters.AddWithValue("@AgeMin", c.AgeMin);

        await connection.OpenAsync();
        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }

    public async Task<List<Categorie>> GetAllAsync()
    {
        List<Categorie> categories = new List<Categorie>();

        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"SELECT 
                        c.Id AS CategorieId,
                        c.Nom AS CategorieNom,
                        c.AgeMin, c.AgeMax,
                        t.Id AS TournoiId,
                        t.Nom AS TournoiNom,
                        t.Lieu, t.MinJoueurs, t.MaxJoueurs, t.EloMin, t.EloMax, t.Statut, t.RondeCourante, t.WomenOnly,
                        t.DateFinInscriptions, t.DateCreation, t.DateMiseAJour
                        FROM Categorie c
                        LEFT JOIN TournoiCategorie tc ON c.Id = tc.CategorieId
                        LEFT JOIN Tournoi t ON t.Id = tc.TournoiId;";

        using SqlCommand command = new SqlCommand(query, connection);
        await connection.OpenAsync();

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            int categorieId = Convert.ToInt32(reader["CategorieId"]);
        
            Categorie? categorie = categories.FirstOrDefault(c => c.Id == categorieId);

            if (categorie == null)
            {
            
                categorie = CategorieMapper.ToCategorieFromJoin(reader);
                categorie.Tournois = new List<Tournoi>();
                categories.Add(categorie);
            }

        
            Tournoi? tournoi = TournoiMapper.ToTournoiFromJoin(reader);

            if (tournoi != null)
            {
                categorie.Tournois.Add(tournoi);
            }
    }

    return categories;
}


    public async Task<Categorie?> GetByIdAsync(int id)
    {
        Categorie? categorie = null;
        List<Tournoi> tournois = new List<Tournoi>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = @"SELECT 
                       c.Id AS CategorieId,
                       c.Nom AS CategorieNom,
                       c.AgeMin,c.AgeMax,
                       t.Id AS TournoiId,
                       t.Nom AS TournoiNom,
                       t.Lieu,t.MinJoueurs,t.MaxJoueurs,t.EloMin,t.EloMax,t.Statut,t.RondeCourante,t.WomenOnly,
                       t.DateFinInscriptions,t.DateCreation,t.DateMiseAJour
                       FROM Categorie c
                       LEFT JOIN TournoiCategorie tc ON c.Id = tc.CategorieId
                       LEFT JOIN Tournoi t ON t.Id = tc.TournoiId
                       WHERE c.Id = @Id; ";


        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        if (reader.Read())
        {
            categorie = CategorieMapper.ToCategorieFromJoin(reader);

            Tournoi? tournoi = TournoiMapper.ToTournoiFromJoin(reader);

            if (tournoi != null) 
            {
                tournois.Add(tournoi);
            }  

            while (reader.Read())
            {
                tournoi = TournoiMapper.ToTournoiFromJoin(reader);
                if (tournoi != null) 
                {
                    tournois.Add(tournoi);
                } 
            }

            categorie.Tournois = tournois;
        }

        return categorie ;
    }
}
