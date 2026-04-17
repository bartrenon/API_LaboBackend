using Domain.Entities;

namespace DAL.Interfaces;

public interface ITournoiRepository
{
    Task<List<Tournoi>> GetAllAsync();

    Task<Tournoi?> GetByIdAsync(int id);

    Task<int> CreateAsync(Tournoi tournoi);
}
