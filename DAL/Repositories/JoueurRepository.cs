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
        string query = @"SELECT j.Id AS JoueurIdAlias,
                                j.Pseudo,j.Email,j.MotDePasseHash,j.DateNaissance,
                                j.Genre,j.elo,
                                
                                i.Id AS InscriptionId,
                                i.JoueurId,i.TournoiId,i.DateInscription

                                FROM Joueur j
                                LEFT JOIN Inscription i ON i.JoueurId = j.Id;";

        using SqlCommand command = new SqlCommand(query, connection);

        await connection.OpenAsync();

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            int JoueurId = Convert.ToInt32(reader["JoueurIdAlias"]);

            Joueur? joueur = joueurs.FirstOrDefault(j => j.Id == JoueurId);

            if(joueur == null) 
            {
                joueur = JoueurMapper.ToJoueurFromJoin(reader);
                joueur.Inscriptions = new List<Inscription>();
                joueurs.Add(joueur);
            }

            Inscription? inscription = InscriptionMapper.ToInscriptionFromJoin(reader);

            if(inscription != null)
            {
                joueur.Inscriptions.Add(inscription);
            }
        }

        return joueurs;
    }

    public async Task<Joueur?> GetByIdAsync(int id)
    {
        Joueur? joueur = null;
        List<Inscription> inscriptions = new List<Inscription>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        string query = @"SELECT j.Id AS JoueurIdAlias,
                                j.Pseudo,j.Email,j.MotDePasseHash,j.DateNaissance,
                                j.Genre,j.elo,
                                
                                i.Id AS InscriptionId,
                                i.JoueurId,i.TournoiId,i.DateInscription

                                FROM Joueur j
                                LEFT JOIN Inscription i ON i.JoueurId = j.Id
                                WHERE j.Id = @Id;";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        if (reader.Read())
        {
            joueur = JoueurMapper.ToJoueurFromJoin(reader);

            Inscription? inscription = InscriptionMapper.ToInscriptionFromJoin(reader); 

            if(inscription != null) 
            {
                inscriptions.Add(inscription);
            }

            while (reader.Read())
            {
                inscription = InscriptionMapper.ToInscriptionFromJoin(reader);
                if(inscription != null) 
                {
                    inscriptions.Add(inscription);
                }
            }
        }

        if (joueur != null) 
        {
            joueur.Inscriptions = inscriptions;
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
