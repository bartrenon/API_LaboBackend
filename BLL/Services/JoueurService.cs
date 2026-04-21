using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;
using System.Text.RegularExpressions;

namespace BLL.Services;

public class JoueurService : IJoueurService
{
    private readonly IJoueurRepository _joueurRepository;
    private readonly IEmailService _emailService;

    public JoueurService(IJoueurRepository joueurRepository, IEmailService emailService)
    {
        _joueurRepository = joueurRepository;
        _emailService = emailService;
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

        string motDePasse = GenererMotDePasse();
        joueur.MotDePasseHash = BCrypt.Net.BCrypt.HashPassword(motDePasse);
        joueur.Elo = 1200;

        int result = await _joueurRepository.CreateAsync(joueur);

        await _emailService.SendPasswordEmailAsync(joueur.Email,joueur.Pseudo ,motDePasse);

        return result;
    }

    public async Task<List<Joueur>> GetAllAsync()
    {
        return await _joueurRepository.GetAllAsync();
    }

    public async Task<Joueur?> GetByIdAsync(int id)
    {
        return await _joueurRepository.GetByIdAsync(id);
    }

    public static string GenererMotDePasse(int longueur = 12)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, longueur)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
