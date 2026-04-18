using DAL.Interfaces;
using DAL.Mappers;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories;

public class JoueurRepository : IJoueurRepository
{
    private readonly string _connectionString;

    public JoueurRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<int> CreateAsync(Joueur j)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = @"INSERT INTO Joueur (Pseudo, Email, MotDePasseHash, DateNaissance, Genre, Elo) 
                       OUTPUT INSERTED.Id VALUES (@Pseudo, @Email, @MotDePasseHash, @DateNaissance, @Genre, @Elo)";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Pseudo", j.Pseudo);
        command.Parameters.AddWithValue("@Email", j.Email);
        command.Parameters.AddWithValue("@MotDePasseHash", j.MotDePasseHash);
        command.Parameters.AddWithValue("@DateNaissance", j.DateNaissance);
        command.Parameters.AddWithValue("@Genre", j.Genre);
        command.Parameters.AddWithValue("@Elo", j.Elo);

        await connection.OpenAsync();
        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }

    public async Task<List<Joueur>> GetAllAsync()
    {
        List<Joueur> joueurs = new List<Joueur>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = "SELECT * FROM Joueur ";

        using SqlCommand command = new SqlCommand(query, connection);

        await connection.OpenAsync();

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            Joueur joueur = JoueurMapper.ToJoueur(reader);

            joueurs.Add(joueur);
        }

        return joueurs;
    }

    public async Task<Joueur?> GetByIdAsync(int id)
    {
        Joueur? joueur = null;

        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = "SELECT * FROM Joueur WHERE Id = @Id";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        if (reader.Read())
        {
            joueur = JoueurMapper.ToJoueur(reader);
        }

        return joueur;
    }

    public async Task<bool> ExistsByEmailOrPseudoAsync(string email, string pseudo)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = @"SELECT COUNT(*) 
                     FROM Joueur
                     WHERE Email = @Email OR Pseudo = @Pseudo";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue("@Pseudo", pseudo);

        await connection.OpenAsync();

        int count = Convert.ToInt32(await command.ExecuteScalarAsync());

        return count > 0;
    }

}
