using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;
using System.Text.RegularExpressions;

namespace BLL.Services;

public class JoueurService : IJoueurService
{
    private readonly IJoueurRepository _joueurRepository;

    public JoueurService(IJoueurRepository joueurRepository)
    {
        _joueurRepository = joueurRepository;
    }

    public async Task<int> CreateAsync(Joueur joueur)
    {
        if (string.IsNullOrWhiteSpace(joueur.Pseudo)) 
        {
            throw new ArgumentException("Le pseudo est obligatoire");
        }

        if (string.IsNullOrWhiteSpace(joueur.MotDePasseHash)) 
        {
            throw new ArgumentException("Le mot de passe est obligatoire");
        }

        if (Regex.IsMatch(joueur.Email,@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@" +
                                @"[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?" +
                                @"(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$")) 
        {
            throw new ArgumentException("Email invalide");
        }
            

        if (joueur.Genre is not ("H" or "F" or "A")) 
        {
            throw new ArgumentException("Genre invalide");
        }

        
        if (await _joueurRepository.ExistsByEmailOrPseudoAsync(joueur.Email, joueur.Pseudo)) 
        {
            throw new Exception("Email ou pseudo déjà utilisé");
        }
           
        joueur.MotDePasseHash = BCrypt.Net.BCrypt.HashPassword(joueur.MotDePasseHash);
        joueur.Elo = 1200;

        return await _joueurRepository.CreateAsync(joueur);
    }

    public async Task<List<Joueur>> GetAllAsync()
    {
        return await _joueurRepository.GetAllAsync();
    }

    public async Task<Joueur?> GetByIdAsync(int id)
    {
        return await _joueurRepository.GetByIdAsync(id);
    }
}
