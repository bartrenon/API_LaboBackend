using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;

namespace BLL.Services;

public class CategorieService : ICategorieService
{
    private readonly ICategorieRepository _categorieRepository;

    public CategorieService(ICategorieRepository categorieRepository)
    {
        _categorieRepository = categorieRepository;
    }

    public Task<int> CreateAsync(Categorie categorie)
    {
        return  _categorieRepository.CreateAsync(categorie);
    }

    public Task<List<Categorie>> GetAllAsync()
    {
        return _categorieRepository.GetAllAsync();
    }

    public Task<Categorie?> GetByIdAsync(int id)
    {
        return _categorieRepository.GetByIdAsync(id);
    }
}
