using Domain.Entities;

namespace BLL.Interfaces;

public interface IJoueurService
{
    Task<List<Joueur>> GetAllAsync();

    Task<Joueur?> GetByIdAsync(int id);

    Task<int> CreateAsync(Joueur joueur);
}
