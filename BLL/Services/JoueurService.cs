using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;

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
        if (await _joueurRepository.ExistsByEmailOrPseudoAsync(joueur.Pseudo,joueur.Email)) 
        {
            throw new Exception("Email ou/et pseudo est déjà utilisé");
        }

        joueur.MotDePasseHash = BCrypt.Net.BCrypt.HashPassword(joueur.MotDePasseHash);

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
