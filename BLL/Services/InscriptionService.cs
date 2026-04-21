using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;

namespace BLL.Services;

public class InscriptionService : IInscriptionService
{

    private readonly IInscriptionRepository _inscriptionRepository;
    private readonly ITournoiRepository _tournoiRepository;
    private readonly IJoueurRepository _joueurRepository;

    public InscriptionService(IInscriptionRepository inscriptionRepository, ITournoiRepository tournoiRepository, IJoueurRepository joueurRepository)
    {
        _inscriptionRepository = inscriptionRepository;

        _tournoiRepository = tournoiRepository;

        _joueurRepository = joueurRepository;
    }

    public async Task<int> CreateAsync(Inscription inscription)
    {
        Tournoi? tournoi = await _tournoiRepository.GetByIdAsync(inscription.TournoiId);

        if (tournoi == null) 
        {
            throw new Exception("Tournoi introuvable");
        }
           
        Joueur? joueur = await _joueurRepository.GetByIdAsync(inscription.JoueurId);

        if (joueur == null) 
        {
            throw new Exception("Joueur introuvable");
        }
            
        bool existe = await _inscriptionRepository.ExistsAsync(inscription.JoueurId, inscription.TournoiId);
        

        if (existe) 
        {
            throw new Exception("Le joueur est déjà inscrit à ce tournoi");
        }
           
        
        if (tournoi.DateFinInscriptions <= DateTime.Now) 
        {
            throw new Exception("Les inscriptions sont clôturées");
        }
           
        
        if (tournoi.JoueursInscrits.Count >= tournoi.MaxJoueurs) 
        {
            throw new Exception("Le tournoi est complet");
        }
           
        bool okCategorie = false;

        foreach (Categorie cat in tournoi.Categories)
        {
            int age = DateTime.Now.Year - joueur.DateNaissance.Year;

            bool ageOK = age >= cat.AgeMin && age <= cat.AgeMax;
            bool eloOK = (tournoi.EloMin == null || joueur.Elo >= tournoi.EloMin)
                      && (tournoi.EloMax == null || joueur.Elo <= tournoi.EloMax);
            bool genreOK = (!tournoi.WomenOnly || joueur.Genre == "F");

            if (ageOK && eloOK && genreOK)
            {
                okCategorie = true;
                break;
            }
        }

        if (!okCategorie) 
        {
            throw new Exception("Le joueur ne correspond à aucune catégorie du tournoi");
        }
           
        if (tournoi.Statut != "En attente de joueurs" && tournoi.Statut != "En préparation") 
        {
            throw new Exception("Le tournoi n'accepte plus d'inscriptions");
        }
            
        inscription.DateInscription = DateTime.Now;

        return await _inscriptionRepository.CreateAsync(inscription);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        
        Inscription? inscription = await _inscriptionRepository.GetByIdAsync(id);
        
        if (inscription == null) 
        {
            throw new Exception("Inscription introuvable");
        }
            

        Tournoi? tournoi = await _tournoiRepository.GetByIdAsync(inscription.TournoiId);
       
        if (tournoi == null) 
        {
            throw new Exception("Tournoi introuvable");
        }
            
        if (tournoi.Statut == "En cours" || tournoi.Statut == "Clôturé") 
        {
            throw new Exception("Impossible de se désinscrire : le tournoi a déjà commencé");
        }
           

        if (tournoi.DateFinInscriptions <= DateTime.Now) 
        {
            throw new Exception("Impossible de se désinscrire : les inscriptions sont clôturées");
        }
          
        return await _inscriptionRepository.DeleteAsync(id);
    }


    public async Task<List<Inscription>> GetAllAsync()
    {
        return await _inscriptionRepository.GetAllAsync();
    }

    public async Task<Inscription?> GetByIdAsync(int id)
    {
        return await _inscriptionRepository.GetByIdAsync(id);
    }
}
