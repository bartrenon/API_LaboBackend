using Domain.Entities;

namespace BLL.Interfaces;

public interface ITournoiService
{
    Task<List<Tournoi>> GetAllAsync();

    Task<Tournoi?> GetByIdAsync(int id);

    Task<int> CreateAsync(Tournoi tournoi);

    Task DeleteAsync(int id);
}
