using BLL.Dto;
using DAL.Dto;
using Domain.Entities;

namespace BLL.Interfaces;

public interface ITournoiService
{
    Task<List<Tournoi>> GetAllAsync();

    Task<List<Tournoi>> GetLastNotClosedAsync();

    Task<Tournoi?> GetByIdAsync(int id);

    Task<Tournoi?> GetDetails(int id);

    Task<int> CreateAsync(TournoiCreate tournoi);

    Task DeleteAsync(int id);

    Task<bool> DemarrerAsync(int id);

    Task GenererRencontresRoundRobin(Tournoi tournoi);

    Task<bool> PasserRondeSuivanteAsync(int tournoiId);

    Task<bool> CloturerTournoiAsync(int id);

    Task<List<Score>> GetScoresAsync(int tournoiId, int? ronde);

}
