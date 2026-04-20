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
    public async Task<ActionResult<RencontreAll>> GetById(int id)
    {
        Rencontre? rencontre = await _rencontreService.GetByIdAsync(id);

        if (rencontre != null)
        {
            RencontreAll result = RencontreMapper.ToRencontreAll(rencontre);

            return Ok(result);
        }

        return NotFound();
    }

    [HttpGet]
    public async Task<ActionResult<List<RencontreAll>>> GetAll()
    {
        List<Rencontre> rencontres = await _rencontreService.GetAllAsync();

        List<RencontreAll> results = rencontres.Select(r => RencontreMapper.ToRencontreAll(r)).ToList();

        return Ok(results);
    }
}

