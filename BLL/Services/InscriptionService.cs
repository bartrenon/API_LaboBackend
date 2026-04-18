using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;

namespace BLL.Services;

public class InscriptionService : IInscriptionService
{

    private readonly IInscriptionRepository _inscriptionRepository;

    public InscriptionService(IInscriptionRepository inscriptionRepository)
    {
        _inscriptionRepository = inscriptionRepository;
    }

    public async Task<int> CreateAsync(Inscription inscription)
    {
        return await _inscriptionRepository.CreateAsync(inscription);
    }

    public async Task<List<Inscription>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Inscription?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
