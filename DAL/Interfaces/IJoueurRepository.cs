using Domain.Entities;

namespace DAL.Interfaces;

public interface IJoueurRepository
{
    Task<List<Joueur>> GetAllAsync();

    Task<Joueur?> GetByIdAsync(int id);

    Task<int> CreateAsync(Joueur joueur);

    Task<bool> ExistsByEmailOrPseudoAsync(string email, string pseudo);

}
