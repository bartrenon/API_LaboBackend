using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;

namespace BLL.Services;

public class RencontreService : IRencontreService
{
    private readonly IRencontreRepository _rencontreRepository;

    public RencontreService(IRencontreRepository rencontreRepository)
    {
        _rencontreRepository = rencontreRepository;
    }

    public Task<int> CreateAsync(Rencontre rencontre)
    {
        return  _rencontreRepository.CreateAsync(rencontre);
    }

    public Task<List<Rencontre>> GetAllAsync()
    {
        return _rencontreRepository.GetAllAsync();
    }

    public Task<Rencontre?> GetByIdAsync(int id)
    {
        return _rencontreRepository.GetByIdAsync(id);
    }
}
