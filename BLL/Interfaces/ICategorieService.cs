using Domain.Entities;

namespace BLL.Interfaces;

public interface ICategorieService
{
    Task<List<Categorie>> GetAllAsync();

    Task<Categorie?> GetByIdAsync(int id);

    Task<int> CreateAsync(Categorie categorie);
}
