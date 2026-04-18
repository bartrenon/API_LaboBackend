using API_LaboBackend.DTO.Inscription;
using API_LaboBackend.Mappers;
using BLL.Interfaces;
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
    public async Task<ActionResult<InscriptionAllInfo>> GetById(int id) 
    {
        Inscription? inscription = await _inscriptionService.GetByIdAsync(id);

        if( inscription != null) 
        {
            InscriptionAllInfo result = InscriptionMapper.ToInscriptionAllInfo(inscription);

            return Ok(result);
        }

        return NotFound();
    }

    [HttpGet]
    public async Task<ActionResult<List<InscriptionShortInfo>>> GetAll() 
    {
        List<Inscription> inscriptions = await _inscriptionService.GetAllAsync();

        List<InscriptionShortInfo> results = inscriptions.Select(i => InscriptionMapper.ToInscriptionShortInfo(i)).ToList();

        return Ok(results);
    }
}
