using API_LaboBackend.DTO.Tournoi;
using API_LaboBackend.Mappers;
using BLL.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API_LaboBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TournoiController : ControllerBase
{
    private readonly ITournoiService _tournoiService;

    public TournoiController(ITournoiService tournoiService)
    {
        _tournoiService = tournoiService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TournoiShort>>> GetAll()
    {
        List<Tournoi> tournois = await _tournoiService.GetAllAsync();

        List<TournoiShort> result = new List<TournoiShort>();

        foreach (Tournoi tournoi in tournois)
        {
            result.Add(TournoiMapper.ToTournoiShort(tournoi));
        }

        return Ok(result);
    }

    [HttpGet("recents")]
    public async Task<ActionResult<List<TournoiAll>>> GetLastNotClosed()
    {
        List<Tournoi> tournois = await _tournoiService.GetLastNotClosedAsync();

        List<TournoiAll> result = new List<TournoiAll>();

        foreach (Tournoi tournoi in tournois)
        {
            result.Add(TournoiMapper.ToTournoiAll(tournoi));
        }

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TournoiAll>> GetById(int id)
    {
        Tournoi? tournoi = await _tournoiService.GetByIdAsync(id);

        if(tournoi == null)
        {
            return NotFound();
        }

        return Ok(TournoiMapper.ToTournoiAll(tournoi));
    }


    [HttpGet("Details/{id}")]
    public async Task<ActionResult<TournoiDetails>> GetByDetails(int id)
    {
        Tournoi? tournoi = await _tournoiService.GetDetails(id);

        if (tournoi == null)
        {
            return NotFound();
        }

        return Ok(TournoiMapper.ToTournoiDetails(tournoi));
    }


    [HttpPost]
    public async Task<ActionResult> Create(TournoiCreate t)
    {
        Tournoi tournoi = TournoiMapper.ToTournoi(t);

        tournoi.Id = await _tournoiService.CreateAsync(tournoi);

        return CreatedAtAction(nameof(GetById), new { id = tournoi.Id }, TournoiMapper.ToTournoiAll(tournoi));
    }

    [HttpPost("{id}/demarrer")]
    public async Task<IActionResult> Demarrer(int id)
    {
        
        bool ok = await _tournoiService.DemarrerAsync(id);
            
        if (!ok) 
        {
            return NotFound();
        }

        return Ok(new { message = "Tournoi démarré avec succès." });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _tournoiService.DeleteAsync(id);

        return NoContent();
    }

    [HttpPost("{id}/next-round")]
    public async Task<IActionResult> PasserRonde(int id)
    {
        
        bool ok = await _tournoiService.PasserRondeSuivanteAsync(id);

        if (!ok) 
        {
            return NotFound();
        }
        
        return Ok(new { message = "Passage à la ronde suivante effectué." });
        
    }

    [HttpPost("{id}/cloturer")]
    public async Task<IActionResult> Cloturer(int id)
    {
        
         bool ok = await _tournoiService.CloturerTournoiAsync(id);

        if (!ok) 
        {
            return NotFound();
        }    

        return Ok(new { message = "Tournoi clôturé avec succès." });
        
    }

    [HttpGet("{id}/scores")]
    public async Task<IActionResult> GetScores(int id, int? ronde)
    {
         
        var scores = await _tournoiService.GetScoresAsync(id, ronde);
            
        return Ok(scores);
       
    }
}
