using Domain.Entities;

namespace BLL.Interfaces;

public interface IRencontreService
{
    Task<List<Rencontre>> GetAllAsync();

    Task<Rencontre?> GetByIdAsync(int id);

    Task<int> CreateAsync(Rencontre rencontre);
}
