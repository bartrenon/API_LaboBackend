using DAL.Dto;
using Domain.Entities;

namespace DAL.Interfaces;

public interface ITournoiRepository
{
    Task<List<Tournoi>> GetAllAsync();

    Task<Tournoi?> GetByIdAsync(int id);

    public Task<Tournoi?> GetDetails(int id);
    Task<int> CreateAsync(TournoiCreate t);

    Task<bool> DeleteAsync(int id);

    Task<List<Tournoi>> GetLastNotClosedAsync();

    Task<bool> StartAsync(int id);

    Task<bool> UpdateRondeAsync(int tournoiId, int nouvelleRonde, string nouveauStatut);

    Task<bool> CloturerAsync(int Id);

    Task<List<Rencontre>> GetByTournoiAndRondeAsync(int tournoiId, int? ronde);
}
