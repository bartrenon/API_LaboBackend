using Domain.Entities;

namespace DAL.Interfaces;

public interface IInscriptionRepository
{
    Task<List<Inscription>> GetAllAsync();

    Task<Inscription?> GetByIdAsync(int id);

    Task<int> CreateAsync(Inscription inscription);
}
