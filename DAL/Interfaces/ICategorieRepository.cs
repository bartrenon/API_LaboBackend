using Domain.Entities;

namespace DAL.Interfaces;

public interface ICategorieRepository
{
    Task<IEnumerable<Categorie>> GetAllAsync(); 
    Task<Categorie?> GetByIdAsync(int id);
    Task<int> Create(Categorie categorie);
}
