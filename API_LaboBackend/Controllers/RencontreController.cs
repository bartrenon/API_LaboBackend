using API_LaboBackend.DTO.Rencontre;
using API_LaboBackend.Mappers;
using BLL.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API_LaboBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RencontreController : ControllerBase
{
    private readonly IRencontreService _rencontreService;

    public RencontreController(IRencontreService rencontreService)
    {
        _rencontreService = rencontreService;
    }

    [HttpPost]
    public async Task<ActionResult> Create(RencontreCreate c)
    {

        Rencontre rencontre = RencontreMapper.ToRencontre(c);

        rencontre.Id = await _rencontreService.CreateAsync(rencontre);

        return CreatedAtAction(nameof(GetById), new { id = rencontre.Id }, rencontre);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RencontreAllInfo>> GetById(int id)
    {
        Rencontre? rencontre = await _rencontreService.GetByIdAsync(id);

        if (rencontre != null)
        {
            RencontreAllInfo result = RencontreMapper.ToRencontreAllInfo(rencontre);

            return Ok(result);
        }

        return NotFound();
    }

    [HttpGet]
    public async Task<ActionResult<List<RencontreShortInfo>>> GetAll()
    {
        List<Rencontre> rencontres = await _rencontreService.GetAllAsync();

        List<RencontreShortInfo> results = rencontres.Select(r => RencontreMapper.ToRencontreShortInfo(r)).ToList();

        return Ok(results);
    }
}

