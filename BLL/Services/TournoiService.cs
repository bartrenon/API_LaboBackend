using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;
using Microsoft.Data.SqlClient;

namespace BLL.Services;

public class TournoiService : ITournoiService
{
    private readonly ITournoiRepository _tournoiRepository;

    public TournoiService(ITournoiRepository tournoiRepository)
    {
        _tournoiRepository = tournoiRepository;
    }

    public async Task<int> CreateAsync(Tournoi tournoi)
    {
        
        if (string.IsNullOrWhiteSpace(tournoi.Nom)) 
        {
            throw new ArgumentException("Le nom du tournoi est obligatoire");
        }

        if (string.IsNullOrWhiteSpace(tournoi.Lieu)) 
        {
            throw new ArgumentException("Le lieu du tournoi est obligatoire");
        }
            
        if (tournoi.MinJoueurs <= 0) 
        {
            throw new ArgumentException("Le nombre minimum de joueurs doit être supérieur à 0");
        }

        if (tournoi.MaxJoueurs <= 0) 
        {
            throw new ArgumentException("Le nombre maximum de joueurs doit être supérieur à 0");
        }

        if (tournoi.Categories == null || tournoi.Categories.Count == 0) 
        {
            throw new ArgumentException("Le tournoi doit contenir au moins une catégorie");
        }
           
        if (tournoi.MinJoueurs > tournoi.MaxJoueurs) 
        {
            throw new ArgumentException("Le nombre minimum de joueurs ne peut pas dépasser le maximum");
        }
            
        if (tournoi.EloMax != 0 && tournoi.EloMin > tournoi.EloMax) 
        {
            throw new ArgumentException("L'ELO minimum ne peut pas dépasser l'ELO maximum");
        }
            
        if (tournoi.DateFinInscriptions <= DateTime.Now.AddDays(tournoi.MinJoueurs)) 
        {
            throw new ArgumentException("La date de fin des inscriptions n'est pas valide");
        }
            
        tournoi.RondeCourante = 0;
        tournoi.Statut = "en attente de joueurs";
        tournoi.DateCreation = DateTime.Now;
        tournoi.DateMiseAJour = DateTime.Now;

        return await _tournoiRepository.CreateAsync(tournoi);
    }


    public async Task DeleteAsync(int id)
    {
        
        Tournoi? tournoi = await _tournoiRepository.GetByIdAsync(id);

        if (tournoi == null) 
        {
            throw new KeyNotFoundException("Ce tournoi n'existe pas");
        }
            
        if (tournoi.DateFinInscriptions <= DateTime.Now) 
        {
            throw new InvalidOperationException("Impossible de supprimer un tournoi qui a déjà commencé");
        }
           
        bool deleted = await _tournoiRepository.DeleteAsync(id);

        if (!deleted) 
        {
            throw new KeyNotFoundException("Tournoi introuvable");
        }
    }


    public Task<List<Tournoi>> GetAllAsync()
    {
        return _tournoiRepository.GetAllAsync();    
    }

    public Task<Tournoi?> GetByIdAsync(int id)
    {
        return _tournoiRepository.GetByIdAsync(id);
    }

    public Task<Tournoi?> GetDetails(int id)
    {
        return _tournoiRepository.GetDetails(id);
    }

    public Task<List<Tournoi>> GetLastNotClosedAsync()
    {
        return _tournoiRepository.GetLastNotClosedAsync();
    }
}
