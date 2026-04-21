using Domain.Entities;

namespace BLL.Interfaces;

public interface ITournoiService
{
    Task<List<Tournoi>> GetAllAsync();

    Task<List<Tournoi>> GetLastNotClosedAsync();

    Task<Tournoi?> GetByIdAsync(int id);

    Task<Tournoi?> GetDetails(int id);

    Task<int> CreateAsync(Tournoi tournoi);

    Task DeleteAsync(int id);

    Task<bool> DemarrerAsync(int tournoiId);

    Task GenererRencontresRoundRobin(Tournoi tournoi);

    Task<bool> PasserRondeSuivanteAsync(int tournoiId);

    Task<bool> CloturerTournoiAsync(int tournoiId);

}
