using Domain.Entities;

namespace BLL.Interfaces;

public interface IInscriptionService
{
    Task<List<Inscription>> GetAllAsync();

    Task<Inscription?> GetByIdAsync(int id);

    Task<int> CreateAsync(Inscription inscription);
}
