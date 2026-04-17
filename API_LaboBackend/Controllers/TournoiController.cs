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
    public async Task<ActionResult<List<TournoiShortInfo>>> GetAll()
    {
        List<Tournoi> tournois = await _tournoiService.GetAllAsync();

        List<TournoiShortInfo> result = new List<TournoiShortInfo>();

        foreach (Tournoi tournoi in tournois)
        {
            result.Add(TournoiMapper.ToTournoiShortInfo(tournoi));
        }

        return Ok(result);
    }

    [HttpGet("recents")]
    public async Task<ActionResult<List<TournoiShortInfo>>> GetLastNotClosed()
    {
        List<Tournoi> tournois = await _tournoiService.GetLastNotClosedAsync();

        List<TournoiShortInfo> result = new List<TournoiShortInfo>();

        foreach (Tournoi tournoi in tournois)
        {
            result.Add(TournoiMapper.ToTournoiShortInfo(tournoi));
        }

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TournoiAllInfo>> GetById(int id)
    {
        Tournoi? tournoi = await _tournoiService.GetByIdAsync(id);

        if(tournoi == null)
        {
            return NotFound();
        }

        return Ok(TournoiMapper.ToTournoiAllInfo(tournoi));
    }


    [HttpPost]
    public async Task<ActionResult> Create(TournoiCreate t)
    {
        Tournoi tournoi = TournoiMapper.ToTournoi(t);

        tournoi.Id = await _tournoiService.CreateAsync(tournoi);

        return CreatedAtAction(nameof(GetById), new { id = tournoi.Id }, TournoiMapper.ToTournoiAllInfo(tournoi));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _tournoiService.DeleteAsync(id);

        return NoContent();
    }

}
