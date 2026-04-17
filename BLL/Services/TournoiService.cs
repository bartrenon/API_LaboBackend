using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;

namespace BLL.Services;

public class TournoiService : ITournoiService
{
    private readonly ITournoiRepository _tournoiRepository;

    public TournoiService(ITournoiRepository tournoiRepository)
    {
        _tournoiRepository = tournoiRepository;
    }

    public Task<int> CreateAsync(Tournoi tournoi)
    {
        if(tournoi.EloMin > tournoi.EloMax && tournoi.EloMax != 0) 
        {
            throw new Exception("elo min ne peut etre superieur elo max");
        }

        if(tournoi.MinJoueurs > tournoi.MaxJoueurs) 
        {
            throw new Exception("Joueur min ne peut etre superieur Joueur max");
        }

        if (tournoi.DateFinInscriptions <= DateTime.Now.AddDays(tournoi.MinJoueurs))
        {
            throw new Exception("La date de fin des inscriptions n'est pas valide");
        }

        return _tournoiRepository.CreateAsync(tournoi);
    }

    public Task<List<Tournoi>> GetAllAsync()
    {
        return _tournoiRepository.GetAllAsync();    
    }

    public Task<Tournoi?> GetByIdAsync(int id)
    {
        return _tournoiRepository.GetByIdAsync(id);
    }
}
