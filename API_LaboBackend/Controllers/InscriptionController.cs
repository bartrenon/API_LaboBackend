using API_LaboBackend.DTO.Inscription;
using API_LaboBackend.Mappers;
using BLL.Interfaces;
using BLL.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API_LaboBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InscriptionController : ControllerBase
{
    private readonly IInscriptionService _inscriptionService;

    public InscriptionController(IInscriptionService inscriptionService)
    {
        _inscriptionService = inscriptionService;
    }

    [HttpPost]
    public async Task<ActionResult> Create(InscriptionCreate i)
    {

        Inscription inscription = InscriptionMapper.ToInscription(i);

        inscription.Id = await _inscriptionService.CreateAsync(inscription);

        return CreatedAtAction(nameof(GetById), new { id = inscription.Id }, inscription);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InscriptionAll>> GetById(int id) 
    {
        Inscription? inscription = await _inscriptionService.GetByIdAsync(id);

        if( inscription != null) 
        {
            InscriptionAll result = InscriptionMapper.ToInscriptionAll(inscription);

            return Ok(result);
        }

        return NotFound();
    }

    [HttpGet]
    public async Task<ActionResult<List<InscriptionAll>>> GetAll() 
    {
        List<Inscription> inscriptions = await _inscriptionService.GetAllAsync();

        List<InscriptionAll> results = inscriptions.Select(i => InscriptionMapper.ToInscriptionAll(i)).ToList();

        return Ok(results);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _inscriptionService.DeleteAsync(id);

        return NoContent();
    }
}
