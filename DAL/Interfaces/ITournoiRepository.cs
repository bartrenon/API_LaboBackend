using Domain.Entities;

namespace DAL.Interfaces;

public interface ITournoiRepository
{
    Task<List<Tournoi>> GetAllAsync();

    Task<Tournoi?> GetByIdAsync(int id);

    public Task<Tournoi?> GetDetails(int id);

    Task<int> CreateAsync(Tournoi tournoi);

    Task<bool> DeleteAsync(int id);

    Task<List<Tournoi>> GetLastNotClosedAsync();

    Task<bool> StartAsync(int id, int rondeCourante, DateTime dateMiseAJour);

    Task<bool> UpdateRondeAsync(int tournoiId, int nouvelleRonde, string nouveauStatut);

    Task<bool> CloturerAsync(int tournoiId);

    Task<List<Rencontre>> GetByTournoiAndRondeAsync(int tournoiId, int? ronde);
}
