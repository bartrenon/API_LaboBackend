using API_LaboBackend.DTO.Inscription;
using API_LaboBackend.DTO.Joueur;
using API_LaboBackend.Mappers;
using BLL.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API_LaboBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JoueurController : ControllerBase
{
    private readonly IJoueurService _joueurService;

    public JoueurController(IJoueurService joueurService)
    {
        _joueurService = joueurService;
    }

    [HttpGet]
    public async Task<ActionResult<List<JoueurShortInfo>>> GetAll()
    {
        List<Joueur> joueurs = await _joueurService.GetAllAsync();

        List<JoueurShortInfo> results = new List<JoueurShortInfo>();

        foreach (Joueur joueur in joueurs)
        {
            results.Add(JoueurMapper.ToJoueurShortInfo(joueur));
        }

        return Ok(results);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<JoueurAllInfo>> GetById(int id)
    {
        Joueur? joueur = await _joueurService.GetByIdAsync(id);

        if(joueur == null) { 
            return NotFound();
        }

        return Ok(JoueurMapper.ToJoueurAllInfo(joueur));

    }


    [HttpPost]
    public async Task<ActionResult> Create(JoueurCreate j)
    {

        Joueur joueur = JoueurMapper.ToJoueur(j);

        joueur.Id = await _joueurService.CreateAsync(joueur);

        return CreatedAtAction(nameof(GetById), new { id = joueur.Id }, joueur);
    }


}
