using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;

namespace BLL.Services;

public class RencontreService : IRencontreService
{
    private readonly IRencontreRepository _rencontreRepository;
    private readonly ITournoiRepository _tournoiRepository;

    public RencontreService(IRencontreRepository rencontreRepository, ITournoiRepository tournoiRepository)
    {
        _rencontreRepository = rencontreRepository;
        _tournoiRepository = tournoiRepository;
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

    public async Task<bool> ModifierResultatAsync(int rencontreId, string resultat)
    {
        
        Rencontre? rencontre = await _rencontreRepository.GetByIdAsync(rencontreId);
        if (rencontre == null) 
        {
            throw new Exception("Rencontre introuvable");
        }
            
        Tournoi? tournoi = await _tournoiRepository.GetDetails(rencontre.TournoiId);

        if (tournoi == null) 
        {
            throw new Exception("Tournoi introuvable");
        }
            
        if (rencontre.Ronde != tournoi.RondeCourante) 
        {
            throw new Exception("Seules les rencontres de la ronde courante peuvent être modifiées");
        }
            
        return await _rencontreRepository.UpdateResultatAsync(rencontreId, resultat);
    }
}
