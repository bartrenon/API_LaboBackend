using DAL.Dto;
using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;
using BLL.Dto;

namespace BLL.Services;

public class TournoiService : ITournoiService
{
    private readonly ITournoiRepository _tournoiRepository;
    private readonly IRencontreRepository _rencontreRepository;

    public TournoiService(ITournoiRepository tournoiRepository, IRencontreRepository rencontreRepository)
    {
        _tournoiRepository = tournoiRepository;
        _rencontreRepository = rencontreRepository;
    }

    public async Task<int> CreateAsync(TournoiCreate tournoi)
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

        if (tournoi.CategoriesIds == null || tournoi.CategoriesIds.Count == 0) 
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

    public async Task<bool> DemarrerAsync(int tournoiId)
    {
        var tournoi = await _tournoiRepository.GetDetails(tournoiId);

        if (tournoi == null) 
        {
            throw new Exception("Tournoi introuvable");
        }

        if (tournoi.JoueursInscrits.Count < tournoi.MinJoueurs) 
        {
            throw new Exception("Le nombre minimum de participants n'est pas atteint");
        }
            
        if (tournoi.DateFinInscriptions > DateTime.Now.Date) 
        {
            throw new Exception("La date de fin des inscriptions n'est pas encore dépassée");
        }
            
        await GenererRencontresRoundRobin(tournoi);

        int nouvelleRonde = 1;
        DateTime maintenant = DateTime.Now;

        return await _tournoiRepository.StartAsync(tournoi.Id, nouvelleRonde, maintenant);
    }

    public async Task GenererRencontresRoundRobin(Tournoi tournoi)
    {
        List<Joueur> joueurs = tournoi.JoueursInscrits.ToList();

        
        bool hasBye = false;
        if (joueurs.Count % 2 != 0)
        {
            joueurs.Add(new Joueur { Id = -1 }); 
            hasBye = true;
        }

        int n = joueurs.Count;
        int totalRounds = n - 1;
        int half = n / 2;

        for (int ronde = 1; ronde <= totalRounds; ronde++)
        {
            for (int i = 0; i < half; i++)
            {
                var blanc = joueurs[i];
                var noir = joueurs[n - 1 - i];

                if (blanc.Id != -1 && noir.Id != -1)
                {
                    await _rencontreRepository.CreateAsync(new Rencontre
                    {
                        TournoiId = tournoi.Id,
                        Ronde = ronde,
                        JoueurBlancId = blanc.Id,
                        JoueurNoirId = noir.Id
                    });
                }
            }

            var pivot = joueurs[1];
            joueurs.RemoveAt(1);
            joueurs.Add(pivot);
        }
    }


    public async Task<bool> PasserRondeSuivanteAsync(int tournoiId)
    {
        
        Tournoi? tournoi = await _tournoiRepository.GetDetails(tournoiId);

        if (tournoi == null) 
        {
            throw new Exception("Tournoi introuvable");
        }
            

        if (tournoi.Statut != "En cours") 
        {
            throw new Exception("Le tournoi n'est pas en cours");
        }
            
        List<Rencontre> rencontres = await _tournoiRepository.GetByTournoiAndRondeAsync(tournoiId, tournoi.RondeCourante);

        if (rencontres.Count == 0) 
        {
            throw new Exception("Aucune rencontre pour cette ronde");
        }
            
        if (rencontres.Any(r => r.Resultat == null)) 
        {
            throw new Exception("Toutes les rencontres de la ronde courante doivent être terminées");
        }
     
        int prochaineRonde = tournoi.RondeCourante + 1;

        int totalRondes = (tournoi.JoueursInscrits.Count - 1) * 2;

        if (prochaineRonde > totalRondes)
        {
            await _tournoiRepository.CloturerAsync(tournoi.Id);
            return true;
        }

        string nouveauStatut = prochaineRonde > totalRondes ? "Clôturé" : "En cours";

        return await _tournoiRepository.UpdateRondeAsync(tournoiId, prochaineRonde, nouveauStatut );
    }

    public async Task<bool> CloturerTournoiAsync(int tournoiId)
    {
        Tournoi? tournoi = await _tournoiRepository.GetDetails(tournoiId);
        
        if (tournoi == null) 
        {
            throw new Exception("Tournoi introuvable");
        }

        int totalRondes = tournoi.JoueursInscrits.Count - 1;

       
        if (tournoi.RondeCourante <= totalRondes) 
        {
            throw new Exception("Le tournoi n'est pas encore terminé");
        }
           
        
        List<Rencontre> rencontresDerniereRonde =  await _tournoiRepository.GetByTournoiAndRondeAsync(tournoiId, totalRondes);

        if (rencontresDerniereRonde.Any(r => string.IsNullOrEmpty(r.Resultat))) 
        {
            throw new Exception("Toutes les rencontres doivent être terminées avant de clôturer");
        } 

        return await _tournoiRepository.CloturerAsync(tournoiId);
    }


    public async Task<List<Score>> GetScoresAsync(int tournoiId, int? ronde)
    {
        var tournoi = await _tournoiRepository.GetDetails(tournoiId);
        if (tournoi == null)
            throw new Exception("Tournoi introuvable");

        List<Rencontre> rencontres;

        
        rencontres = await _tournoiRepository.GetByTournoiAndRondeAsync(tournoiId, ronde);

        
        var scores = tournoi.JoueursInscrits
            .Select(j => new Score
            {
                Nom = j.Pseudo,
                MatchsJoues = 0,
                Victoires = 0,
                Defaites = 0,
                Egalites = 0,
                ScoreFinal = 0
            })
            .ToDictionary(s => s.Nom);

        
        foreach (var r in rencontres)
        {
            if (string.IsNullOrEmpty(r.Resultat))
                continue;

            var blanc = tournoi.JoueursInscrits.First(j => j.Id == r.JoueurBlancId).Pseudo;
            var noir = tournoi.JoueursInscrits.First(j => j.Id == r.JoueurNoirId).Pseudo;

            scores[blanc].MatchsJoues++;
            scores[noir].MatchsJoues++;

            switch (r.Resultat)
            {
                case "1-0":
                    scores[blanc].Victoires++;
                    scores[blanc].ScoreFinal += 1;
                    scores[noir].Defaites++;
                    break;

                case "0-1":
                    scores[noir].Victoires++;
                    scores[noir].ScoreFinal += 1;
                    scores[blanc].Defaites++;
                    break;

                case "0.5-0.5":
                case "0,5-0,5": 
                    scores[blanc].Egalites++;
                    scores[noir].Egalites++;
                    scores[blanc].ScoreFinal += 0.5;
                    scores[noir].ScoreFinal += 0.5;
                    break;
            }
        }

        return scores.Values
            .OrderByDescending(s => s.ScoreFinal)
            .ThenByDescending(s => s.Victoires)
            .ToList();
    }

}
