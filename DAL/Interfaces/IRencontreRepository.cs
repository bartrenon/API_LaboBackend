using Domain.Entities;

namespace DAL.Interfaces;

public interface IRencontreRepository
{
    Task<List<Rencontre>> GetAllAsync();

    Task<Rencontre?> GetByIdAsync(int id);

    Task<int> CreateAsync(Rencontre rencontre);    
}

